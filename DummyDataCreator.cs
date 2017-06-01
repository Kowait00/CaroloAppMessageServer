using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
        int port;
        private volatile bool _shouldStop = false;

        public DummyDataCreator(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Sends tesMessages as UDP packets repeatedly 
        /// to loopback address over specified port (1 packet per second)
        /// </summary>
        private void sendMessages()
        {
            _shouldStop = false;
            IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            int i = 0;
            while (!_shouldStop)
            {
                byte[] data = createDummyDataByteArray(i); //testDataBytes;
                server.SendTo(data, data.Length, SocketFlags.None, RemoteEndPoint);
                Debug.WriteLine("Dummy UDP Packet sent");
                i++;
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

        public byte[] createDummyDataByteArray(int seed)
        {
            double poseX = .2 * seed * Math.Cos(.02 * seed * Math.PI);
            double poseY = .2 * seed * Math.Sin(.02 * seed * Math.PI);
            double posePsi = 0.2 * (seed % 16);
            double movementS = 0.5 * (seed % 10);
            double movementV = 0.5 * (seed % 10);
            double movementA = 0.25 * (seed % 10);
            double rotPsi_K = 0.2 * (seed % 9) - 0.8;
            double rotPsi_Ko = 0.2 * (seed % 9) - 0.8;
            double rotYaw_K = 0.2 * (seed % 9) - 0.8;
            double rotYaw_Ko = 0.2 * (seed % 9) - 0.8;
            double envUsFront = 7 - 0.5 * (seed % 15) + 0.001;
            double envUsRear = 7 - 0.5 * (seed % 15) + 0.001;
            double envIR_Side_Front = 0.65 - 0.05 * (seed % 14) + 0.001;
            double envIR_Side_Rear = 0.65 - 0.05 * (seed % 14) + 0.001;
            double envIR_Front_Left = 0.65 - 0.05 * (seed % 14) + 0.001;
            double valideSideDisLeft = 0;
            double valideSideDisRight = 0;

            byte[] bytes = BitConverter.GetBytes(poseX).Concat(BitConverter.GetBytes(poseY)).Concat(BitConverter.GetBytes(posePsi))
                .Concat(BitConverter.GetBytes(movementS)).Concat(BitConverter.GetBytes(movementV)).Concat(BitConverter.GetBytes(movementA))
                .Concat(BitConverter.GetBytes(rotPsi_K)).Concat(BitConverter.GetBytes(rotPsi_Ko))
                .Concat(BitConverter.GetBytes(rotYaw_K)).Concat(BitConverter.GetBytes(rotYaw_Ko))
                .Concat(BitConverter.GetBytes(envUsFront)).Concat(BitConverter.GetBytes(envUsRear))
                .Concat(BitConverter.GetBytes(envIR_Side_Front)).Concat(BitConverter.GetBytes(envIR_Side_Rear)).Concat(BitConverter.GetBytes(envIR_Front_Left))
                .Concat(BitConverter.GetBytes(valideSideDisLeft)).Concat(BitConverter.GetBytes(valideSideDisRight)).ToArray();

            Console.Write("Generated Dummy Data: ");
            for(int i = 0; i < bytes.Length / sizeof(double); i++)
            {
                Console.Write(BitConverter.ToDouble(bytes, i * sizeof(double)) + "; ");
            }
            Console.WriteLine();
            return bytes;
        }

    }
}
