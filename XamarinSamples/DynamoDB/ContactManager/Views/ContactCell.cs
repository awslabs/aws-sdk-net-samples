using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace ContactManager
{
    public class ContactCell : ViewCell
    {
        public ContactCell()
        {
            var label = new Label
            {
                YAlign = TextAlignment.Center
            };

            label.SetBinding(Label.TextProperty, "FirstName");

            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { label }
            };

            View = layout;
        }


        protected override void OnBindingContextChanged()
        {
            View.BindingContext = BindingContext;
            base.OnBindingContextChanged();
        }

    }
}
