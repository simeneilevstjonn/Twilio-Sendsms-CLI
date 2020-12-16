using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace sendsms
{
    class Program
    {

        // Define SMS info variables
        static string To { get; set; }
        static string From { get; set; }
        static string Message { get; set; }

        static void Main(string[] args)
        {
            // Check how many args have been given
            switch (args.Length)
            {
                case 3:
                    To = args[0];
                    From = args[1];
                    Message = args[2];
                    break;
                case 0:
                    Console.Write("To: ");
                    To = Console.ReadLine();
                    Console.Write("From: ");
                    From = Console.ReadLine();
                    Console.Write("Message: ");
                    Message = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Error");
                    // TODO helpful error message
                    Environment.Exit(0);
                    break;
            }

            // Grab information from config file
            string ConfigFile = "";
            bool IsWindows = false;
            
            try
            {
                // Get the file for the current OS
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    IsWindows = false;
                    ConfigFile = File.ReadAllText("/etc/sendsms/config.json");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    IsWindows = true;
                    ConfigFile = File.ReadAllText("C:\\Program Files\\sendsms\\config.json");
                }
                else
                {
                    Console.WriteLine("Unsupported operating system");
                    Environment.Exit(0);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not find config file at " + IsWindows switch
                {
                    false => "/etc/sendsms/config.json",
                    true => "C:\\Program Files\\sendsms\\config.json"
                });

                /*
                  Config file format:
                    {
                        "AccountSID": "INSERT Account SID",
                        "AuthToken": "INSERT AUTH TOKEN",
                        "ServiceSID": "INSERT MESSAGING SERVICE SID"
                    }
                 */

                Environment.Exit(0);
            }

            JObject Config = JObject.Parse(ConfigFile);

            TwilioClient.Init((string)Config["AccountSID"], (string)Config["AuthToken"]);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(To))
            {
                From = new PhoneNumber(From),
                MessagingServiceSid = (string)Config["ServiceSID"],
                Body = Message
            };

            var message = MessageResource.Create(messageOptions);
            Console.WriteLine("\nStatus: " + message.Status);
            if (message.ErrorMessage != null)
            {
                Console.WriteLine(message.ErrorMessage);
                Environment.Exit((int)message.ErrorCode);
            }
        }
    }
}
