using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Amazon;

namespace AWSSDK.CognitoIdentity.Sample
{
    [Activity(Label = "AWSSDK.CognitoIdentity.Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += async delegate
            {
                var credentials = new CognitoCustomAWSCredentials("user1");
                var identity = await credentials.GetIdentityIdAsync();

                Console.WriteLine("The Cognito Identity is = {0}", identity);
            };
        }
    }
}

