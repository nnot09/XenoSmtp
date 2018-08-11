using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SmtpHost host = new SmtpHost()
            {
                Host = "smtp.somesite.com",
                Port = 25
            };

            SmtpLogin credentials = new SmtpLogin()
            {
                Username = "info@somesite.com",
                Password = "bigadmen123"
            };

            using (SmtpSocket auth = new SmtpSocket(host))
            {
                auth.Connect();
                if (auth.Authenticate(credentials))
                    Console.WriteLine("Logged in");
                else
                    Console.WriteLine("Failed");
            }

            Console.WriteLine(" == Program Executed == ");

            Console.ReadLine();
        }
    }
}
