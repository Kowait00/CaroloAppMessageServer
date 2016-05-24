﻿using System;
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
        // Global variables to set up the sender and receiver properties
        IPAddress multicastAddress = IPAddress.Parse("224.0.0.251");
        int multicastPort = 8625;
        int receiverPort = 8626;

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

        private void runServerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if(!runServerCheckbox.Checked)
            {
                setupGroupBox.Enabled = true;
                runServerCheckbox.Text = "Start Server";
                //Stop receiving and sending background worker
                if (networkCommunicationBackgroundWorker != null) networkCommunicationBackgroundWorker.CancelAsync();
                if (packetReceiver != null) packetReceiver.CloseSocket();
                if (dummyDataCreator != null) dummyDataCreator.requestStop();
            }
            else
            {
                // If the run server box got checked, check for valid setup variables
                // and start the UDP reception and transmission.
                // Also start sending dummy data if checkbox was checked.
                // If run server box got unchecked stop everything that was started

                bool validSetupVars = true;     //Only if all setup variables are valid the server can be started

                runServerCheckbox.Text = "Stop Server";

                // receiverPort input in range of possible ports
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
                            dummyDataCreator.startSendingDummyData();
                        }
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        senderInterfaceWarningLabel.Visible = true;
                        validSetupVars = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        validSetupVars = false;
                    }
                }
                if (!validSetupVars)
                {
                    //If setup variable values are invalid reset GUI to unstarted state
                    runServerCheckbox.Checked = false;                    
                    runServerCheckbox.Text = "Start Server";
                    setupGroupBox.Enabled = true;
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

        private void senderInterfaceComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            senderInterfaceWarningLabel.Visible = false;
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
            networkCommunicationBackgroundWorker.ReportProgress(0, "Server started\nWaiting for UDP packets");
            while (!networkCommunicationBackgroundWorker.CancellationPending)
            {
                byte[] data = packetReceiver.receivePacket();
                //Thread.Sleep(1000);
                //byte[] data = System.Text.Encoding.ASCII.GetBytes("TestDataThatWasNotReallyReceived");
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
            dataReceivedOutputTextBox.AppendText("Server " + result + "\n");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Stop receiving and sending background worker
            if (networkCommunicationBackgroundWorker != null) networkCommunicationBackgroundWorker.CancelAsync();
            if (packetReceiver != null) packetReceiver.CloseSocket();
            if (dummyDataCreator != null) dummyDataCreator.requestStop();
        }
    }
}
