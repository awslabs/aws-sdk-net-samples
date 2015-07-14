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

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace AmazonDynamoDB_Sample
{
    [DynamoDBTable("Movies")]
    public class Movie
    {
        [DynamoDBHashKey]
        public string Title { get; set; }
        [DynamoDBRangeKey(AttributeName="Released")]
        public DateTime ReleaseDate { get; set; }

        public List<string> Genres { get; set; }
        [DynamoDBProperty("Actors")]
        public List<string> ActorNames { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} - {1}
Actors: {2}", Title, ReleaseDate, string.Join(", ", ActorNames.ToArray()));
        }
    }

    [DynamoDBTable("Actors")]
    public class Actor
    {
        [DynamoDBHashKey]
        public string Name { get; set; }

        public string Bio { get; set; }
        public DateTime BirthDate { get; set; }

        [DynamoDBProperty(AttributeName = "Height")]
        public float HeightInMeters { get; set; }

        [DynamoDBProperty(Converter=typeof(AddressConverter))]
        public Address Address { get; set; }

        [DynamoDBIgnore]
        public string Comment { get; set; }

        public TimeSpan Age
        {
            get
            {
                return DateTime.UtcNow - BirthDate.ToUniversalTime();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, BirthDate);
        }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class AddressConverter : IPropertyConverter
    {
        private XmlSerializer _serializer = new XmlSerializer(typeof(Address));

        #region IPropertyConverter Members

        public object FromEntry(DynamoDBEntry entry)
        {
            Primitive primitive = entry as Primitive;
            if (primitive == null) return null;

            if (primitive.Type != DynamoDBEntryType.String) throw new InvalidCastException();
            string xml = primitive.AsString();
            using (StringReader reader = new StringReader(xml))
            {
                return _serializer.Deserialize(reader);
            }
        }

        public DynamoDBEntry ToEntry(object value)
        {
            Address address = value as Address;
            if (address == null) return null;

            string xml;
            using (StringWriter stringWriter = new StringWriter())
            {
                _serializer.Serialize(stringWriter, address);
                xml = stringWriter.ToString();
            }
            return new Primitive(xml);
        }

        #endregion
    }
}
