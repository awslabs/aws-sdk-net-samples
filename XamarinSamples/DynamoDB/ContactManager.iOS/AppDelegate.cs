using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Facebook.CoreKit;

using ContactManager;
using Xamarin.Forms;

namespace ContactManager.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        private UIWindow _window;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            Profile.EnableUpdatesOnAccessTokenChange(true);
            Settings.AppID = Constants.FB_APP_ID;
            Settings.DisplayName = Constants.FB_APP_NAME;


            global::Xamarin.Forms.Forms.Init();
            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            LoadApplication(new App());

            base.FinishedLaunching(app, options);

            return ApplicationDelegate.SharedInstance.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }
    }
}
