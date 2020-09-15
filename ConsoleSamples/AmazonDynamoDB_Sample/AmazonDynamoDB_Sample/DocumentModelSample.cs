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

using Amazon.DynamoDBv2.DocumentModel;

namespace AmazonDynamoDB_Sample
{
    public partial class Program
    {
        public static void RunDocumentModelSample()
        {
            Console.WriteLine("Loading Businesses table");
            Table table = Table.LoadTable(client, "Businesses");

            Console.WriteLine("Creating and saving first item");
            Document chainStore2 = new Document();
            chainStore2["Name"] = "Big Sales Inc";
            chainStore2["Id"] = 2;
            chainStore2["Owner"] = "Big Sales Corp";
            chainStore2["Managers"] = new List<string> { "Samantha Jones", "Richard Frost" };
            chainStore2["FoundedDate"] = new DateTime(1980, 7, 4);
            chainStore2["Address"] = "123 Main Street, New York, New York";
            chainStore2["Employees"] = 46;
            chainStore2["State"] = "NY";
            table.PutItem(chainStore2);

            Console.WriteLine("Creating and saving second item");
            Document chainStore13 = new Document();
            chainStore13["Name"] = "Big Sales Inc";
            chainStore13["Id"] = 13;
            chainStore13["Owner"] = "Big Sales Corp";
            chainStore13["Managers"] = new List<string> { "Anil Garg", "Alex Short" };
            chainStore13["FoundedDate"] = new DateTime(1999, 3, 15);
            chainStore13["Address"] = "1999 West Ave, Chicago, Illinois";
            chainStore13["Employees"] = 54;
            chainStore13["State"] = "IL";
            table.PutItem(chainStore13);

            Console.WriteLine("Creating and saving third item");
            Document tinyDiner = new Document();
            tinyDiner["Name"] = "Tiny Map-themed Diner";
            tinyDiner["Id"] = 0;
            tinyDiner["Owner"] = "John Doe";
            tinyDiner["FoundedDate"] = new DateTime(1974, 12, 10);
            tinyDiner["Address"] = "800 Lincoln Ave, Seattle, Washington";
            tinyDiner["State"] = "WA";
            table.PutItem(tinyDiner);


            Console.WriteLine("Creating and saving fourth item");
            Document internetStore = new Document();
            internetStore["Name"] = "Best Online Store Ever";
            internetStore["Id"] = 0;
            internetStore["Owner"] = "Jane Doe";
            internetStore["FoundedDate"] = new DateTime(1994, 2, 19);
            internetStore["Employees"] = 5;
            internetStore["Url"] = "http://www.best-online-store-ever.fake";
            internetStore["Phone"] = "425-555-1234";
            table.PutItem(internetStore);

			Search scan;
			ScanFilter scanFilter;
			Console.WriteLine();
			Console.WriteLine("Scanning for items (no filter) to get count");
			scanFilter = new ScanFilter();
			scan = table.Scan(scanFilter);
			Console.WriteLine("Number of items returned (should be 4): " + scan.Count);
			Console.WriteLine();

			Console.WriteLine("Loading item");
            Document doc1 = table.GetItem("Big Sales Inc", 2);
            Console.WriteLine("Attribute counts match (should be true): " +
                (chainStore2.GetAttributeNames().Count == doc1.GetAttributeNames().Count));

            Console.WriteLine("Loading item...");
            Document doc2 = table.GetItem("Best Online Store Ever", 0);
            Console.WriteLine("Attribute counts match (should be true): " +
                (chainStore2.GetAttributeNames().Count == doc1.GetAttributeNames().Count));
            Console.WriteLine("Change item: remove one attribute, add one, modify one attribute");
            doc2["Phone"] = null;
            doc2["Twitter"] = "best-online-store-ever";
            doc2["Employees"] = 4;
            Console.WriteLine("Updating item");
            table.UpdateItem(doc2);

            Console.WriteLine("Reloading item");
            doc2 = table.GetItem("Best Online Store Ever", 0);
            Console.WriteLine("Phone attribute present (should be false): " + doc2.Contains("Phone"));
            Console.WriteLine("Twitter attribute present (should be true): " + doc2.Contains("Twitter"));
            Console.WriteLine("Employees attribute equals 4: " + (object.Equals(doc2["Employees"].AsPrimitive().Value, "4")));

            Console.WriteLine("Loading nonexistent item");
            Document doc3 = table.GetItem("Big Sales Inc", 3);
            Console.WriteLine("Returned document == null (should be true): " + (doc3 == null));


            Search query;
            Console.WriteLine();
            Console.WriteLine("Querying for items (Equals)");
            query = table.Query("Big Sales Inc", new QueryFilter("Id", QueryOperator.Equal, 2));
            List<Document> queryItems1 = query.GetRemaining();
            Console.WriteLine("Number of items returned (should be 1): " + queryItems1.Count);

            Console.WriteLine();
            Console.WriteLine("Querying for items (Between)");
            QueryFilter filter = new QueryFilter();
            filter.AddCondition("Name", QueryOperator.Equal, "Big Sales Inc");
            filter.AddCondition("Id", QueryOperator.Between, 0, 15);
            QueryOperationConfig queryConfig = new QueryOperationConfig
            {
                Filter = filter,
                Limit = 1
            };
            query = table.Query(queryConfig);
            int totalItems = 0;
            while (!query.IsDone)
            {
                Console.WriteLine("Retrieving next set (page) of items");
				List<Document> querySet = query.GetNextSet();
				Console.WriteLine("More items (Should be true until last set): " + !query.IsDone);
				Console.WriteLine("Number of items returned in set (should be 1 (will be 0 for final set - no more to get): " + querySet.Count);

                foreach (Document doc in querySet)
                {
                    Console.WriteLine("Retrieving individual properties");
                    Primitive name = doc["Name"].AsPrimitive();
                    Primitive id = doc["Id"].AsPrimitive();
                    PrimitiveList managers = doc["Managers"].AsPrimitiveList();
                    Console.WriteLine("Name = {0}, Id = {1}, # of managers = {2}", name.Value, id.Value, managers.Entries.Count);
                    totalItems++;
                }
            }
            Console.WriteLine("Total items found (should be 2): " + totalItems);


            Console.WriteLine();
            Console.WriteLine("Scanning for items (GreaterThan)");
            scanFilter = new ScanFilter();
            scanFilter.AddCondition("Employees", ScanOperator.GreaterThan, 50);
            scan = table.Scan(scanFilter);
            List<Document> scanItems1 = scan.GetRemaining();
            Console.WriteLine("Number of items returned (should be 1): " + scanItems1.Count);

            Console.WriteLine();
            Console.WriteLine("Scanning for items (GreaterThan and LessThan)");
            scanFilter = new ScanFilter();
            scanFilter.AddCondition("Employees", ScanOperator.GreaterThan, 2);
            scanFilter.AddCondition("FoundedDate", ScanOperator.LessThan, new DateTime(1993, 1, 1));
            scan = table.Scan(scanFilter);
            List<Document> scanItems2 = scan.GetRemaining();
            Console.WriteLine("Number of items returned (should be 1): " + scanItems2.Count);


            Console.WriteLine();
            Console.WriteLine("Retrieving an item");
            Document existingDoc = table.GetItem("Big Sales Inc", 13);
            Console.WriteLine("Returned document == null (should be false): " + (existingDoc == null));
            Console.WriteLine("Deleting item");
            table.DeleteItem("Big Sales Inc", 13);
            Console.WriteLine("Retrieving same item");
            existingDoc = table.GetItem("Big Sales Inc", 13);
            Console.WriteLine("Returned document == null (should be true): " + (existingDoc == null));


            Console.WriteLine();
            Console.WriteLine("Scanning for items (no filter) and deleting all");
            scanFilter = new ScanFilter();
            scan = table.Scan(scanFilter);
            List<Document> scanItems3 = scan.GetRemaining();
            Console.WriteLine("Number of items returned (should be 3): " + scanItems3.Count);
            for (int i = 0; i < scanItems3.Count; i++)
            {
                Document item = scanItems3[i];
                Console.WriteLine("Deleting item {0} of {1}", i + 1, scanItems3.Count);
                table.DeleteItem(item);
            }

            Console.WriteLine("Scanning table again");
            scan = table.Scan(scanFilter);
            scanItems3 = scan.GetRemaining();
            Console.WriteLine("Number of items returned (should be 0): " + scanItems3.Count);
        }
    }
}
