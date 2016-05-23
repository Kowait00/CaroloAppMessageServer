using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaroloAppMessageServer
{
    public partial class Form1 : Form
    {
        DummyDataCreator dummyDataCreator = null;

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
                if(dummyDataCreator != null) dummyDataCreator.requestStop(); 
            }
            else
            {
                // If the run server box got checked, check for valid setup variables
                // and start the UDP reception and transmission.
                // Also start sending dummy data if checkbox was checked.
                // If run server box got unchecked stop everything that was started

                bool validSetupVars = true;     //Only if all setup variables are valid the server can be started
                int receiverPort;

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
                }
                else
                {
                    //If setup variable values are invalid uncheck checkboxbutton
                    runServerCheckbox.Checked = false;
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

        private void senderInterfaceComboBox_DropDown(object sender, EventArgs e)
        {
            senderInterfaceComboBox.Items.Clear();
            List<KeyValuePair<String, IPAddress>> networkInterfaces = NetworkInterfaceCollector.findNetworkInterfaces();
            foreach(KeyValuePair<String, IPAddress> nIf in networkInterfaces)
            {
                senderInterfaceComboBox.Items.Add(nIf);
            }
            
        }
    }
}
