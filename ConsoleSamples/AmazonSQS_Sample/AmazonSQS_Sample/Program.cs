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
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AmazonSQS_Sample
{
    class Program
    {
        public static void Main(string[] args)
        {
            IAmazonSQS sqs = new AmazonSQSClient(RegionEndpoint.USWest2);

            try
            {
                Console.WriteLine("===========================================");
                Console.WriteLine("Getting Started with Amazon SQS");
                Console.WriteLine("===========================================\n");                
                
                //Creating a queue
                Console.WriteLine("Create a queue called MyQueue.\n");
                CreateQueueRequest sqsRequest = new CreateQueueRequest();
                sqsRequest.QueueName = "MyQueue";
                CreateQueueResponse createQueueResponse = sqs.CreateQueue(sqsRequest);
                String myQueueUrl;
                myQueueUrl = createQueueResponse.QueueUrl;

                //Confirming the queue exists
                ListQueuesRequest listQueuesRequest = new ListQueuesRequest();
                ListQueuesResponse listQueuesResponse = sqs.ListQueues(listQueuesRequest);

                Console.WriteLine("Printing list of Amazon SQS queues.\n");
                foreach (String queueUrl in listQueuesResponse.QueueUrls)
                {
                    Console.WriteLine("  QueueUrl: {0}", queueUrl);
                }
                Console.WriteLine();

                //Sending a message
                Console.WriteLine("Sending a message to MyQueue.\n");
                SendMessageRequest sendMessageRequest = new SendMessageRequest();
                sendMessageRequest.QueueUrl = myQueueUrl; //URL from initial queue creation
                sendMessageRequest.MessageBody = "This is my message text.";
                sqs.SendMessage(sendMessageRequest);
                
                //Receiving a message
                ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
                receiveMessageRequest.QueueUrl = myQueueUrl;
                ReceiveMessageResponse receiveMessageResponse = sqs.ReceiveMessage(receiveMessageRequest);

                Console.WriteLine("Printing received message.\n");
                foreach (Message message in receiveMessageResponse.Messages)
                {
                    Console.WriteLine("  Message");
                    Console.WriteLine("    MessageId: {0}", message.MessageId);
                    Console.WriteLine("    ReceiptHandle: {0}", message.ReceiptHandle);
                    Console.WriteLine("    MD5OfBody: {0}", message.MD5OfBody);
                    Console.WriteLine("    Body: {0}", message.Body);

                    foreach (KeyValuePair<string, string> entry in message.Attributes)
                    {
                        Console.WriteLine("  Attribute");
                        Console.WriteLine("    Name: {0}", entry.Key);
                        Console.WriteLine("    Value: {0}", entry.Value);
                    }
                }
                String messageRecieptHandle = receiveMessageResponse.Messages[0].ReceiptHandle;

                //Deleting a message
                Console.WriteLine("Deleting the message.\n");
                DeleteMessageRequest deleteRequest = new DeleteMessageRequest();
                deleteRequest.QueueUrl = myQueueUrl;
                deleteRequest.ReceiptHandle = messageRecieptHandle;
                sqs.DeleteMessage(deleteRequest);

            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.Read();
        }
    }
}