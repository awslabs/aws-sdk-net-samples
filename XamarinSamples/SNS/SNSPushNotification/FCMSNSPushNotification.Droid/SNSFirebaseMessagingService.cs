using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Firebase.Messaging;
using Android.Util;
using System.Collections.Generic;
using Android.Support.V4.App;

namespace SNSPushNotification.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class SNSFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "SNSFirebaseMessagingService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            var noti = message.GetNotification();
            foreach(var e in message.Data)
            {
                Log.Debug(TAG, "Notification Message Body: " + e);
            }
            var body = message.ToString();
            Log.Debug(TAG, "Notification Message Body: " + body);
            SendNotification(body, message.Data);
        }

 
        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            Log.Debug(TAG, "NEW_TOKEN" + token);
            SNSUtils.RegisterDevice(SNSUtils.Platform.FCM, token);
        }


        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }
            const int notification_id = 100;


            var pendingIntent = PendingIntent.GetActivity(this, notification_id, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, "my_notification_channel")
                                      .SetSmallIcon(Resource.Drawable.abc_list_pressed_holo_light)
                                      .SetContentTitle("FCM Message")
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(notification_id, notificationBuilder.Build());
        }
    }
}