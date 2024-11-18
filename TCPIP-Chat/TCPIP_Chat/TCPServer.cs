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

        /*         public async Task CheckForNewClient()
                {
                    using TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                } */

        public void showMessage()
        {
            if (recivedMessage != null)
            {
                System.Console.WriteLine($"Time: {DateTimeOffset.Now} \t | \t Message:" + recivedMessage);
                recivedMessage = null;
            }
        }
    }
}