using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Media;
using System;

namespace SNSPushNotification.Droid
{
    public class AndroidUtils
    {

        private static int REQUEST_CODE = 1001;

        public static void ShowNotification(Context context, string contentTitle,
                string contentText)
        {
            // Intent
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle(contentTitle)
                .SetContentText(contentText)
                .SetDefaults(NotificationDefaults.Sound|NotificationDefaults.Vibrate)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));

            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Notify(1001, builder.Build());
        }

        public static void ShowNotification(Context context, String contentText)
        {
            ShowNotification(context, "", contentText);
        }
    }
}