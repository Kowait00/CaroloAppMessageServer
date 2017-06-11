using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace CaroloAppMessageServer
{
    public partial class Form1 : Form
    {
        // Global variables to set up the sender and receiver properties
        IPAddress multicastAddress = IPAddress.Parse("224.0.0.251");
        int multicastPort = 8625;
        int receiverPort = 27000;

        DummyDataCreator dummyDataCreator = null;
        UdpPacketReceiver packetReceiver = null;
        UdpBroadcastSender broadcastSender = null;
        BackgroundWorker networkCommunicationBackgroundWorker = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Is triggered when the button for starting/stopping the server is toggled.
        /// Checks for valid setup variablesset by the user in the Setup group box 
        /// and, when they are valid, starts/stops background worker for receiving 
        /// and sending UDP packets according to the setup variables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runServerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            // If run server button got unchecked stop everything that was started
            if (!runServerCheckbox.Checked)
            {
                setupGroupBox.Enabled = true;
                runServerCheckbox.Text = "Start Server";
                //Stop background worker for receiving and sending UDP packages
                if (networkCommunicationBackgroundWorker != null) networkCommunicationBackgroundWorker.CancelAsync();
                if (packetReceiver != null) packetReceiver.CloseSocket();
                if (dummyDataCreator != null) dummyDataCreator.RequestStop();
            }
            else
            {
                // If the run server box got checked, check for valid setup variables
                // and start the UDP reception and transmission.
                // Also start sending dummy data if checkbox was checked.

                bool validSetupVars = true;     //Only if all setup variables are valid the server can be started

                runServerCheckbox.Text = "Stop Server";

                // receiverPort input in range of possible ports?
                if (!Int32.TryParse(receiverPortInputTextBox.Text, out receiverPort)
                    || receiverPort <= 0 || receiverPort > 65535)
                {
                    validSetupVars = false;
                    receiverPortWarningLabel.Visible = true;
                }
                else receiverPortWarningLabel.Visible = false;

                if (senderInterfaceComboBox.SelectedItem == null)
                {
                    validSetupVars = false;
                    senderInterfaceWarningLabel.Visible = true;
                }
                else senderInterfaceWarningLabel.Visible = false;


                if (validSetupVars)
                {
                    setupGroupBox.Enabled = false;
                    
                    try
                    {
                        // Initialization of receiver and sender for later usage in background worker
                        KeyValuePair<String, IPAddress> localNetworkInterface = (KeyValuePair<String, IPAddress>) senderInterfaceComboBox.SelectedItem;
                        packetReceiver = new UdpPacketReceiver(receiverPort);
                        broadcastSender = new UdpBroadcastSender(localNetworkInterface.Value, multicastAddress, multicastPort);
                        senderInterfaceWarningLabel.Visible = false;

                        // Start receiving and sending UDP packets in background worker
                        networkCommunicationBackgroundWorker = new BackgroundWorker();
                        networkCommunicationBackgroundWorker.WorkerReportsProgress = true;
                        networkCommunicationBackgroundWorker.WorkerSupportsCancellation = true;
                        networkCommunicationBackgroundWorker.DoWork += new DoWorkEventHandler(networkCommunicationBackgroundWorker_DoWork);
                        networkCommunicationBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(networkCommunicationBackgroundWorker_ProgressChanged);
                        networkCommunicationBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(networkCommunicationBackgroundWorker_RunWorkerCompleted);
                        networkCommunicationBackgroundWorker.RunWorkerAsync(receiverPort);

                        // If checkbox checked start sending dummy data
                        if (dummyDataCheckBox.Checked)
                        {
                            dummyDataCreator = new DummyDataCreator(receiverPort);
                            dummyDataCreator.StartSendingDummyData();
                        }
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        // The selected network interface couldn't be used, (it's probably not connected to a network)
                        // Prompt user to choose another one. 
                        senderInterfaceWarningLabel.Visible = true;
                        validSetupVars = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        validSetupVars = false;
                    }
                }
                // If any of the setup variable values are invalid reset GUI to unstarted state
                if (!validSetupVars)
                {
                    runServerCheckbox.Checked = false;                    
                    runServerCheckbox.Text = "Start Server";
                    setupGroupBox.Enabled = true;
                }
            }

        }

        /// <summary>
        /// Prevents values other than digits from being entered into the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void receiverPortInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            //Only accept digits, backspace and delete in the textbox
            if(!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void senderInterfaceComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            senderInterfaceWarningLabel.Visible = false;
        }

        /// <summary>
        /// Scrolls textbox automatically to the bottom when new text is added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataReceivedOutputTextBox_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            dataReceivedOutputTextBox.SelectionStart = dataReceivedOutputTextBox.Text.Length;
            // scroll it automatically
            dataReceivedOutputTextBox.ScrollToCaret();
        }

        /// <summary>
        /// Retreives all network interfaces and adds them to the dropdown of the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void senderInterfaceComboBox_DropDown(object sender, EventArgs e)
        {
            senderInterfaceComboBox.Items.Clear();
            List<KeyValuePair<String, IPAddress>> networkInterfaces = NetworkInterfaceCollector.FindNetworkInterfaces();
            foreach(KeyValuePair<String, IPAddress> nIf in networkInterfaces)
            {
                senderInterfaceComboBox.Items.Add(nIf);
            }
            
        }

        /// <summary>
        /// Background worker for network communication.
        /// Receives UDP packages and sends them on via UDP multicast
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void networkCommunicationBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            networkCommunicationBackgroundWorker.ReportProgress(0, "Server started\nWaiting for UDP packets");
            int numberReceivedPackets = 0;
            DateTime lastProgressReport = DateTime.Now;
            while (!networkCommunicationBackgroundWorker.CancellationPending)
            {
                byte[] data = packetReceiver.ReceivePacket();
                if (data.Length > 0)
                {
                    numberReceivedPackets++;
                    broadcastSender.SendPacket(data);

                    if((DateTime.Now - lastProgressReport).TotalMilliseconds >= 1000)
                    {
                        networkCommunicationBackgroundWorker.ReportProgress(0, numberReceivedPackets + " Packets received and sent on");
                        lastProgressReport = DateTime.Now;
                        numberReceivedPackets = 0;
                    }
                }
            }

            e.Result = "stopped";
        }

        /// <summary>
        /// Output the contents of newly received UDP packages by the backgroundworker to the output textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void networkCommunicationBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string results = (string)e.UserState;
            dataReceivedOutputTextBox.AppendText(results + "\n");
        }

        /// <summary>
        /// Output completed message into the output textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void networkCommunicationBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                dataReceivedOutputTextBox.AppendText("Server " + (string)e.Result + "\n");
            }
            catch (ObjectDisposedException)
            {
                // Window was closed, no need to display result
            }
        }

        /// <summary>
        /// When form is closed, release all resources properly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Stop receiving and sending background worker
            if (networkCommunicationBackgroundWorker != null) networkCommunicationBackgroundWorker.CancelAsync();
            if (packetReceiver != null) packetReceiver.CloseSocket();
            if (dummyDataCreator != null) dummyDataCreator.RequestStop();
        }
    }
}
