# AWS SDK for Dot Net - Xamarin Cognito Sync Manager Sample

#### This sample demonstrates how to use Amazon Sync Manager with the AWS Mobile SDK for Xamarin. 

##### Configure the Sync Manager Sample

1. To run the Sync Manager Sample you will need to create a Cognito Identity Pool, to handle authentication with AWS.  A pool can be created on the [Cognito console]( https://console.aws.amazon.com/cognito/home). Follow the steps on the console and create an Auth Pool with your facebook login. 
2. Open the Constants.cs file and update the following IdentityPoolId, CognitoIdentityRegion,CognitoSyncRegion.
3. Update the facebook application id  & facebook application name in strings.xml(string values app_id, app_name) for Android.
4. Update the facebook application id  & facebook application name in AppDelegate for iOS.(appId & appName) for iOS.
5. Add the following nuget packages to Droid and iOS project xamarin.facebook & AWSCore
6. Add the following nuget packages to portable project AWS Cognito SyncManager.

#####Run the sample
1. Connect your Device/Start your simulator. 
2. Run the application.