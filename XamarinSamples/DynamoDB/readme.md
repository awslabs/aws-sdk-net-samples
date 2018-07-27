# AWS SDK for Dot Net - Xamarin DynamoDB Sample

  
#### This sample demonstrates how to use Amazon DynamoDB with the AWS Mobile SDK for Xamarin.


##### Configure the DynamoDB Sample

1. Create a Cognito Identity Pool to handle authentication with AWS. Update the Cognito Policies to give access to all DynamoDB Operations.
2. Follow the link https://docs.aws.amazon.com/cognito/latest/developerguide/cognito-user-pools-social-idp.html,  register an app with Facebook  and add the Social IdP to the User Pool 
3. Open the Constants.cs file and update the following FB_APP_ID, FB_APP_NAME,PROVIDER_NAME,COGNITO_IDENTITY_POOL_ID and COGNITO_REGION.
4. Open Info.plist in ContactManager.iOS project, update the value for CFBundleURLSchemes, the value should be "fb<your facebook app id>", for example, the value can be fb111111111111, if your facebook app id is 111111111111
	
     	
 	

##### Issues may be encountered 
1. Build of ContactManager.iOS project fail with error [Compiling IB documents for earlier than iOS 7 is no longer supported.] in visual studio 2017.


     Solution:  Change the vaule of MinimumOSVersion in info.plist from 6.0 to 7.0
2. The application crash on ios simulator, an exception like "Application Settings exception, Unable to store key CognitoIdentity:IdentityId:..." is thrown. 

    Solution: Install provisioning profile on the mac on which the simulator runs and add custom entitilements from IOS Bundle Signing. Can follow this thread https://forums.developer.apple.com/thread/60617#180567
   
