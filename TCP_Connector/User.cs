using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Connector
{
    public class User
    {
        public string name { get; private set; }
        private string? password { get; set; }

        public User(string Name, string Password)
        {
            name = Name;
            password = HashPwToHex(Password);
        }

        public User()
        {
            name = "Anonymus";
        }

        public void SetName(string Name)
        {
            name = Name;
        }

        public void setPassword(string Password)
        {
            password = HashPwToHex(Password);
        }


        private string HashPwToHex(string PW)
        {
            byte[] pwBytes = Encoding.UTF8.GetBytes(PW);
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(pwBytes);
                string hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashHex;
            }
        }

        public void ShowUser()
        {
            System.Console.WriteLine($"Username: {name}");
            System.Console.WriteLine($"PW in HexHash: {password}");
        }

        public static void DisplayWelcomeMessage()
        {
            Console.WriteLine(
            """
              _----------_,
            ,"__         _-:, 
           /    ""--_--""...:\
          /         |.........\
         /          |..........\
        /,         _'_........./:
        ! -,    _-"   "-_... ,;;:
        \   -_-"         "-_/;;;;
         \   \             /;;;;'
          \   \           /;;;;
           '.  \         /;;;'
             "-_\_______/;;'
        """
            );
            Console.WriteLine("Welcome to the Vdit Chat\n");
            Console.WriteLine("Please log in.\n");
        }

        public static string PromptForInput(string promptMessage)
        {
            string? input = null;
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(promptMessage);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                }
            }
            return input;
        }

        public static string PromptForHiddenInput(string promptMessage)
        {
            Console.Write(promptMessage);
            string input = string.Empty;
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(intercept: true); // Eingabe wird nicht in der Konsole angezeigt

                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    // Bei Backspace das letzte Zeichen entfernen
                    input = input[..^1];
                    Console.Write("\b \b"); // Überschreibt das letzte Zeichen in der Konsole
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    // Gültiges Zeichen zur Eingabe hinzufügen
                    input += key.KeyChar;
                    Console.Write("*"); // Sternchen anzeigen
                }

            } while (key.Key != ConsoleKey.Enter); // Eingabe endet mit Enter

            Console.WriteLine(); // Gehe zur nächsten Zeile
            return input;
        }

    }
}