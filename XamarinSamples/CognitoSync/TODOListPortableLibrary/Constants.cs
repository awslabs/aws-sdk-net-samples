using Amazon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOListPortableLibrary
{
    public class Constants
    {
        //identity pool id for cognito credentials
        public const string IdentityPoolId = "";
        public const string PROVIDER_NAME = "graph.facebook.com";
        //set your regionendpoints here
        public static RegionEndpoint CognitoIdentityRegion = RegionEndpoint.USEast1;
        public static RegionEndpoint CognitoSyncRegion = RegionEndpoint.USEast1;
    }
}
