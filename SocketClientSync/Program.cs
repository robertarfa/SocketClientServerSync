using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientSync
{
    internal class Program
    {

        public class SyncSocketClient
        {
            public static void StartClient()
            {
                byte[] buffer = new byte[1024];

                try
                {
                    var hostName = Dns.GetHostName();
                    IPHostEntry iPHostInfo = Dns.GetHostEntry(hostName);

                    Console.WriteLine($"Host: {hostName}");

                    IPAddress ipAddress = iPHostInfo.AddressList[0];

                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 45500);

                    Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        client.Connect(ipEndPoint);
                        Console.WriteLine("Socket Connected");

                        client.RemoteEndPoint.ToString();
                        byte[] msg = Encoding.ASCII.GetBytes("This is just a test");
                        int byteSent = client.Send(msg);
                        int byteRec = client.Receive(buffer);

                        Console.WriteLine($"Echoed test {Encoding.ASCII.GetString(buffer, 0, byteRec)}");

                        //release the socket
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }

        }

        static void Main(string[] args)
        {

            Console.WriteLine("Press enter to continue....");
            Console.ReadLine();


            SyncSocketClient.StartClient();
            Console.ReadLine();
        }
    }
}
