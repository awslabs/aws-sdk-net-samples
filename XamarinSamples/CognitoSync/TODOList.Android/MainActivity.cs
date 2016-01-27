using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Facebook;
using Xamarin.Facebook.Share.Widget;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.AppEvents;
using Xamarin.Facebook.Share.Model;
using Xamarin.Facebook.Share;
using Xamarin.Facebook.Login;
using Android.Support.V4.App;
using TODOListPortableLibrary;
using System.Collections.Generic;
using Amazon;


[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace TODOList.Android
{
    [Activity(Label = "TODOList.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {
        int count = 1;

        LoginButton loginButton;

        ICallbackManager callbackManager;
        CognitoAccessTokenTracker tracker;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FacebookSdk.SdkInitialize(this.ApplicationContext);

            callbackManager = CallbackManagerFactory.Create();

            LoginManager.Instance.RegisterCallback(callbackManager, new FacebookCallback<LoginResult>()
            {
                HandleSuccess = loginResult =>
                {
                    var accessToken = loginResult.AccessToken;
                    UpdateCredentials(accessToken.Token);
                    Intent todoActivity = new Intent(this, typeof(TodoActivity));
                    StartActivity(todoActivity);
                },
                HandleCancel = () =>
                {
                    UpdateCredentials(string.Empty);
                },
                HandleError = loginError =>
                {
                    UpdateCredentials(string.Empty);
                }
            });
            LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile" });

            //initialize the logs

            var logConfig = AWSConfigs.LoggingConfig;
            logConfig.LogTo = LoggingOptions.SystemDiagnostics;
            logConfig.LogMetrics = true;
            logConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            logConfig.LogResponses = ResponseLoggingOption.Always;

        }


        private void GotoTodo()
        {
            Intent intent = new Intent(this, typeof(TodoActivity));
            StartActivity(intent);
        }

        private void trackerAccessTokenChangedEvent(object sender, AccessTokenChangedArgs e)
        {
            UpdateCredentials(e.NewAccessToken);
        }

        private void UpdateCredentials(string newToken)
        {
            var cred = CognitoSyncUtils.Credentials;
            if (string.IsNullOrEmpty(newToken))
                cred.RemoveLogin(Constants.PROVIDER_NAME);
            else
                cred.AddLogin(Constants.PROVIDER_NAME, newToken);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (tracker != null)
                tracker.StopTracking();
        }

        void ShowAlert(string title, string msg, string buttonText = null)
        {
            new AlertDialog.Builder(Parent)
                .SetTitle(title)
                .SetMessage(msg)
                .SetPositiveButton(buttonText, (s2, e2) => { })
                .Show();
        }
    }

    internal class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
    {
        public Action HandleCancel { get; set; }
        public Action<FacebookException> HandleError { get; set; }
        public Action<TResult> HandleSuccess { get; set; }

        public void OnCancel()
        {
            var c = HandleCancel;
            if (c != null)
                c();
        }

        public void OnError(FacebookException error)
        {
            var c = HandleError;
            if (c != null)
                c(error);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var c = HandleSuccess;
            if (c != null)
                c(result.JavaCast<TResult>());
        }
    }

    internal class AccessTokenChangedArgs : EventArgs
    {
        public string OldAccessToken { get; private set; }
        public string NewAccessToken { get; private set; }

        internal AccessTokenChangedArgs(string oldAccessToken, string newAccessToken)
        {
            this.OldAccessToken = oldAccessToken;
            this.NewAccessToken = newAccessToken;
        }

    }

    internal class CognitoAccessTokenTracker : AccessTokenTracker
    {

        internal event EventHandler<AccessTokenChangedArgs> AccessTokenChangedEvent;

        protected override void OnCurrentAccessTokenChanged(AccessToken p0, AccessToken p1)
        {
            if (AccessTokenChangedEvent != null)
            {
                var args = new AccessTokenChangedArgs(p0.Token, p1.Token);
                AccessTokenChangedEvent(this, args);
            }
        }
    }

}

