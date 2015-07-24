using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using System.Collections.Generic;
using Java.Security;
using Android.Util;

[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace ContactManager.Droid
{
    [Activity(Label = "My Contacts", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {

        ICallbackManager callbackManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            FacebookSdk.SdkInitialize(this.ApplicationContext);

            LoadApplication(new App());

            callbackManager = CallbackManagerFactory.Create();

            LoginManager.Instance.RegisterCallback(callbackManager, new FacebookCallback<LoginResult>()
            {
                HandleSuccess = loginResult =>
                {
                    var accessToken = loginResult.AccessToken;
                    App.OnFacebookAuthSuccess(accessToken.Token);
                },
                HandleCancel = () =>
                {
                    App.OnFacebookAuthFailed();
                },
                HandleError = loginError =>
                {
                    App.OnFacebookAuthFailed();
                }
            });

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
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

    }
}

