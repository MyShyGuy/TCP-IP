using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using TCP_Connector;

#pragma warning disable CS1998

class Programm
{
    static async Task Main(String[] args)
    {
        User clientUser = new User();
        User.DisplayWelcomeMessage();
        clientUser.SetName(User.PromptForInput("Username: "));
        clientUser.setPassword(User.PromptForHiddenInput("Password: "));
        clientUser.ShowUser();

        try
        {

            string? Input;
            bool isValid = false;
            string serverIp = "127.0.0.1";
            int port = 13000;

            // Erstelle einen neuen TcpClient und verbinde ihn mit dem Server
            TcpLink serverlink = new TcpLink(serverIp, port);
            TcpClient client = new TcpClient(serverIp, port);
            NetworkStream stream = client.GetStream();
            bool isConnected = client.Connected;

            Task readTask = Task.Run(() => TcpLink.MsgReadAsync(stream));


            System.Console.WriteLine("Verbinde mit: " + serverIp);
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
                    if (Input == "x")
                    {
                        client.Close();
                        isConnected = client.Connected;
                    }

                }
                isValid = false;
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error: {ex.Message}");
            Console.ReadKey();
        }
    }
}