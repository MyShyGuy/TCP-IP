using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TCPIP_Chat
{


    public class TCPServer
    {
        private TcpListener? _tcpListener;
        private int _clientCounter = 0;

        public TCPServer()
        {
            StartServer();
        }

        private async Task StartServer()
        {
            var port = 13000;
            var hostAddress = IPAddress.Parse("127.0.0.1");
            _tcpListener = new TcpListener(hostAddress, port);
            _tcpListener.Start();

            Console.WriteLine($"Server started on {hostAddress}:{port}");
            Console.WriteLine("Waiting for clients...");

            while (true)
            {
                // Warte auf neuen Client
                TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                _clientCounter++;
                Console.WriteLine($"Client {_clientCounter} connected!");

                // Starte einen separaten Task für den Client
                _ = HandleClientAsync(client, _clientCounter);
            }
        }

        private async Task HandleClientAsync(TcpClient client, int clientId)
        {
            byte[] buffer = new byte[1024];

            try
            {
                var tcpStream = client.GetStream();
                int readTotal;

                while ((readTotal = await tcpStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, readTotal);
                    Console.WriteLine($"Client {clientId}: {receivedMessage}");

                    // Option: Sende eine Antwort zurück
                    string response = $"Server received: {receivedMessage}";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    await tcpStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client {clientId}: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Client {clientId} disconnected.");
                client.Close();
            }
        }
    }








    /*     public class TCPServer
        {
            string? recivedMessage;
            private TcpListener? _tcpListener;

            private int _clientCounter = 0;

            public TCPServer()
            {
                StartServer();
            }

            private async Task StartServer()
            {
                var port = 13000;
                var hostAddress = IPAddress.Parse("127.0.0.1");
                _tcpListener = new TcpListener(hostAddress, port);
                _tcpListener.Start();

                byte[] buffer = new byte[1024];

                using TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                _clientCounter++;

                var tcpStream = client.GetStream();

                int readTotal;

                while ((readTotal = await tcpStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    recivedMessage = Encoding.UTF8.GetString(buffer, 0, readTotal);
                    showMessage();
                }
            }

                    public async Task CheckForNewClient()
                    {
                        using TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                    } 

            public void showMessage()
            {
                if (recivedMessage != null)
                {
                    System.Console.WriteLine($"Time: {DateTimeOffset.Now} \t | \t Message:" + recivedMessage);
                    recivedMessage = null;
                }
            }
        } */
}