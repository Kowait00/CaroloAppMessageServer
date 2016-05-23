using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaroloAppMessageServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Setup GUI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


            //Setup variables:
            IPAddress multicastAddress = IPAddress.Parse("224.0.0.251");   //Multicast: 224.0.0.251 Android: "192.168.178.56" Desktop: "192.168.178.48"
            int portToReceiveOn = 8626;
            int multicastPort = 8625;
            List<KeyValuePair<String, IPAddress>> netIntfList;
            int listElement = 2;        //Number of the element the correct network interface is stored in
            IPAddress localAddress;   //IP Address of the network interface used to send the multicast


            //start a thread that generates udp packets on the loopback ip address for testing
            TestMessageCreator testMsgCreator = new TestMessageCreator(portToReceiveOn);
            testMsgCreator.startSendingMessages();

            //Find all network interfaces
            netIntfList = NetworkInterfaceCollector.findNetworkInterfaces();
            localAddress = netIntfList.ElementAt(listElement).Value;

            //Start receiving and sending on UDP packets
            UdpPacketReceiver packetReceiver = new UdpPacketReceiver(portToReceiveOn);
            UdpBroadcastSender broadcastSender = new UdpBroadcastSender(localAddress, multicastAddress, multicastPort);
            while(true)
            {
                byte[] data = packetReceiver.receivePacket();
                broadcastSender.sendPacket(data);
            }

            //Request the TestMessageCreator to stop sending messages
            testMsgCreator.requestStop();

        }
    }
}
