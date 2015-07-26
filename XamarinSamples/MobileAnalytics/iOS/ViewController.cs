using System;
using UIKit;
using Amazon.MobileAnalytics.MobileAnalyticsManager;

namespace MobileAnalyticsSample.iOS
{
    public partial class ViewController : UIViewController
    {
        public static MobileAnalyticsManager Manager { get; set; }


        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


        partial void UIButton4_TouchUpInside(UIButton sender)
        {
            // Custom Event Buttoon Handler
            CustomEvent customEvent = new CustomEvent("level_complete");

            customEvent.AddAttribute("LevelName", "Level5");
            customEvent.AddAttribute("Successful", "True");
            customEvent.AddMetric("Score", 12345);
            customEvent.AddMetric("TimeInLevel", 64);

            Manager.RecordEvent(customEvent);
        }

        partial void UIButton6_TouchUpInside(UIButton sender)
        {
            // Monetization Event Button Handler
            MonetizationEvent monetizationEvent = new MonetizationEvent();

            monetizationEvent.Quantity = 10.0;
            monetizationEvent.ItemPrice = 2.00;
            monetizationEvent.ProductId = "ProductId123";
            monetizationEvent.ItemPriceFormatted = "$2.00";
            monetizationEvent.Store = "Apple";
            monetizationEvent.TransactionId = "TransactionId123";
            monetizationEvent.Currency = "USD";

            Manager.RecordEvent(monetizationEvent);
        }
    }
}