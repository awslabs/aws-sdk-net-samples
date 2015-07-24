using Xamarin.Forms;
using Android.App;
using System;
using Xamarin.Forms.Platform.Android;
using ContactManager;
using ContactManager.Droid;
using Xamarin.Facebook.Login;
using Xamarin.Facebook;
using System.Collections.Generic;
using Object = Java.Lang.Object;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace ContactManager.Droid
{
    public class FacebookLoginButtonRenderer:ButtonRenderer
    {
        private static Activity _activity;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            _activity = this.Context as Activity;

            if (this.Control != null)
            {
                Android.Widget.Button button = this.Control;
                button.SetOnClickListener(ButtonClickListener.Instance.Value);
            }
        }
        
        private class ButtonClickListener : Object, IOnClickListener
        {
            public static readonly Lazy<ButtonClickListener> Instance = new Lazy<ButtonClickListener>(() => new ButtonClickListener());

            public void OnClick(View v)
            {    
                LoginManager.Instance.LogInWithReadPermissions(_activity, new List<string> { "public_profile" });
            }
        }

        

    }
}