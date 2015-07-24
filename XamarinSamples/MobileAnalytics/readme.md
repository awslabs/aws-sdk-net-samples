#Mobile Analytics Sample

###How to Run the Amazon Mobile Analytics Sample

#####Setting up the SDK and Sample

1. Get your App ID in the [Amazon Mobile Analytics console](https://console.aws.amazon.com/mobileanalytics/home/?region=us-east-1#/overview?consoleState=management). 
2. Create a Cognito Identity Pool to handle authentication with AWS.  A pool can be created on the [Cognito console]( https://console.aws.amazon.com/cognito/home).
3. Add your App ID and Cognito Pool ID in MainActivity.cs(Android sample) and AppDelegate.cs(iOS sample).
4. If you are using visual studio then right click on the solution and click on **Manage Nuget Packages**. Go to **Settings** on the Bottom left corner and add a new package source pointing it to local directory where you have downlaoded the nuget package. Move this package source to the top by clicking on the up Arrow and click on OK. Select **Include Pre-releases** in the dropdown menu and install the packages AWSSDK.CognitoIdentity and AWSSDK.MobileAnalytics.
5. If you are using Xamarin Studio, open the solution file and right click on sample project and right click -> Add -> Add Nuget Packages. Click on **Show pre-release package** (bottom left). At the top left click on the drop down menu and select **Configure Sources** and add a new nuget source pointing to local directory where you download the nuget package. Install the packages AWSSDK.CognitoIdentity and AWSSDK.MobileAnalytics.

#####Run the sample
1. Connect your Mobile device to PC and click "debug" button in Visual Studio or Xamarin Studio.
2. In the app view, you can press two buttons. One will record a custom event and the other will record a monetization event.
Note: The AWS SDK sends events every 60 seconds. After clicking on one of the buttons to record an event, you need to wait up to 60 seconds for the events to be delivered (you should see a logging message stating successful delivery when this happens). Once sent, it will take up to an hour for the events to show up on the Amazon Mobile Analytics console.

