using System.Net.Sockets;
using System.Net;
using System.Text;

namespace PW_110724_DateTimeServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, 11000);
            serverSocket.Bind(endPoint);
            serverSocket.Listen(10);

            try
            {
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();
                    byte[] buffer = new byte[1024];
                    int receivedBytes = clientSocket.Receive(buffer);
                    string clientRequest = Encoding.ASCII.GetString(buffer, 0, receivedBytes);

                    string response = string.Empty;
                    if (clientRequest.Equals("date", StringComparison.OrdinalIgnoreCase))
                    {
                        response = DateTime.Now.ToShortDateString();
                    }
                    else if (clientRequest.Equals("time", StringComparison.OrdinalIgnoreCase))
                    {
                        response = DateTime.Now.ToLongTimeString();
                    }

                    clientSocket.Send(Encoding.ASCII.GetBytes(response));
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
