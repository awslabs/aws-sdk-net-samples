using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ContactManager
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            App.OnFacebookAuthSuccess = token =>
            {
                var cred = AmazonUtils.Credentials;
                cred.AddLogin(Constants.PROVIDER_NAME, token);
                ((App)App.Current).UpdateMainPage(new ContactListPage());
            };

            App.OnFacebookAuthFailed = () =>
            {
               
            };
        }
    }
}
