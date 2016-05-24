using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaroloAppMessageServer
{
    public partial class Form1 : Form
    {
        DummyDataCreator dummyDataCreator = null;
        IPAddress multicastAddress = IPAddress.Parse("224.0.0.251");
        int receiverPort = 8626;
        int multicastPort = 8625;
        UdpPacketReceiver packetReceiver = null;
        UdpBroadcastSender broadcastSender = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void runServerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if(!runServerCheckbox.Checked)
            {
                setupGroupBox.Enabled = true;
                runServerCheckbox.Text = "Start Server";
                if(dummyDataCreator != null) dummyDataCreator.requestStop();
                //Stop receiving and sending background worker
                networkCommunicationBackgroundWorker.CancelAsync();
                packetReceiver.stopReceiving();
            }
            else
            {
                // If the run server box got checked, check for valid setup variables
                // and start the UDP reception and transmission.
                // Also start sending dummy data if checkbox was checked.
                // If run server box got unchecked stop everything that was started

                bool validSetupVars = true;     //Only if all setup variables are valid the server can be started

                runServerCheckbox.Text = "Stop Server";

                if (!Int32.TryParse(receiverPortInputTextBox.Text, out receiverPort))
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
                    if (dummyDataCheckBox.Checked)
                    {
                        dummyDataCreator = new DummyDataCreator(receiverPort);
                        dummyDataCreator.startSendingDummyData();
                    }
                    try
                    {
                        // Initialization of receiver and sender for later usage in background worker
                        KeyValuePair<String, IPAddress> localNetworkInterface = (KeyValuePair<String, IPAddress>) senderInterfaceComboBox.SelectedItem;
                        packetReceiver = new UdpPacketReceiver(receiverPort);
                        broadcastSender = new UdpBroadcastSender(localNetworkInterface.Value, multicastAddress, multicastPort);
                        // Start receiving and sending UDP packets
                        networkCommunicationBackgroundWorker.RunWorkerAsync(receiverPort);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(e.ToString());
                        //TODO: Handle if backgroundworker is started again whiled
                        //shutdown is still pending
                    }
                }
                else
                {
                    //If setup variable values are invalid uncheck checkboxbutton
                    runServerCheckbox.Checked = false;
                    runServerCheckbox.Text = "Start Server";
                }
            }

        }

        private void receiverPortInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            //Only accept digits, backspace and delete in the textbox
            if(!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void dataReceivedOutputTextBox_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            dataReceivedOutputTextBox.SelectionStart = dataReceivedOutputTextBox.Text.Length;
            // scroll it automatically
            dataReceivedOutputTextBox.ScrollToCaret();
        }

        private void senderInterfaceComboBox_DropDown(object sender, EventArgs e)
        {
            senderInterfaceComboBox.Items.Clear();
            List<KeyValuePair<String, IPAddress>> networkInterfaces = NetworkInterfaceCollector.findNetworkInterfaces();
            foreach(KeyValuePair<String, IPAddress> nIf in networkInterfaces)
            {
                senderInterfaceComboBox.Items.Add(nIf);
            }
            
        }

        /*
         * Background worker for network communication:
         */
        private void networkCommunicationBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!networkCommunicationBackgroundWorker.CancellationPending)
            {
                //byte[] data = packetReceiver.receivePacket();
                Thread.Sleep(1000);
                byte[] data = System.Text.Encoding.ASCII.GetBytes("TestDataThatWasNotReallyReceived");
                Console.WriteLine("Packet received and sent on");
                broadcastSender.sendPacket(data);
                
                networkCommunicationBackgroundWorker.ReportProgress(0, Encoding.ASCII.GetString(data, 0, data.Length));
            }
            //packetReceiver = null;
            //broadcastSender = null;
            e.Result = "stopped";
        }

        private void networkCommunicationBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string results = (string)e.UserState;
            dataReceivedOutputTextBox.AppendText(results + "\n");
        }

        private void networkCommunicationBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = (string)e.Result;
            dataReceivedOutputTextBox.AppendText("Server " + result);
        }

    }
}
