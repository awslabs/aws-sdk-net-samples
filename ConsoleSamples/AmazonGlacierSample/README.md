## Amazon Glacier Sample
This is a sample that demonstrates how to use the Amazon Glacier using the high level API provided by ArchiveTransferManager.

### Running the Sample
The basic steps for running the Amazon Glacier sample are:

1. Open the AmazonGlacierSample.sln file in Visual Studio.
2. Open the App.config file.
3. Configure your AWS credentials. View the [developer guide](http://docs.aws.amazon.com/AWSSdkDocsNET/latest/DeveloperGuide/net-dg-config-creds.html) for configuring AWS credentials:
4. Set proper values for the variables vaultName, filePath and downloadFilePath. (Please note that download of an archive can take hours to complete.)
5. Run the sample in Debug mode by typing F5.
