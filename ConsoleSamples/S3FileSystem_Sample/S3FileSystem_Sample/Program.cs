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
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.S3;
using Amazon.S3.IO;

namespace S3FileSystem_Sample
{
    class Program
    {
        // Change the AWSProfileName to the profile you want to use in the App.config file.
        // See http://docs.aws.amazon.com/AWSSdkDocsNET/latest/DeveloperGuide/net-dg-config-creds.html for more details.
        // You must also sign up for an Amazon S3 account for this to work
        // See http://aws.amazon.com/s3/ for details on creating an Amazon S3 account
        // Change the bucketName field to a unique name that will be created and used for the sample.
        static string bucketName = "*** Enter a Unique Bucket Name Here ***";
        static IAmazonS3 client;

        static void Main(string[] args)
        {
            if (checkRequiredFields())
            {
                using (client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.USWest2))
                {
                    // Creates the bucket.
                    S3DirectoryInfo rootDirectory = new S3DirectoryInfo(client, bucketName);
                    rootDirectory.Create();

                    // Creates a file at the root of the bucket.
                    S3FileInfo readme = rootDirectory.GetFile("README.txt");
                    using (StreamWriter writer = new StreamWriter(readme.OpenWrite()))
                        writer.WriteLine("This is my readme file.");

                    DirectoryInfo localRoot = new DirectoryInfo(@"C:\");
                    DirectoryInfo localCode = localRoot.CreateSubdirectory("code");

                    // Create a directory called code and write a file to it.
                    S3DirectoryInfo codeDir = rootDirectory.CreateSubdirectory("code");
                    S3FileInfo codeFile = codeDir.GetFile("Program.cs");
                    using(StreamWriter writer = new StreamWriter(codeFile.OpenWrite()))
                    {
                        writer.WriteLine("namespace S3FileSystem_Sample");
                        writer.WriteLine("{");
                        writer.WriteLine("    class Program");
                        writer.WriteLine("    {");
                        writer.WriteLine("        static void Main(string[] args)");
                        writer.WriteLine("        {");
                        writer.WriteLine("            Console.WriteLine(\"Hello World\");");
                        writer.WriteLine("        }");
                        writer.WriteLine("    }");
                        writer.WriteLine("}");
                    }


                    // Create a directory called license and write a file to it.
                    S3DirectoryInfo licensesDir = rootDirectory.CreateSubdirectory("licenses");
                    S3FileInfo licenseFile = licensesDir.GetFile("license.txt");
                    using (StreamWriter writer = new StreamWriter(licenseFile.OpenWrite()))
                        writer.WriteLine("A license to code");


                    Console.WriteLine("Write Directory Structure");
                    Console.WriteLine("------------------------------------");
                    WriteDirectoryStructure(rootDirectory, 0);


                    Console.WriteLine("\n\n");
                    foreach (var file in codeDir.GetFiles())
                    {
                        Console.WriteLine("Content of {0}", file.Name);
                        Console.WriteLine("------------------------------------");
                        using (StreamReader reader = file.OpenText())
                        {
                            Console.WriteLine(reader.ReadToEnd());
                        }
                    }

                    // Deletes all the files and then the bucket.
                    rootDirectory.Delete(true);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void WriteDirectoryStructure(S3DirectoryInfo directory, int level)
        {
            StringBuilder indentation = new StringBuilder();
            for (int i = 0; i < level; i++)
                indentation.Append("\t");

            Console.WriteLine("{0}{1}", indentation, directory.Name);
            foreach (var file in directory.GetFiles())
                Console.WriteLine("\t{0}{1}", indentation, file.Name);

            foreach (var subDirectory in directory.GetDirectories())
            {
                WriteDirectoryStructure(subDirectory, level + 1);
            }
        }

        static bool checkRequiredFields()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
            {
                Console.WriteLine("AWSProfileName was not set in the App.config file.");
                return false;
            }
            if (string.IsNullOrEmpty(bucketName))
            {
                Console.WriteLine("The variable bucketName is not set.");
                return false;
            }

            return true;
        }
    }
}
