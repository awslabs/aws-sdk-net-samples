/*******************************************************************************
* Copyright 2009-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
* 
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using Microsoft.Win32;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace AmazonS3TransferUtility_Sample
{
    /// <summary>
    /// Interaction logic for S3TransferWindow.xaml
    /// </summary>
    public partial class S3TransferWindow : Window, INotifyPropertyChanged
    {
        public const int FIVE_MINUTES = 5 * 60 * 1000;

        TransferUtility _transferUtility;

        string _bucket;
        string _uploadFile;
        string _uploadDirectory;

        #region Initialization
        public S3TransferWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            loadConfiguration();
        }

        /// <summary>
        /// This method loads the AWSProfileName that is set in the App.config and creates the transfer utility.
        /// </summary>
        private void loadConfiguration()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
            {
                MessageBox.Show(this, "AWSProfileName is not set in the App.Config", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            this._transferUtility = new TransferUtility(RegionEndpoint.USWest2);

            // Update the Bucket to the optionally supplied Bucket from the App.config.
            this.Bucket = appConfig["Bucket"];
        }
        #endregion

        #region Bound Properties

        public string Bucket
        {
            get { return this._bucket; }
            set
            {
                this._bucket = value;
                this.notifyPropertyChanged("Bucket");
            }
        }

        public string UploadFile
        {
            get { return this._uploadFile; }
            set
            {
                this._uploadFile = value;
                this.notifyPropertyChanged("UploadFile");
            }
        }

        public string UploadDirectory
        {
            get { return this._uploadDirectory; }
            set
            {
                this._uploadDirectory = value;
                this.notifyPropertyChanged("UploadDirectory");
            }
        }
        #endregion

        #region Button Click Event Handlers
        private void browseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                this.UploadFile = dlg.FileName;
            }
        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {
            // Ensure the progress bar is empty.
            resetProgressBars();
            updateIsEnabled(this._ctlUploadFile, false);

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.threadedUploadFile));
        }

        private void browseDirectory_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.Description = "Select a folder to upload.";
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            System.Windows.Forms.DialogResult result = dlg.ShowDialog(new WindowWrapper(source.Handle));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.UploadDirectory = dlg.SelectedPath;
            }
        }

        private void uploadDirectory_Click(object sender, RoutedEventArgs e)
        {
            resetProgressBars();
            updateIsEnabled(this._ctlUploadDirectory, false);

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.threadedUploadDirectory));
        }
        #endregion

        #region S3 Upload Call

        /// <summary>
        /// This method is called in a background thread so as not to block the UI as the upload is 
        /// going.
        /// </summary>
        /// <param name="state">unused</param>
        private void threadedUploadFile(object state)
        {
            try
            {
                // Make sure the bucket exists
                this._transferUtility.S3Client.PutBucket(new PutBucketRequest() { BucketName = this.Bucket });

                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest()
                {
                    BucketName = this.Bucket,
                    FilePath = this.UploadFile
                };
                request.UploadProgressEvent += this.uploadFileProgressCallback;

                this._transferUtility.Upload(request);

                displayMessageBox("Completed file upload!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                displayMessageBox(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                updateIsEnabled(this._ctlUploadFile, true);
            }
        }

        /// <summary>
        /// This method is called in background thread so as not to block the UI as the upload is 
        /// going.
        /// </summary>
        /// <param name="state">unused</param>
        private void threadedUploadDirectory(object state)
        {
            try
            {
                // Make sure the bucket exists
                this._transferUtility.S3Client.PutBucket(new PutBucketRequest() { BucketName = this.Bucket });

                TransferUtilityUploadDirectoryRequest request = new TransferUtilityUploadDirectoryRequest()
                {
                    BucketName = this.Bucket,
                    Directory = this.UploadDirectory,
                    SearchOption = SearchOption.AllDirectories
                };
                request.UploadDirectoryProgressEvent += this.uploadDirectoryProgressCallback;
                this._transferUtility.UploadDirectory(request);

                displayMessageBox("Completed directory upload!", "Success", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                updateIsEnabled(this._ctlUploadDirectory, true);
            }
        }
        #endregion

        #region Upload Event Callbacks

        // This gets called as bytes are written to the request stream.  The sender is the TransferUtilityUploadRequest
        // that was used to start the upload. The UploadProgressArgs contains the total bytes to be transferred and how many bytes 
        // have already been transferred.
        private void uploadFileProgressCallback(object sender, UploadProgressArgs e)
        {
            updateProgressBar(this._ctlFileProgressBar, 0, e.TotalBytes, e.TransferredBytes,
                this._ctlFileTransferLabel, "Bytes", null);
        }

        // This gets called as bytes are written to the request stream.  The sender is the TransferUtilityUploadDirectoryRequest
        // that was used to start the upload. The UploadDirectoryProgressArgs contains the total number of files that will be upload, 
        // how many files have been upload so far, total number of bytes to be transferred for the current file being upload and
        // how many bytes have been upload so far for the current file being uploaded.
        private void uploadDirectoryProgressCallback(object sender, UploadDirectoryProgressArgs e)
        {
            updateProgressBar(this._ctlDirectoryFileProgressBar, 0, e.TotalNumberOfFiles, e.NumberOfFilesUploaded,
                this._ctlNumberOfFilesLabel, "Files", null);
            updateProgressBar(this._ctlDirectoryCurrentFileProgressBar, 0, e.TotalNumberOfBytesForCurrentFile, e.TransferredBytesForCurrentFile,
                this._ctlCurrentFilesTransferLabel, "Bytes", e.CurrentFile);
        }

        #endregion

        #region UI Utility Methods
        private void updateProgressBar(ProgressBar bar, long min, long max, long value, TextBlock label, string labelPostFix, string filepath)
        {
            bar.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new DispatcherOperationCallback(delegate
                {
                    bar.Minimum = min;
                    bar.Maximum = max;
                    bar.Value = value;

                    if (label != null)
                    {
                        string labelText = string.Format("{0} / {1} {2}",
                            value == int.MinValue ? "0" : value.ToString("#,0"),
                            max == int.MaxValue ? "0" : max.ToString("#,0"),
                            labelPostFix);

                        if (!string.IsNullOrEmpty(filepath))
                        {
                            labelText += string.Format(" ({0})", new FileInfo(filepath).Name);
                        }

                        label.Text = labelText;
                    }
                    return null;
                }), null);
        }

        private void updateIsEnabled(Control btn, bool enabled)
        {
            btn.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new DispatcherOperationCallback(delegate
                {
                    btn.IsEnabled = enabled;
                    return null;
                }), null);
        }

        private void resetProgressBars()
        {
            updateProgressBar(this._ctlFileProgressBar, int.MinValue, int.MaxValue, int.MinValue,
                this._ctlFileTransferLabel, "Bytes", null);
            updateProgressBar(this._ctlDirectoryFileProgressBar, int.MinValue, int.MaxValue, int.MinValue,
                this._ctlNumberOfFilesLabel, "Files", null);
            updateProgressBar(this._ctlDirectoryCurrentFileProgressBar, int.MinValue, int.MaxValue, int.MinValue,
                this._ctlCurrentFilesTransferLabel, "Bytes", null);
        }

        private void displayMessageBox(string message, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new DispatcherOperationCallback(delegate
                {
                    MessageBox.Show(this, message, caption, button, image);
                    return null;
                }), null);
        }

        /// <summary>
        /// Utility Class to get the Winform Directroy browser to work in WPF.
        /// </summary>
        class WindowWrapper : System.Windows.Forms.IWin32Window
        {
            IntPtr _handle;
            public WindowWrapper(IntPtr handle)
            {
                this._handle = handle;
            }

            IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return this._handle; }
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
