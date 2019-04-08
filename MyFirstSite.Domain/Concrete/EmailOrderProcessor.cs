using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Entities;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MyFirstSite.Domain.Concrete{

    public class EmailSetting{

        public string MailToAddress = "cerakuzz@mail.ru";
        public string MailFromAddress = "nesorubov@gmail.com";
        public bool UseSsl = true;
        public string Username = "cerakuzz@mail.ru";
        public string Password = "Yfafyfbk1979";
        public string ServerName = "smtp.mail.ru";
        public int ServerPort = 465;
        public bool WriteAsFile = false;
        public string FileLocation = @"G:\C#\SITE_INTERNET_SHOP";
    }

    public class EmailOrderProcessor: IOrderProcessor {

        private EmailSetting emailSetting;

        public EmailOrderProcessor(EmailSetting settings) {
            emailSetting = settings;
        }

        public void ProcessorOrder(Cart cart, ShippingDetail shippingDetail) {

            using(var smtpClient=new SmtpClient(emailSetting.ServerName, emailSetting.ServerPort)) {
                smtpClient.EnableSsl = emailSetting.UseSsl;
                //smtpClient.Host = emailSetting.ServerName;
                //smtpClient.Port = emailSetting.ServerPort;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(emailSetting.Username, emailSetting.Password);
                if (emailSetting.WriteAsFile) {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSetting.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items:");

                foreach(var line in cart.Lines) {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0}x{1} subtotal:{2:c}", line.Product.Name, line.Quantity, subtotal);
                }

                body.AppendFormat("Total order value:{0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingDetail.Name)
                    .AppendLine(shippingDetail.LastName)
                    .AppendLine(shippingDetail.Email)
                    .AppendLine(shippingDetail.Phone)
                    .AppendLine(shippingDetail.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap:{0}", shippingDetail.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(emailSetting.MailFromAddress, emailSetting.MailToAddress, "New order submitted!", body.ToString());

                if (emailSetting.WriteAsFile) {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
