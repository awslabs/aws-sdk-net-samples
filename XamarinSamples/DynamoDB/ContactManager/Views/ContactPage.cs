using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace ContactManager
{
    public class ContactPage : ContentPage
    {
        public ContactPage()
        {
            this.SetBinding(ContentPage.TitleProperty, "FirstName");
            NavigationPage.SetHasNavigationBar(this, true);

            var firstNameLabel = new Label { Text = "First Name" };
            var firstNameEntry = new DoneEntry { Keyboard = Keyboard.Text };
            firstNameEntry.SetBinding(Entry.TextProperty, "FirstName");

            var lastNameLabel = new Label { Text = "Last Name" };
            var lastNameEntry = new DoneEntry { Keyboard = Keyboard.Text };
            lastNameEntry.SetBinding(Entry.TextProperty, "LastName");

            var homePhoneNumberLabel = new Label { Text = "Home Phone" };
            var homePhoneNumberEntry = new DoneEntry { Keyboard = Keyboard.Telephone };
            homePhoneNumberEntry.SetBinding(Entry.TextProperty, "HomePhoneNumber");

            var workPhoneNumberLabel = new Label { Text = "Work Phone" };
            var workPhoneNumberEntry = new DoneEntry { Keyboard = Keyboard.Telephone };
            workPhoneNumberEntry.SetBinding(Entry.TextProperty, "WorkPhoneNumber");

            var mobileNumberLabel = new Label { Text = "Mobile Number" };
            var mobileNumberEntry = new DoneEntry { Keyboard = Keyboard.Telephone };
            mobileNumberEntry.SetBinding(Entry.TextProperty, "MobileNumber");

            var emailLabel = new Label { Text = "Email" };
            var emailEntry = new DoneEntry { Keyboard = Keyboard.Email };
            emailEntry.SetBinding(Entry.TextProperty, "EmailAddress");

            var saveButton = new Button { Text = "Save"};
            saveButton.Clicked += async (sender, e) =>
            {
                var contact = (Contact)BindingContext;
                var id = await AmazonUtils.Credentials.GetIdentityIdAsync();
                DynamoDBOperationConfig config = new DynamoDBOperationConfig
                {
                    IgnoreNullValues = false
                };
                if (string.IsNullOrEmpty(contact.UserId))
                {
                    var userId = Guid.NewGuid().ToString();
                    contact.Id = id;
                    contact.UserId = userId;
                }
                await AmazonUtils.DDBContext.SaveAsync<Contact>(contact, config);
                Navigation.PopAsync();
            };

            var deleteButton = new Button { Text = "Delete", BorderColor = Color.Red, BorderRadius = 2 };
            deleteButton.Clicked += async (sender, e) =>
            {
                var contact = (Contact)BindingContext;
                if (!string.IsNullOrEmpty(contact.UserId))
                {
                    contact.Id = await AmazonUtils.Credentials.GetIdentityIdAsync();
                    await AmazonUtils.DDBContext.DeleteAsync<Contact>(contact);
                }

                Navigation.PopAsync();
            };

            var cancelButton = new Button { Text = "Cancel", BorderColor = Color.Gray, BorderRadius = 2 };
            cancelButton.Clicked += (sender, e) =>
            {
                Navigation.PopAsync();
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Padding = new Thickness(20),
                    Children = {
					    firstNameLabel, firstNameEntry, 
					    lastNameLabel, lastNameEntry,
					    homePhoneNumberLabel, homePhoneNumberEntry,
                        workPhoneNumberLabel, workPhoneNumberEntry,
                        mobileNumberLabel, mobileNumberEntry,
                        emailLabel, emailEntry,
					    saveButton, cancelButton,deleteButton
				    }
                }
            };
        }
    }
}
