# AWS SDK for Dot Net - Xamarin DynamoDB Sample

#### This sample demonstrates how to use Amazon DynamoDB with the AWS Mobile SDK for Xamarin. 


##### Configure the DynamoDB Sample

1. To run the DynamoDB Sample you will need to create a Cognito Identity Pool, to handle authentication with AWS.  A pool can be created on the [Cognito console]( https://console.aws.amazon.com/cognito/home). Follow the steps on the console and create an Auth Pool with your facebook login. 

2. Create the DynamoDB table Named "ContactList" with Hash & Range Keys Hash Key "Id" and  range Key "UserId" (Refer Contacts.cs). Once the table is created copy the Table ARN and go back to IAM and select the Cognito Auth Policy and add the following custom policy , be sure to update your table arn before adding the custom policy

	```
	{
		"Version": "2012-10-17",
		"Statement": [
			{
				"Effect": "Allow",
				"Action": [
					"dynamodb:*"
				],
				"Resource": [
					"<table arn>"
				],
				"Condition": {
					"ForAllValues:StringEquals": {
						"dynamodb:LeadingKeys": [
							"${cognito-identity.amazonaws.com:sub}"
						]
					}
				}
			}
		]
	}
	```

	This will ensure fine grained access control for the DynamoDB tables

3. Open the Constants.cs file and update the following COGNITO_POOL_ID, COGNITO_REGION, DYNAMODB_REGION.
4. Update the facebook application id  & facebook application name in strings.xml & Constants.cs
5. Add the following nuget packages to Droid and iOS project xamarin.facebook & AWSCore
6. Add the following nuget packages to portable project AWS Cognito Identity & AWS DynamoDB.


#####Run the sample
1. Connect your Android Device/Start your simulator. 
2. Run the application.