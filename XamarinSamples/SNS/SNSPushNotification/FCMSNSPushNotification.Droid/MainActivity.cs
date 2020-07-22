using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Tasks;


namespace SNSPushNotification.Droid
{
    [Activity (Label = "SNSPushNotification", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug("MainActivity", GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug("MainActivity", "This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
                Log.Debug("MainActivity", "Google Play Services is available.");
                return true;
            }
        }
        void CreateNotificationChannel()
        {
            string token = FirebaseInstanceId.Instance.Token;
            Log.Debug("MainActivity", "current InstanceID token: " + token);
            if (!string.IsNullOrEmpty(token))
                SNSUtils.RegisterDevice(SNSUtils.Platform.FCM, token);
        }
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            global::Xamarin.Forms.Forms.Init (this, bundle);
            LoadApplication (new SNSPushNotification.App ());

            if(IsPlayServicesAvailable())
            {
                RegisterForSNS();
            }
        }

        private void RegisterForSNS()
        {
            string token = FirebaseInstanceId.Instance.Token;
            Log.Debug("MainActivity", "current InstanceID token: " + token);
            if (!string.IsNullOrEmpty(token))
                SNSUtils.RegisterDevice(SNSUtils.Platform.FCM, token);
        }
    } 
}

