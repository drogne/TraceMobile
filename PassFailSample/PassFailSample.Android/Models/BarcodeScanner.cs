using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Usb;
using Android.OS;
using Com.Cognex.Dataman.Sdk;
using Com.Cognex.Dataman.Sdk.Discovery;
using Com.Cognex.Mobile.Barcode.Sdk;
using Java.Nio;
using Java.Util;
using PassFailSample.Models;
using Exception = Java.Lang.Exception;
using Xamarin.Forms;
using PassFailSample.Droid.Models;
using Java.Lang;
using Android.Preferences;
using static Com.Cognex.Mobile.Barcode.Sdk.ReaderDevice;

[assembly: Dependency(typeof(BarcodeScanner))]
namespace PassFailSample.Droid.Models
{
    public class BarcodeScanner : Java.Lang.Object, ReaderDevice.IOnConnectionCompletedListener, ReaderDevice.IReaderDeviceListener, IBarcodeScanner
    {
        private const string SELECTED_DEVICE = "selectedDevice";
        private const string ACTIVE_SYMBOLOGIES = "activeSymbologies";

        private bool _ListeningForUsb = false;
        private static ContextWrapper _Activity;

        #region Properties

        public bool IsConnected
        {
            get
            {
                return _ReaderDevice?.ConnectionState == ConnectionState.Connecting || _ReaderDevice?.ConnectionState == ConnectionState.Connected;
            }
        }
        private static ReaderDevice _ReaderDevice { get; set; }
        private ISharedPreferences _SharedPreferences { get; set; }
        #endregion

        #region Static Methods

        internal void Initialize(ContextWrapper wrapper)
        {
            _Activity = wrapper;
            this._SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(wrapper);
            _ReaderDevice = ReaderDevice.GetMXDevice(_Activity);
            this.TryConnect();
        }
        
        #endregion

        public bool TryConnect()
        {
            try
            {
                //if (!this._ListeningForUsb)
                //{
                //    this._MxConnectedReceiver = new BroadcastReceiverTMP();
                //    _Activity.RegisterReceiver(this._MxConnectedReceiver, new IntentFilter(MXHelper.ActionMxAttached));
                //    this._ListeningForUsb = true;
                //}
                if (_ReaderDevice.GetAvailability() == Availability.Available)
                {
                    _ReaderDevice.SetReaderDeviceListener(this);
                    _ReaderDevice.EnableImage(true);
                    _ReaderDevice.Connect(this);
                }
            }
            catch(Exception ex)
            {

            }
            return this.IsConnected;
        }

        public void Disconnect()
        {
            if (this.IsConnected)
            {
                App.Current.MainPage.DisplayAlert("Scanner Disconnected", "Pull trigger to power on MX-1000", "OK");
                //if (this._ListeningForUsb && this._MxConnectedReceiver != null)
                //{
                //    _Activity.UnregisterReceiver(this._MxConnectedReceiver);
                //    this._MxConnectedReceiver.Dispose();
                //    this._ListeningForUsb = false;
                //}
                _ReaderDevice.Disconnect();
            }
        }

        public void Trigger()
        {
            //this._ReaderDevice.StartScanning();
        }

        public void OnAvailabilityChanged(ReaderDevice p0)
        {
            if(_ReaderDevice.GetAvailability() == Availability.Available)
            {
                this.TryConnect();
            }
            else
            {
                this.Disconnect();
            }
        }

        public void OnConnectionCompleted(ReaderDevice p0, Throwable p1)
        {
            if(p1 != null)
            {
                MessagingCenter.Send<Page, Throwable>(App.Current.MainPage, "Exception", p1);
            }
        }

        public void OnConnectionStateChanged(ReaderDevice p0)
        {
            if(p0.ConnectionState == ConnectionState.Connected)
            {
                _ReaderDevice = p0;
                this.TryConnect();
                //this.IsConnected = true;
            }
            else if(p0.ConnectionState == ConnectionState.Disconnected)
            {
                this.Disconnect();
                //this.IsConnected = false;
            }
        }

        public void OnReadResultReceived(ReaderDevice p0, ReadResults p1)
        {
            try
            {
                if (p1.Count > 0)
                {
                    var result = p1.GetResultAt(0);

                    if (result.IsGoodRead)
                    {
                        MessagingCenter.Send<Page, string>(App.Current.MainPage, "GoodRead", result.ReadString);
                        MessagingCenter.Send<Page, MemoryStream>(App.Current.MainPage, "ImageReceived", this.ConvertBitmapToStream(result.Image));
                    }
                }
            }
            catch(Exception ex)
            {
                MessagingCenter.Send<Page, Throwable>(App.Current.MainPage, "Exception", ex);
            }
        }
        private MemoryStream ConvertBitmapToStream(Bitmap image)
        {
            MemoryStream stream = new MemoryStream();
            image.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            return stream; 
        }
    }
}