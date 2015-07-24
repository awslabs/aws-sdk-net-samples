using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


namespace ContactManager
{
    [DynamoDBTable("ContactList")]
    public class Contact
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBRangeKey]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string FirstName { get; set; }
        
        [DynamoDBProperty]
        public string LastName { get; set; }
            
        [DynamoDBProperty]
        public int HomePhoneNumber { get; set; }
        
        [DynamoDBProperty]
        public int WorkPhoneNumber { get; set; }
        
        [DynamoDBProperty]
        public int MobileNumber { get; set; }
        
        [DynamoDBProperty]
        public string EmailAddress { get; set; }
    }
}
