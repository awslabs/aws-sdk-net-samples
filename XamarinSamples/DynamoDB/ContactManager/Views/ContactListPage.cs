using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContactManager
{
    public class ContactListPage : ContentPage
    {
        ListView listView;

        public ContactListPage()
        {
            Title = "My Contacts";

            NavigationPage.SetHasNavigationBar(this, true);

            listView = new ListView
            {
                RowHeight = 40,
                ItemTemplate = new DataTemplate(typeof(ContactCell))
            };

            listView.ItemSelected += (sender, e) =>
            {
                var contact = (Contact)e.SelectedItem;
                var contactPage = new ContactPage();
                contactPage.BindingContext = contact;
                Navigation.PushAsync(contactPage);
            };

            var layout = new StackLayout();

            layout.Children.Add(listView);
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = layout;
            ToolbarItem tbi = null;
            if (Device.OS == TargetPlatform.iOS)
            {
                tbi = new ToolbarItem("+", null, () =>
                {
                    var contact = new Contact();
                    var contactPage = new ContactPage();
                    contactPage.BindingContext = contact;
                    Navigation.PushAsync(contactPage);
                }, 0, 0);
            }
            if (Device.OS == TargetPlatform.Android)
            {
                tbi = new ToolbarItem("+", "plus", () =>
                {
                    var contact = new Contact();
                    var contactPage = new ContactPage();
                    contactPage.BindingContext = contact;
                    Navigation.PushAsync(contactPage);
                }, 0, 0);
            }

            ToolbarItems.Add(tbi);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadContacts().ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    listView.ItemsSource = task.Result;
                });
            });
        }

        private Task<List<Contact>> LoadContacts()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var search = context.ScanAsync<Contact>(conditions);
            return search.GetNextSetAsync();
        }
    }
}
