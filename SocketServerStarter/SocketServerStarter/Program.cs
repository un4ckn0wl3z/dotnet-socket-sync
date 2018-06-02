using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketServerStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddr = IPAddress.Any;
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            try
            {

                listenerSocket.Bind(ipep);

                listenerSocket.Listen(5);

                Console.WriteLine("About to accept incoming connection.");

                Socket client = listenerSocket.Accept();
                Console.WriteLine("Client connected. " + client.ToString() + " - IP End Point " + client.RemoteEndPoint.ToString());

                byte[] buffer = new byte[128];

                int numberOfReceivedBytes = 0;

                while (true)
                {

                    numberOfReceivedBytes = client.Receive(buffer);

                    Console.WriteLine("Number of received bytes: " + numberOfReceivedBytes);

                    Console.WriteLine("Data sent by client is: " + buffer);

                    string recievedText = Encoding.ASCII.GetString(buffer, 0, numberOfReceivedBytes);

                    string recievedTexts = recievedText.Trim();

                    Console.WriteLine("Data sent by client is: " + recievedText);

                    client.Send(Encoding.ASCII.GetBytes(recievedTexts + "\r\n"));

                    if (recievedTexts == "x")
                    {
                        break;
                    }

                    Array.Clear(buffer, 0, buffer.Length);
                    numberOfReceivedBytes = 0;
                }
            }
            catch (Exception excp){
                Console.WriteLine(excp.ToString());
            }

        }
    }
}
