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
using Amazon.CognitoIdentity;
using System.Net.Http;
using ThirdParty.Json.LitJson;
using Amazon;

namespace AWSSDK.CognitoIdentity.Sample
{
    /// <summary>
    /// Your custom BYOI Credentials
    /// </summary>
    public class CognitoCustomAWSCredentials : CognitoAWSCredentials
    {
        private const string URL = "http://example.com/?alias={0}";
        private const string PROVIDER_NAME = "";
        private const string IDENTITY_POOL_ID = "";
        private static RegionEndpoint CognitoRegion = RegionEndpoint.USEast1;
        private string Username;

        public CognitoCustomAWSCredentials(string username)
            : base(IDENTITY_POOL_ID, CognitoRegion)
        {
            this.Username = username;
        }

        public override async System.Threading.Tasks.Task<CognitoAWSCredentials.IdentityState> RefreshIdentityAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(string.Format(URL,this.Username));
            var content = await response.Content.ReadAsStringAsync();
            JsonData json = JsonMapper.ToObject(content);

            //The backend has to send us back an Identity and a OpenID token
            string identityId = json["IdentityId"].ToString();
            string token = json["Token"].ToString();

            var idState = new IdentityState(identityId, PROVIDER_NAME, token, false);

            response.Dispose();
            client.Dispose();


            return idState;
        }
    }
}