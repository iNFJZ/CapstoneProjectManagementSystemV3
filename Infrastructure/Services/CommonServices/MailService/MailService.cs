using Infrastructure.Entities.Common.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.MailService
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSettings;
        private readonly ILogger<MailService> _loggerMailService;
        private const string MailHN = "MailSettingsHN";
        private const string MailCT = "MailSettingsCT";
        private const string MailHCM = "MailSettingsHCM";
        private const string MailQN = "MailSettingsQN";
        private const string MailDN = "MailSettingsDN";
        public const string SessionKeyCampus = "campus";
        private readonly IConfiguration _configuration;

        public MailService(IOptions<MailSetting> mailSettings, ILogger<MailService> loggerMailService, IConfiguration configuration)
        {
            _configuration = configuration;
            string mailSettingValue = MailHN;
            try
            {
                IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
                var campus = _httpContextAccessor.HttpContext.Session.GetString(SessionKeyCampus);
                if (!string.IsNullOrEmpty(campus))
                {
                    switch (campus)
                    {
                        case "CT":
                            mailSettingValue = MailCT;
                            break;
                        case "HCM":
                            mailSettingValue = MailHCM;
                            break;
                        case "DN":
                            mailSettingValue = MailDN;
                            break;
                        case "QN":
                            mailSettingValue = MailQN;
                            break;
                        default: break;
                    }
                }
            }
            catch (Exception)
            {
                mailSettingValue = MailHN;
            }
            var mailSettingConfiguaration = _configuration.GetSection(mailSettingValue);
            _mailSettings = mailSettingConfiguaration.Get<MailSetting>();
            _loggerMailService = loggerMailService;
            //_loggerMailService.LogInformation("Create SendMailService");
        }

        public async Task SendEmailAsync(string email, string subject, string OTP)
        {
            await SendMail(new MailContent()
            {
                ToMail = email,
                Subject = subject,
                Body = GetMailBody(email, OTP),
            });
        }

        public async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.ToMail));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send mail Exception Type: {0}", ex.GetType());
                Console.WriteLine(" Message: {0}", ex.Message);

                //System.IO.Directory.CreateDirectory("mailssave");
                //var emailSaveFile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                //await email.WriteToAsync(emailSaveFile);

                //_loggerMailService.LogInformation("Send mail error, save here - " + emailSaveFile);
                //_loggerMailService.LogError(ex.Message);
            }

            smtp.Disconnect(true);
            _loggerMailService.LogInformation("Send mail to" + mailContent.ToMail);
        }
        public string GetMailBody(string personalEmail, string OTP)
        {

            return "Use this OTP to verify your email: " + OTP + ". Notice: This OTP will expire in 5 minutes";
        }
        public string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(10).ToString()
                + random.Next(10).ToString()
                + random.Next(10).ToString()
                + random.Next(10).ToString()
                + random.Next(10).ToString()
                + random.Next(10).ToString();
        }


        // send email change topic
        public async Task SendMailChangeRequest(string receiver, string cc, string subject, string body)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(receiver));
            string[] CCId = cc.Split(',');
            foreach (string CCEmail in CCId)
            {
                email.Cc.Add(MailboxAddress.Parse(CCEmail));
            }
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send mail Exception Type: {0}", ex.GetType());
                Console.WriteLine(" Message: {0}", ex.Message);
            }
            smtp.Disconnect(true);
        }

        //send email when after have notification 
        public async Task SendMailNotification(string receiver, string cc, string subject, string body)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            string[] receiverArr = receiver.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < receiverArr.Length; i++)
            {
                email.To.Add(MailboxAddress.Parse(receiverArr[i]));
            }
            if (cc != null && cc != "")
            {
                string[] CCEmail = cc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < CCEmail.Length; i++)
                {
                    email.Cc.Add(MailboxAddress.Parse(CCEmail[i]));
                }
            }

            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send mail Exception Type: {0}", ex.GetType());
                Console.WriteLine(" Message: {0}", ex.Message);
            }
            smtp.Disconnect(true);
        }
    }
}
