using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using System.Device.Gpio;
using Twilio.Rest.Api.V2010.Account;

namespace FYP
{
    public class Program
    {
        public static void Main(string[] args)
        {

            /**const string accountSid = "ACdcebe7ac0e81e87e0390b9eb5eaff0e9";
            const string authToken = "ea5ee0912ce6292cc67ea78bd28cdf4c";
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Join Earth's mightiest heroes. Like Kevin Bacon.",
                from: new Twilio.Types.PhoneNumber("+"),
                to: new Twilio.Types.PhoneNumber("+6584559484")
            );

            Console.WriteLine(message.Sid);
            TwilioClient.Init(accountSid, authToken);**/
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    }
}