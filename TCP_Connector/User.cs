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
    }
}