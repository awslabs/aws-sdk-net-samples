using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Facebook.LoginKit;
using TODOListPortableLibrary;

namespace TODOList.iOS
{
    public class LoginViewController : UIViewController
    {
        List<string> readPermissions = new List<string> { "public_profile" };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoginManager login = new LoginManager();
            login.LogInWithReadPermissions(readPermissions.ToArray(), delegate(LoginManagerLoginResult result, NSError error)
            {
                if (error != null)
                {
                    UpdateCredentials(string.Empty);
                }
                else if (result.IsCancelled)
                {
                    UpdateCredentials(string.Empty);
                }
                else
                {
                    var accessToken = result.Token;
                    UpdateCredentials(accessToken.TokenString);

                    ((AppDelegate)UIApplication.SharedApplication.Delegate).UpdateRootViewController(new TODOViewController());
                }
            });

        }

        private void UpdateCredentials(string token)
        {
            var cred = CognitoSyncUtils.Credentials;
            if (string.IsNullOrEmpty(token))
            {
                cred.RemoveLogin(Constants.PROVIDER_NAME);
            }
            else
            {
                cred.AddLogin(Constants.PROVIDER_NAME, token);
            }
        }

    }
}