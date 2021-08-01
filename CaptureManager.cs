using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.Capture;
using System.Threading.Tasks;
using Windows.System.Display;
using Windows.Storage;
using Windows.Media.MediaProperties;
using System.Diagnostics;
using Microsoft.Samples.SimpleCommunication;
using Windows.Storage.Streams;
using System.IO;

namespace ContinuousAudio
{
    internal class CaptureManager
    {
        public DisplayRequest displayRequest = new DisplayRequest();
        public bool isInitialized;
        public bool isRecording;
        private StorageFolder captureFolder = null;
        public static MemoryStream memStream = new MemoryStream();
        IRandomAccessStream stream = memStream.AsRandomAccessStream();

        MediaCapture captureManager;

        public CaptureManager()
        {

        }

        public async Task InitializeCameraAsync()
        {
            captureManager = new MediaCapture();
            await captureManager.InitializeAsync();
            isInitialized = true;
            if (isInitialized)
            {
                await StartRecordingAudioAsync();
            }
        }

        public async Task StartRecordingAudioAsync()
        {
            
            var encodingProfile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.High);
            await captureManager.StartRecordToStreamAsync(encodingProfile, stream);

            /*var picturesLibrary = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            captureFolder = picturesLibrary.SaveFolder;
            var audioFile = await captureFolder.CreateFileAsync("test.wav", CreationCollisionOption.GenerateUniqueName);
            await captureManager.StartRecordToStorageFileAsync(encodingProfile, audioFile);*/

            


           


            Debug.WriteLine("Started recording!");
            isRecording = true;
        }

        public async Task StopRecordingAudioAsync()
        {
            isRecording = false;
            await captureManager.StopRecordAsync();


            /*var picturesLibrary = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            captureFolder = picturesLibrary.SaveFolder;
            //captureFolder = @"C:\Users\40731\source\repos\ConnectToWebSocket\ConnectToWebSocket\bin\Debug\netcoreapp3.1";
            var audioFile = await captureFolder.CreateFileAsync("abcd.wav", CreationCollisionOption.GenerateUniqueName);*/

            MemoryStream ms = (MemoryStream)stream.AsStream();
            string sourcePath = @"C:\Users\40731\Pictures";
            string fileName = "abcd.wav";

            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            using (var fsSource = new FileStream(sourceFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                ms.WriteTo(fsSource);
            }
            Debug.WriteLine("Stopped recording!");

        }


    }
}
