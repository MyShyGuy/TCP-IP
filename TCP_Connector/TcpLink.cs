using System.Reflection;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace TCP_Connector;

public class TcpLink
{
    public string serverIp;
    public int port;

    public TcpLink(string IP, int Port)
    {
        this.serverIp = IP;
        this.port = Port;

    }

    public async void sendMSG(string msg, TcpClient client)
    {
        try
        {
            // Hole den Netzwerkstream des Clients
            NetworkStream stream = client.GetStream();

            // Optional: Daten an den Server senden
            string message = msg;
            byte[] dataToSend = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
            //Console.WriteLine("Nachricht gesendet: " + message);
        }
        catch (Exception)
        {
            throw;
        }
    }

    // Asynchrone Methode zum Lesen und Schreiben von Daten
    public static async Task MsgReadAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                // Daten vom Server asynchron lesen
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    System.Console.WriteLine("Verbindung zum server wurde geschlossen.");
                    break;
                }

                // Konvertiere die gelesenen Bytes in einen String und gebe sie aus
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine(response);
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error: {ex.Message}");
        }
    }
}