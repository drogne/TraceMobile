using System;
using System.IO;
using Xamarin.Forms;

namespace PassFailSample.Models
{
    public interface IBarcodeScanner
    {
        // QUESTION: guessing this is not yet actually hooked up to anything in the Android Barcode scanner implementation?
        // is _ConnectingToUSB supposed to be IsConnected?
        bool IsConnected { get; }

        bool TryConnect();
        void Disconnect();
        void Trigger();
        
        //event EventHandler<string> BarcodeScanned;
        //event EventHandler<Exception> ErrorOccurred;
        //event EventHandler<MemoryStream> ImageArrived;
    }
}