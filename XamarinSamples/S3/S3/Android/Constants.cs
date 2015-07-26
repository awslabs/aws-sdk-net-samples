
using Amazon;
using System.Net;

namespace S3.Droid
{
    public class Constants
    {

        // You should replace these values with your own
        public const string COGNITO_POOL_ID = "";


        // Note, the bucket will be created in all lower case letters
        // If you don't enter an all lower case title, any references you add
        // will need to be sanitized
        public const string BUCKET_NAME = "s3_droid_sample_bucket";

        public static RegionEndpoint REGION = RegionEndpoint.USEast1;

        public const HttpStatusCode NO_SUCH_BUCKET_STATUS_CODE = HttpStatusCode.NotFound;
        public const HttpStatusCode BUCKET_ACCESS_FORBIDDEN_STATUS_CODE = HttpStatusCode.Forbidden;
        public const HttpStatusCode BUCKET_REDIRECT_STATUS_CODE = HttpStatusCode.Redirect;

    }
}