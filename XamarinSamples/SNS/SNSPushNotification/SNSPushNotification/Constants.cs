using System;
using System.Collections.Generic;
using System.Text;
using Amazon;

namespace SNSPushNotification
{
    public class Constants
    {
        //identity pool id for cognito credentials
        public const string IdentityPoolId = "";

        //sns android platform arn
        public const string AndroidPlatformApplicationArn = "";
        public const string FCMPlatformApplicationArn = "";
         

        
        //sns ios platform arn
        public const string iOSPlatformApplicationArn = "";

        //project id for android gcm
        public const string GoogleConsoleProjectId = "";

        public static RegionEndpoint CognitoRegion = RegionEndpoint.USEast1;
        public static RegionEndpoint SnsRegion = RegionEndpoint.USEast1;

    }
}
