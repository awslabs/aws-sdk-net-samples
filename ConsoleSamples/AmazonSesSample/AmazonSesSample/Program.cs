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

using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace AmazonSesSample
{
    class Program
    {
        // Change the AWSProfileName to the profile you want to use in the App.config file.
        // See http://docs.aws.amazon.com/AWSSdkDocsNET/latest/DeveloperGuide/net-dg-config-creds.html for more details.
        // You must also sign up for an Amazon SES account for this to work
        // See http://aws.amazon.com/ses/ for details on creating an Amazon SES account
        // This sample send a mail using SES.
        // Change the senderAddress and receiverAddress fields to values that match your senderAddress and receiverAddress
        
        // Set the sender's email address here.
        static string senderAddress=null;

        // Set the receiver's email address here.
        static string receiverAddress=null;
        
        static void Main(string[] args)
        {
            if (CheckRequiredFields())
            {
                using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
                {
                    var sendRequest = new SendEmailRequest
                                        {
                                            Source = senderAddress,
                                            Destination = new Destination { ToAddresses = new List<string> { receiverAddress } },
                                            Message = new Message
                                            {
                                                Subject = new Content("Sample Mail using SES"),
                                                Body = new Body { Text = new Content("Sample message content.") }
                                            }
                                        };
                    try
                    {
                        Console.WriteLine("Sending email using AWS SES...");
                        var response = client.SendEmail(sendRequest);
                        Console.WriteLine("The email was sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The email was not sent.");
                        Console.WriteLine("Error message: " + ex.Message);

                    }
                }
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        static bool CheckRequiredFields()
        {
            var appConfig = ConfigurationManager.AppSettings;

            if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
            {
                Console.WriteLine("AWSProfileName was not set in the App.config file.");
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
