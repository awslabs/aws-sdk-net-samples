# AWS SDK for Dot Net - Xamarin Android S3 Sample

#### This sample demonstrates how to use Amazon S3 with the AWS Mobile SDK for Xamarin. 

##### Configure the S3 Sample
Note: To run the S3 Sample you will need to create a Cognito Identity Pool, to handle authentication with AWS.  A pool can be created on the [Cognito console]( https://console.aws.amazon.com/cognito/home). Update the Cognito Policies to give access to All S3 Operations.

1. Open the Constants.cs file and update the following COGNITO_POOL_ID, BUCKET_NAME, REGION.
2. If you are using visual studio then right click on the solution and click on Manage Nuget Packages. Go to settings on the Bottom left corner and add a new package source pointing it to local directory where you have downlaoded the nuget package. Move this package  source to the top by clicking on the up Arrow and click on OK. Select include pre-releases and install the packages AWSSDK.CognitoIdentity & AWSSDK.S3.
3. If you are using Xamarin Studio then Open the solution file and right click on AndroidSample project and right click -> Add -> nuget References. Click on Show Pre-release Package (bottom left). At the top left click on the drop down and select configure sources and add the location of the local file system and click on OK. Install the packages AWSSDK.CognitoIdentity & AWSSDK.S3.

#####Run the sample
1. Connect your Android Device/Start your simulator. 
2. Run the application.
3. Click on Initialize - This Creates the bucket if its not already present.
4. Take a picture 
5. Upload Picture
6. View all your Pics