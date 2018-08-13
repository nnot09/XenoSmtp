using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SmtpTest
{
    public class SmtpSocket : IDisposable
    {
        private SmtpHost smtp;
        private Socket socket;
        private string lastResponse;

        public SmtpSocket(SmtpHost smtp)
        {
            this.smtp = smtp;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            socket.Connect(smtp.Host, smtp.Port);
            lastResponse = Receive();
        }

        public bool Authenticate(SmtpLogin login)
        {
            bool fAuthenticated = false;

            Send("EHLO " + System.Net.Dns.GetHostName(), true);
            Send("EHLO " + System.Net.Dns.GetHostName(), true);
         
            if ( IsResponseCode ("250") )
            {
                Send("AUTH LOGIN", true);

                if ( IsResponseCode ("334") )
                {
                    string username = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(login.Username));
                    string password = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(login.Password));

                    Send(username, true);
                    Send(password, true);

                    if ( IsResponseCode ("235") )
                    {
                        // TODO send a test mail through these credentials to verify it's correctness. This can be a false-positive aswell.
                        fAuthenticated = true;
                    }
                }

                Send("close", true);
            }

            return fAuthenticated;
        }

        private void Send(string data, bool eol)
        {
            string formed = data + ((eol) ? "\r\n" : string.Empty);

            socket.Send(System.Text.Encoding.Unicode.GetBytes(formed));

            lastResponse = Receive();
        }

        private bool IsResponseCode(string expected)
        {
            // FIXME: improve this
            return lastResponse.Contains(expected);
        }

        private string Receive()
        {
            StringBuilder response = new StringBuilder();

            int bytes;

            byte[] buffer = new byte[1024];

            while ( ( bytes = socket.Receive(buffer, 0, 1024, SocketFlags.None) ) > 0  )
            {
                string translated = System.Text.Encoding.Unicode.GetString(buffer, 0, bytes);
                response.Append(response);
            }

            return response.ToString();
        }

        public void Dispose()
        {
            if (socket != null)
            {
                socket.Close();
                socket.Dispose();
            }
        }
    }
}
