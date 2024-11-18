

using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using TCP_Connector;

class Programm
{
    static async Task Main(String[] args)
    {
        string? Input;
        bool isValid = false;
        string serverIp = "127.0.0.1";
        int port = 13000;

        TcpLink serverlink = new TcpLink(serverIp, port);
        TcpClient client = new TcpClient(serverIp, port);
        bool isConnected = client.Connected;
        // Erstelle einen neuen TcpClient und verbinde ihn mit dem Server

        System.Console.WriteLine("Verbinde mit: " + serverIp);

        /*         await serverlink.reciveMSG(client);
                await serverlink.reciveMSG(client);
                await serverlink.reciveMSG(client); */

        System.Console.WriteLine("Bitte geben sie ihren ersten command ein:");

        while (isConnected)
        {
            while (!isValid)
            {
                Input = Console.ReadLine();
                if (Input != null)
                {
                    serverlink.sendMSG(Input, client);
                    isValid = true;
                }
                else if (Input == "x")
                {
                    client.Close();
                    isConnected = client.Connected;
                }

            }
            //await serverlink.reciveMSG(client);
            isValid = false;
        }
    }
}