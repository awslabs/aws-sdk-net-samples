using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace SNSPushNotification.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new SNSPushNotification.App());

            app.RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData token)
        {
            var deviceToken = token.Description.Replace("<", "").Replace(">", "").Replace(" ", "");
            if (!string.IsNullOrEmpty(deviceToken))
            {
                SNSUtils.RegisterDevice(SNSUtils.Platform.IOS, deviceToken);
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Console.WriteLine(@"Failed to register for remote notification {0}", error.Description);
        }

    }
}
