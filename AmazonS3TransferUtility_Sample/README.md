## Amazon S3 TransferUtility Sample
This sample demonstrates using the AWS SDK for .NET and Windows Presentation Framework to upload files and directories and track progress via progress bars.  
The sample takes advantage of the new TransferUtility object available in the AWS SDK for .NET to simplify uploads as well take advantage of Amazon S3's multipart API for large files.

### Running the Sample
To run the Amazon S3 TransferUtility sample:

1. Open the AmazonS3TransferUtility_Sample.sln file in Visual Studio.
2. Open the App.config file.
3. Configure your AWS credentials. View the [developer guide](http://docs.aws.amazon.com/AWSSdkDocsNET/latest/DeveloperGuide/net-dg-config-creds.html) for configuring AWS credentials:
4. Optionally set the Bucket field in the App.config to prefill the text box in the application.
5. Run the sample in Debug mode by typing F5.
6. Enter a bucket name in the "Bucket" field if this was not already set in the App.config.  This bucket will be created if it does not exist.
7. To upload a file click the Browse button by the Choose File textbox to select a file and push the Upload button.
8. To upload a directory click the Browse button by the Choose Directory textbox to select a directory and push the Upload button.

See the [Amazon S3 Getting Started Guide](http://docs.amazonwebservices.com/AmazonS3/latest/gsg/) for more inforamation about Amazon S3.
