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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.Glacier;
using Amazon.Glacier.Model;
using Amazon.Glacier.Transfer;
using System.Collections.Specialized;
using System.Configuration;
using Amazon.Runtime;

namespace AmazonGlacierSample
{
    class Program
    {
        // Change the AWSProfileName to the profile you want to use in the App.config file.
        // See http://aws.amazon.com/credentials  for more details.
        // You must also sign up for an Amazon Glacier account for this to work
        // See http://aws.amazon.com/glacier/ for details on creating an Amazon Glacier account
        // Change the vaultName and fileName fields to values that match your vaultName and fileName

        static ArchiveTransferManager manager;
        static string archiveId;

        //Set the vault name you want to use here.
        static string vaultName=null;

        // Set the file path for the file you want to upload here.
        static string filePath=null;

        // Set the file path for the archive to be saved after download.
        static string downloadFilePath=null;
        

        static void Main(string[] args)
        {
            if (CheckRequiredFields())
            {                
                using (manager = new ArchiveTransferManager(RegionEndpoint.USWest2))
                {
                    try
                    {
                        // Creates a new Vault
                        Console.WriteLine("Create Vault");
                        manager.CreateVault(vaultName);

                        // Uploads the specified file to Glacier.
                        Console.WriteLine("Upload a Archive");
                        var uploadResult = manager.Upload(vaultName, "Archive Description", filePath);
                        archiveId = uploadResult.ArchiveId;
                        Console.WriteLine("Upload successful. Archive Id : {0}  Checksum : {1}",
                            uploadResult.ArchiveId, uploadResult.Checksum);

                        // Downloads the file from Glacier 
                        // This operation can take a long time to complete. 
                        // The ArchiveTransferManager.Download() method creates an Amazon SNS topic, 
                        // and an Amazon SQS queue that is subscribed to that topic. 
                        // It then initiates the archive retrieval job and polls the queue for the 
                        // archive to be available. This polling takes about 4 hours.
                        // Once the archive is available, download will begin.
                        Console.WriteLine("Download the Archive");
                        var options = new DownloadOptions();
                        options.StreamTransferProgress += OnProgress;
                        manager.Download(vaultName, archiveId, downloadFilePath, options);

                        Console.WriteLine("Delete the Archive");
                        manager.DeleteArchive(vaultName, archiveId);

                    }
                    catch (AmazonGlacierException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (AmazonServiceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        static int currentPercentage = -1;
        
        static void OnProgress(object sender, StreamTransferProgressArgs args)
        {
            if (args.PercentDone != currentPercentage)
            {
                currentPercentage = args.PercentDone;
                Console.WriteLine("Downloaded {0}%", args.PercentDone);
            }
        }

        static bool CheckRequiredFields()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
            {
                Console.WriteLine("AWSProfileName was not set in the App.config file.");
                return false;
            }
            if (string.IsNullOrEmpty(vaultName))
            {
                Console.WriteLine("The variable vaultName is not set.");
                return false;
            }
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("The variable filePath is not set.");
                return false;
            }
            if (string.IsNullOrEmpty(downloadFilePath))
            {
                Console.WriteLine("The variable downloadFilePath is not set.");
                return false;
            }

            return true;
        }
    }
}
