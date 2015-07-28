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
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

namespace AmazonDynamoDB_Sample
{
    public partial class Program
    {
        public static IAmazonDynamoDB client;

        public static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Setting up DynamoDB client");
            client = new AmazonDynamoDBClient(RegionEndpoint.USWest2);

            Console.WriteLine();
            Console.WriteLine("Creating sample tables");
            CreateSampleTables();

            Console.WriteLine();
            Console.WriteLine("Running DataModel sample");
            RunDataModelSample();

            Console.WriteLine();
            Console.WriteLine("Running DataModel sample");
            RunDocumentModelSample();

            Console.WriteLine();
            Console.WriteLine("Removing sample tables");
            DeleteSampleTables();

            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.Read();
        }
    }
}