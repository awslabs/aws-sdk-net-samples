/*******************************************************************************
* Copyright 2009-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
* 
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/

using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;


namespace AmazonSesSmtpSample
{
    class Program
    {
        // You must sign up for an Amazon SES account for this sample to work
        // See http://aws.amazon.com/ses/ for details on creating an Amazon SES account.
        // You also need AWS SES SMTP credentials to send emails using SMTP.
        // The AWS SES SMTP credentials are different from AWS access key ID and secret access key.
        // You can create the SMTP credentials from https://console.aws.amazon.com/ses/home#smtp-settings:/
        // This sample sends an email using SES with SMTP protocol.
        // Change the senderAddress and receiverAddress fields to appropriate values.
                
        // Set the sender's email address here.
        static string senderAddress = null;

        // Set the receiver's email address here.
        static string receiverAddress = null;
        
        // Set the SMTP user name in App.config 
        static string smtpUserName = null;

        // Set the SMTP password in App.config 
        static string smtpPassword = null;

        static string host = "email-smtp.us-east-1.amazonaws.com";

        static int port = 587;

        static void Main(string[] args)
        {
            if (CheckRequiredFields())
            {
                var smtpClient = new SmtpClient(host, port);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);

                var message = new MailMessage(
                                from: senderAddress,
                                to: receiverAddress,
                                subject: "Sample email using SMTP Interface",
                                body: "Sample email.");

                try
                {
                    Console.WriteLine("Sending email using SMTP interface...");
                    smtpClient.Send(message);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey(); 
        }

        static bool CheckRequiredFields()
        {
            var appConfig = ConfigurationManager.AppSettings;

            smtpUserName = appConfig["AwsSesSmtpUserName"];
            if (string.IsNullOrEmpty(smtpUserName))
            {
                Console.WriteLine("AwsSesSmtpUserName is not set in the App.config file.");
                return false;
            }

            smtpPassword = appConfig["AwsSesSmtpPassword"];
            if (string.IsNullOrEmpty(smtpPassword))
            {
                Console.WriteLine("AwsSesSmtpPassword is not set in the App.config file.");
                return false;
            }
            if (string.IsNullOrEmpty(senderAddress))
            {
                Console.WriteLine("The variable senderAddress is not set.");
                return false;
            }
            if (string.IsNullOrEmpty(receiverAddress))
            {
                Console.WriteLine("The variable receiverAddress is not set.");
                return false;
            }
 
            return true;
        }
    }
}
