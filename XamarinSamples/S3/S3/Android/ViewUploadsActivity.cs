using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Amazon.S3.Model;
using Android.Graphics;
using System.Threading.Tasks;
using System.IO;

namespace S3.Droid
{
    [Activity(Label = "ViewUploadsActivity")]
    public class ViewUploadsActivity : Activity
    {

        GridView gridView;
        ListObjectsResponse response;
        List<Bitmap> images = new List<Bitmap>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ViewUploads);


            gridView = FindViewById<GridView>(Resource.Id.GridView);
            gridView.Adapter = new ImageAdapter(this,images);
            LoadImages();
        }

        private async Task LoadImages()
        {
            var client = S3Utils.S3Client;

            response = await client.ListObjectsAsync(new ListObjectsRequest()
            {
                BucketName = Constants.BUCKET_NAME.ToLowerInvariant(),
                Prefix = "myPhoto",
                MaxKeys = 20
            });

            response.S3Objects.ForEach(async (s3Object) =>
            {
                var res = await client.GetObjectAsync(new GetObjectRequest()
                {
                    BucketName = Constants.BUCKET_NAME.ToLowerInvariant(),
                    Key = s3Object.Key
                });

                using (var stream = res.ResponseStream)
                {
                    var b = ReadFully(stream);
                    var bitmap = BitmapFactory.DecodeByteArray(b, 0, b.Length);
                    images.Add(bitmap);
                }

                RunOnUiThread(() =>
                {
                    var adp = gridView.Adapter as ImageAdapter;
                    adp.NotifyDataSetChanged();
                });
            });
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        public class ImageAdapter : BaseAdapter
        {
            private readonly Context context;
            private List<Bitmap> images;
            public ImageAdapter(Context c,List<Bitmap> i)
            {
                context = c;
                images = i;
            }

            public override int Count
            {
                get { return images.Count; }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return images[position] as Java.Lang.Object;
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                ImageView imageView;

                if (convertView == null)
                {
                    // if it's not recycled, initialize some attributes
                    imageView = new ImageView(context);
                    imageView.LayoutParameters = new AbsListView.LayoutParams(85, 85);
                    imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                    imageView.SetPadding(8, 8, 8, 8);
                }
                else
                {
                    imageView = (ImageView)convertView;
                }

                imageView.SetImageBitmap(images[position]);

                return imageView;
            }
        }
    }
}