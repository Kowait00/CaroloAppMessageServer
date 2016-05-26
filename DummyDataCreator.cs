using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaroloAppMessageServer
{
    /// <summary>
    /// Creates dummy UDP packages on specified port and sends them over the localhost loopback address.
    /// Test package data can be edited in the testMessages array.
    /// </summary>
    class DummyDataCreator
    {
        string[] testMessages = new string[] {"Wuba", "Duba", "Dub", "Dub", "This is a really long string see if it breaks anything so don't even bother reading because it isd just gibberish. adkfljasdlkfjalkejflkasdvnlkadnvalkdjfalksdjfawlekfk Blablablubb"};
        int port;
        private volatile bool _shouldStop = false;

        public DummyDataCreator(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Sends contents of tesMessages as UDP packets repeatedly 
        /// to loopback address over specified port (1 packet per second)
        /// </summary>
        private void sendMessages()
        {
            _shouldStop = false;
            IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            int i = 0;
            while(!_shouldStop) { 
                byte[] data = Encoding.ASCII.GetBytes(testMessages[i]);
                server.SendTo(data, data.Length, SocketFlags.None, RemoteEndPoint);
                Debug.WriteLine("Dummy UDP Packet sent");
                i++;
                i = i % testMessages.Length;
                Thread.Sleep(1000);
            }
            Debug.WriteLine("Stopped sending dummy messages");
        }

        /// <summary>
        /// Starts a new thread that sends dummy UDP packets
        /// </summary>
        public void startSendingDummyData()
        {
            Thread msgSenderThread = new Thread(new ThreadStart(sendMessages));
            msgSenderThread.Start();
        }

        /// <summary>
        /// Stops the sending of dummy UDP packets
        /// </summary>
        public void requestStop()
        {
            _shouldStop = true;
        }

    }
}
