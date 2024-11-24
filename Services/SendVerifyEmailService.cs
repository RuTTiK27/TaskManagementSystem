using MimeKit;
using MailKit.Net.Smtp;

namespace TaskManagementSystem.Services
{
    public class SendVerifyEmailService
    {
        //TaskManagementSystem = uyrc dojb cjoo wjuv
        public async Task SendVerificationEmailAsync(string email,string verificationCode) 
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Task Management System", "xeroxshops2021@gmail.com"));
            message.To.Add(new MailboxAddress("",email));
            message.Subject = "Your Verification Code";

            message.Body = new TextPart("Plain")
            {
                Text = $"Your verification code is {verificationCode}"
            };

            using (var client = new SmtpClient()) 
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false); // Update with your SMTP details
                await client.AuthenticateAsync("xeroxshops2021@gmail.com", "uyrc dojb cjoo wjuv");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }     
        public string GenerateVerificationCode() 
        {
                Random random = new Random();
                return random.Next(100000,999999).ToString();
        }
    }
}
