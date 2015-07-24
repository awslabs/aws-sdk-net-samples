using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Java.IO;
using Android.Graphics;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.S3;

namespace S3.Droid
{
    [Activity(Label = "S3AndroidSample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        File appDirectory;
        File file;
        ImageView Picture;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button initialize = FindViewById<Button>(Resource.Id.InitializeButton);
            initialize.Click += InitializeClick;

            Button takePicture = FindViewById<Button>(Resource.Id.TakePictureButton);
            takePicture.Click += TakePictureClick;

            Picture = FindViewById<ImageView>(Resource.Id.Picture);

            Button upload = FindViewById<Button>(Resource.Id.UploadButton);
            upload.Click += UploadClick;

            Button viewUploads = FindViewById<Button>(Resource.Id.ViewMyUploads);
            viewUploads.Click += delegate { StartActivity(typeof(ViewUploadsActivity)); };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = Picture.Height;
            var bitmap = file.Path.LoadAndResizeBitmap(width, height);
            if (bitmap != null)
            {
                Picture.SetImageBitmap(bitmap);
                bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        #region private methods


        private async void UploadClick(object sender, EventArgs e)
        {
            if (file == null)
            {
                Toast.MakeText(this, "No file to upload", ToastLength.Short).Show();
                return;
            }

            var S3Client = S3Utils.S3Client;
            try
            {
                var response = await S3Client.PutObjectAsync(new PutObjectRequest()
                {
                    BucketName = Constants.BUCKET_NAME.ToLowerInvariant(),
                    FilePath = file.AbsolutePath,
                    Key = file.Name
                });

                Toast.MakeText(this, "File uploaded to S3 Bucket", ToastLength.Long).Show();

            }
            catch (AmazonS3Exception s3Exception)
            {
                Toast.MakeText(this, "Upload failed, check logs for more information", ToastLength.Long).Show();
                System.Console.WriteLine(s3Exception.StackTrace);
            }
        }

        private async void InitializeClick(object sender, EventArgs e)
        {
            bool bucketExists = await S3Utils.BucketExist();
            if (!bucketExists)
                await S3Utils.CreateBucket();

            appDirectory = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "S3Droid");
            if (!appDirectory.Exists())
                appDirectory.Mkdirs();
        }

        private void TakePictureClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            file = new File(appDirectory, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
            StartActivityForResult(intent, 0);
        }

        #endregion
    }
}

