using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Util.Internal.PlatformServices;

namespace SNSPushNotification
{
    public class SNSUtils
    {

        public enum Platform
        {
            Android,
            IOS,
            WindowsPhone,  
            FCM
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
                case Platform.FCM:
                    arn = Constants.FCMPlatformApplicationArn;
                    break;
            }

            var response = await SnsClient.CreatePlatformEndpointAsync(new CreatePlatformEndpointRequest
                {
                    Token = registrationId,
                    PlatformApplicationArn = arn
                }  
            );

            _endpointArn = response.EndpointArn;
            IApplicationSettings _appSetting = ServiceFactory.Instance.GetService<IApplicationSettings>();
            string oldEndpointArn = _appSetting.GetValue("FCMEndpointArn", ApplicationSettingsMode.Local);
            if(!string.IsNullOrWhiteSpace(oldEndpointArn))
            {
                if (oldEndpointArn != _endpointArn)
                {
                    await SnsClient.DeleteEndpointAsync(new DeleteEndpointRequest
                    {
                        EndpointArn = oldEndpointArn
                    }
                    );
                    _appSetting.SetValue("FCMEndpointArn", _endpointArn, ApplicationSettingsMode.Local);
                }
            }
            else
            {
                _appSetting.SetValue("FCMEndpointArn", _endpointArn, ApplicationSettingsMode.Local);
            }

           

        }

    }
}
