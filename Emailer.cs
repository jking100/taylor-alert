//using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace TaylorEmailerFunction
{
    public class Notification
    {
        public string? Msg { get; set; }
    }
    public class Emailer
    {

        private readonly ILogger<Emailer> _logger;

        public Emailer(ILogger<Emailer> logger)
        {
            _logger = logger;
        }

        [Function("Emailer")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            //_logger.LogInformation("C# HTTP trigger function processed a request.");
            if (req.ContentType != "application/json")
            {
                return new BadRequestObjectResult("Not allowed");
            }

            Notification notification = new Notification();

            try
            {
                using var reader = new StreamReader(req.Body);
                string bodyText = await reader.ReadToEndAsync();

                if (!String.IsNullOrWhiteSpace(bodyText))
                {
                    notification = JsonSerializer.Deserialize<Notification>(bodyText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    notification.Msg = "";
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("Error Reading Request");
            }

            string SendingEmail = Environment.GetEnvironmentVariable("SendingEmail");
            string ReceivingEmail = Environment.GetEnvironmentVariable("ReceivingEmail");

            var EmailMsg = new MimeMessage();
            EmailMsg.From.Add(new MailboxAddress("Auto Notify", SendingEmail));
            EmailMsg.To.Add(new MailboxAddress("Taylor", ReceivingEmail));

            EmailMsg.Subject = "Notification - Script Execution Finished";
            string bodyMsg = $"<h1>Script is done</h1><p>choke or no choke? you will know soon</p><hr><h3>{notification.Msg ?? ""}</h3>";
            EmailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = bodyMsg };

            try
            {
                string GoogleSMTPStr = Environment.GetEnvironmentVariable("GoogleSMTPStr");

                var client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(SendingEmail, GoogleSMTPStr);
                client.Send(EmailMsg);
                client.Disconnect(true);
                client.Dispose();

                return new OkObjectResult(bodyMsg);

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult($"Error trying to send \"{bodyMsg}\"\nError Message:\n{e.Message}");
            }
        }
    }
}
