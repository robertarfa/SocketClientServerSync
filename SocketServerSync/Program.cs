using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerSync
{
    internal class Program
    {

        public class SyncServerSocket
        {
            public static string data = null;

            public static void StartListener()
            {
                byte[] buffer = new byte[1024];

                var hostName = Dns.GetHostName();
                IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
                IPAddress ipAddress = iPHostEntry.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 45500);

                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


                try
                {

                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    while (true)
                    {
                        Console.WriteLine($"Waiting for a connection...");

                        Socket handler = listener.Accept();
                        data = null;

                        while (true)
                        {
                            int byteRec = handler.Receive(buffer);
                            data += Encoding.ASCII.GetString(buffer, 0, byteRec);

                            if (data.IndexOf("<EOF>") == -1)
                            {
                                break;
                            }
                        }

                        Console.WriteLine($"Text received: {data}");

                        byte[] msg = Encoding.ASCII.GetBytes(data);
                        handler.Send(msg);
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();


                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("Press any key to exit!");
                Console.ReadLine();

            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to continue....");
            Console.ReadLine();

            SyncServerSocket.StartListener();
            Console.ReadLine();
        }
    }
}
