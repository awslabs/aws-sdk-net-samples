# Amazon CloudWatch PutMetricData Sample

This sample demonstrates using the AWS SDK for .NET to sample windows performance counters and use them as custom metrics in CloudWatch.

### Running the Sample

To run the Amazon CloudWatch PutMetricData sample:

1. Open the Amazon_CloudWatch_Sample.sln file in Visual Studio.
2. Open the App.config file.
3. Configure your AWS credentials. View the [developer guide](http://docs.aws.amazon.com/AWSSdkDocsNET/latest/DeveloperGuide/net-dg-config-creds.html) for configuring AWS credentials:
4. Run the sample in Debug mode by typing F5.
5. The sample runs as a console application and updates two CloutWatch custom metrics once every minute, PagingFilePctUsage and PagingFilePctUsagePeak.
6. It may take 15 minutes for these metrics to appear in the [Amazon Cloudwatch console](https://console.aws.amazon.com/cloudwatch).
7. Under metrics, in the left hand navigation, select 'EC2' under Metrics.
8. Select the checkboxes for PagingFilePctUsage and PagingFilePctUsagePeak
9. Select a time range of 24H, and a period of 5 minutes.

See the [Amazon Cloudwatch product page](http://aws.amazon.com/cloudwatch/) for more inforamation about Amazon CloudWatch.

