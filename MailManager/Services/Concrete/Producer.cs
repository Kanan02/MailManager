using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace MailManager.Services.Concrete
{
    public class Producer
    {


        public static void Send()
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 20; i++)
            {
                int temp = i;
                Thread task = new Thread(() => SendMail( temp + 1));
                threads.Add(task);
            }

            foreach (Thread item in threads)
            {
                item.Start();
                Thread.Sleep(500);
            }

        }

        public static void SendMail(int n)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                mail.From = new MailAddress("atltaskproducer@gmail.com");
                mail.To.Add("atltaskconsumer@gmail.com");

                mail.Subject = $"Mail From Producer, Message {n}";
                mail.Body = $"This is messge number {n}.";

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("atltaskproducer@gmail.com", "atl12345");

                smtp.EnableSsl = true;

                smtp.Send(mail);

                Console.WriteLine($"Mail {n} sent at: " + DateTime.Now);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
