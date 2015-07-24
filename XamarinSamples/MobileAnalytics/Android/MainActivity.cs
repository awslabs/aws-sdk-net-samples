using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Amazon.MobileAnalytics.MobileAnalyticsManager;
using Amazon.Runtime;
using Amazon;
using Amazon.MobileAnalytics.Model;
using System.Net;
using System.Collections.Generic;
using Amazon.MobileAnalytics;
using System.Collections;
using ThirdParty.Json.LitJson;
using Amazon.CognitoIdentity;

namespace MobileAnalyticsSample_Android
{
    [Activity(Label = "MobileAnalyticsSample_Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const string APP_ID = "YourAppID";
        private string COGNITO_POOL_ID = "YourPoolID";
        private RegionEndpoint COGNITO_REGION = RegionEndpoint.USEast1;
        
        private static MobileAnalyticsManager _manager = null;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button customEventButton = FindViewById<Button>(Resource.Id.button_custom_event);
            Button monetizationEventButton = FindViewById<Button>(Resource.Id.button_monetization_event);


            customEventButton.Click += delegate
            {
                CustomEvent customEvent = new CustomEvent("level_complete");

                customEvent.AddAttribute("LevelName", "Level5");
                customEvent.AddAttribute("Successful", "True");
                customEvent.AddMetric("Score", 12345);
                customEvent.AddMetric("TimeInLevel", 64);

                _manager.RecordEvent(customEvent);
            };


            monetizationEventButton.Click += delegate
            {
                MonetizationEvent monetizationEvent = new MonetizationEvent();

                monetizationEvent.Quantity = 10.0;
                monetizationEvent.ItemPrice = 2.00;
                monetizationEvent.ProductId = "ProductId123";
                monetizationEvent.ItemPriceFormatted = "$2.00";
                monetizationEvent.Store = "Apple";
                monetizationEvent.TransactionId = "TransactionId123";
                monetizationEvent.Currency = "USD";

                _manager.RecordEvent(monetizationEvent);
            };


            // customize your Mobile Analytics Manager Config
			MobileAnalyticsManagerConfig config = new MobileAnalyticsManagerConfig();
			config.AllowUseDataNetwork = true;
            
            _manager = MobileAnalyticsManager.GetOrCreateInstance(APP_ID, new CognitoAWSCredentials(COGNITO_POOL_ID, COGNITO_REGION), RegionEndpoint.USEast1, config);
        }

        protected override void OnResume()
        {
            _manager.ResumeSession();
            base.OnResume();
        }

        protected override void OnPause()
        {
            _manager.PauseSession();
            base.OnPause();
        }
    }
}

