# AWS SDK for Dot Net - Xamarin SNS Sample

#### This sample demonstrates how to use Amazon SNS with the AWS Mobile SDK for Xamarin. 

##### Configure the SNS Sample

1. To run the SNS Sample you will need to create a Cognito Identity Pool, to handle authentication with AWS.  A pool can be created on the [Cognito console]( https://console.aws.amazon.com/cognito/home).
2. You will also need to create an SNS Platform Application for APNS (iOS) and GCM (Android). SNS Platform Application endpoint can be created on the [SNS Console]( https://console.aws.amazon.com/sns/v2/home) .
3. Open the Constants.cs file and update the following IdentityPoolId, AndroidPlatformApplicationArn, iOSPlatformApplicationArn, GoogleConsoleProjectId, CognitoRegion, SnsRegion.
4. Add the following nuget packages to Droid and iOS projects - AWSSDK.CognitoIdentity, AWSSDK.SimpleNotificationService.

#####Run the sample
1. Connect your Device/Start your simulator. 
2. Run the application.