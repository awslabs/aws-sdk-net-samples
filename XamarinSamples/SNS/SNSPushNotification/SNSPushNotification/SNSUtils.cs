using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SNSPushNotification
{
    public class SNSUtils
    {

        public enum Platform
        {
            Android,
            IOS,
            WindowsPhone
        }

        private static AWSCredentials _credentials;

        private static AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(Constants.IdentityPoolId, Constants.CognitoRegion);
                return _credentials;
            }
        }

        private static IAmazonSimpleNotificationService _snsClient;

        private static IAmazonSimpleNotificationService SnsClient
        {
            get
            {
                if (_snsClient == null)
                    _snsClient = new AmazonSimpleNotificationServiceClient(Credentials, Constants.SnsRegion);
                return _snsClient;
            }
        }

        public static async Task RegisterDevice(Platform platform, string registrationId)
        {
            var arn = string.Empty;
            string _endpointArn = string.Empty;
            switch (platform)
            {
                case Platform.Android:
                    arn = Constants.AndroidPlatformApplicationArn;
                    break;
                case Platform.IOS:
                    arn = Constants.iOSPlatformApplicationArn;
                    break;
            }

            var response = await SnsClient.CreatePlatformEndpointAsync(new CreatePlatformEndpointRequest
                {
                    Token = registrationId,
                    PlatformApplicationArn = arn
                }
            );

            _endpointArn = response.EndpointArn;

        }

    }
}
