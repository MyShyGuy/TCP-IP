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
        private readonly List<TcpClient> _connectedClients = new List<TcpClient>(); // liste für verbundene clients
        private readonly object _lock = new object(); //Lock damit falls mehrere async tasks versuchen auf die gleichen sachen zuzugreifen wird dieses unterbunden.

        private readonly Dictionary<string, TcpClient> _loggedInUsers = new Dictionary<string, TcpClient>();

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
                Console.WriteLine($"{DateTimeOffset.Now} Client {_clientCounter} connected!");

                int readUserName;
                var tcpStream = client.GetStream();
                byte[] buffer = new byte[1024];
                string userName = "Anonymus";

                if ((readUserName = await tcpStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    userName = Encoding.UTF8.GetString(buffer, 0, readUserName);
                    TimeOnly currenttime = TimeOnly.FromDateTime(DateTime.Now);
                    Console.WriteLine($"{currenttime} Client {userName}: connected");

                    await BroadcastMessageAsync($"Client {userName}: connected", client);

                    /* // Option: Sende eine Antwort zurück
                    string response = $"Server received: {receivedMessage}";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    await tcpStream.WriteAsync(responseBytes, 0, responseBytes.Length); */


                    lock (_lock)
                    {
                        _connectedClients.Add(client);
                        _loggedInUsers.Add(userName, client);
                    }

                }

                // Starte einen separaten Task für den Client
                _ = HandleClientAsync(client, _clientCounter, userName);
            }
        }

        private async Task HandleClientAsync(TcpClient client, int clientId, string clientName)
        {
            byte[] buffer = new byte[1024];

            try
            {
                var tcpStream = client.GetStream();
                int readTotal;

                while ((readTotal = await tcpStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, readTotal);
                    TimeOnly currenttime = TimeOnly.FromDateTime(DateTime.Now);
                    Console.WriteLine($"{currenttime} Client {clientId} {clientName}: {receivedMessage}");

                    await BroadcastMessageAsync($"{clientName}: {receivedMessage}", client);
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


        private async Task BroadcastMessageAsync(string message, TcpClient senderClient)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            lock (_lock)
            {
                foreach (var client in _connectedClients)
                {
                    if (client != senderClient && client.Connected)
                    {
                        try
                        {
                            var stream = client.GetStream();
                            stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine($"Error broadcasting to client: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}