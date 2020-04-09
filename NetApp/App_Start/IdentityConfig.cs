using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using NetApp.Models;
using NetApp;

using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace NetApp
{
   

    public class EmailService  //: IIdentityMessageService
    {

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "info@melt.events"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.AuthenticateAsync("info@melt.events", "c123456");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendMessage()
        {
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync("nick-su26@mail.ru", "Тема письма", "Тест письма: тест!");
           
        }

        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            // return Task.FromResult(0);

           

            return Task.Factory.StartNew(() =>
            {
                SendMessage();
                // sendMail(message);
            });
        }

        void sendMail(IdentityMessage message)
        {
            #region formatter
            string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
            //string html = "Пожалйста, подтвердите ваш email, кликнув на ссылку ниже:" + message.Body;

           // string html += HttpUtility.HtmlEncode(@"Или скопируйте ссылку в адресную строку в браузере и перейдите по ней:" + message.Body);
            #endregion

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            // msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // SmtpClient smtpClient = new SmtpClient("smtp.yandex.ru", Convert.ToInt32(465));
            // smtpClient.EnableSsl = true;

            //// smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //// smtpClient.UseDefaultCredentials = false;

            // System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
            // smtpClient.Credentials = credentials;

            // smtpClient.Send(msg);

            //var msg = new MailMessage(from, to, subj, body);
            //var smtpClient = new SmtpClient("smtp.yandex.ru", 25);
            //smtpClient.EnableSsl = true;
            //smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential("info@melt.events", "с123456");

            //smtpClient.Send(msg);

            //SmtpClient client = new SmtpClient();
            //client.Host = "smtp.yandex.ru";
            //client.Port = 587;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.EnableSsl = true;
            //client.Credentials = new NetworkCredential("info", "с123456");

            //client.Send(msg);
       
        }

    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
           // manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
