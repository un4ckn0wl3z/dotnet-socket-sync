using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = null;
            client = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddr = null;

            try
            {
                Console.WriteLine("**** Welcome to Socket Client Starter ****");
                Console.WriteLine("Please Type a Valid Server IP Address and Press ENTER: ");
                string strIPAddress = Console.ReadLine();

                Console.WriteLine("Please Supply a Valid Port Number 0 - 65535 and Press ENTER: ");
                string strPort = Console.ReadLine();
                int intPort = 0;

                if (!IPAddress.TryParse(strIPAddress, out ipaddr))
                {
                    Console.WriteLine("Invalid server IP supplied.");
                    return;

                }

                if (!int.TryParse(strPort.Trim(), out intPort))
                {
                    Console.WriteLine("Invalid port number supplied.");
                    return;

                }

                if (intPort <= 0 || intPort > 65535)
                {
                    Console.WriteLine("Port number must be between 0 and 65535.");
                    return;

                }

                Console.WriteLine(string.Format("IPAddress: {0} - Port: {1}", ipaddr.ToString(), intPort));

                client.Connect(ipaddr, intPort);

                // Console.ReadKey();

                Console.WriteLine("Connected to the server, type text and press ENTER to send it to the server, type <EXIT> to close.");

                string inputCommand = string.Empty;

                while (true)
                {
                    inputCommand = Console.ReadLine();
                    if (inputCommand.Equals("<EXIT>"))
                    {
                        break;
                    }
                    byte[] bufferSend = Encoding.ASCII.GetBytes(inputCommand);

                    client.Send(bufferSend);

                    byte[] bufferReceived = new byte[128];

                    int nRecv = client.Receive(bufferReceived);


                    Console.WriteLine("Data received: " + Encoding.ASCII.GetString(bufferReceived, 0, nRecv));


                }
            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.ToString());
            }
            finally {
                Console.WriteLine("FINAL ");
                if (client != null) {
                    if (client.Connected) {
                        client.Shutdown(SocketShutdown.Both);
                    }
                    client.Close();
                    client.Dispose();
                }
            }
            Console.WriteLine("Press a key to exit... ");
            Console.ReadKey();
        }
    }
}
