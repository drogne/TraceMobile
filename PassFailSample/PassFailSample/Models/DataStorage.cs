using PassFailSample.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PassFailSample.Models
{
    public class DataStorage
    {
        #region Fields

        private SemaphoreSlim waiter = new SemaphoreSlim(1);

        #endregion

        #region Properties

        public List<string> ListTextAdders { get; private set; }
        public string StatusReason { get; private set; }
        public string Status { get; private set; }
        public string Barcode { get; private set; }
        public string UserID { get; private set; }
        private DateTime _ScanTime { get; set; }
        public Settings Settings { get; private set; }
        
        #endregion

        #region Constructor and Init/Deinit
        public DataStorage(Settings settings)
        {
            this.Barcode = string.Empty;
            this.UserID = string.Empty;
            this.Settings = settings;
        }
        
        // TODO: Do we need this? maybe create a new csv file for each new instance of this class? This doesn't get called anywhere, call on app startup after autofac init?
        public bool Initialize()
        {
            return DependencyService.Get<ISaveAndLoad>().Initialize();
        }

        // TODO: Do we need this? Close all references when app closes
        public bool Deinitialize()
        {
            return true;
        }
        #endregion

        #region Methods

        public void SetUserID(string user)
        {
            waiter.Wait();
            this.UserID = user;
            waiter.Release();
        }
        public void SetBarcode(string barcode)
        {
            waiter.Wait();
            this.Barcode = barcode;
            this._ScanTime = DateTime.Now;
            waiter.Release();
        }
        public void SetStatus(string status)
        {
            waiter.Wait();
            this.Status = status;
            waiter.Release();
        }
        public void SetStatusReason(string statusReason)
        {
            waiter.Wait();
            this.StatusReason = statusReason;
            waiter.Release();
        }
        public void SetTextField(string text)
        {
            waiter.Wait();
            this.ListTextAdders.Add(text);
            waiter.Release();
        }
        private void ClearStoredBarcodeData()
        {
            this.Barcode = string.Empty;
            this.Status = string.Empty;
            this.StatusReason = string.Empty;
            //this.ListTextAdders.Clear(); // TODO add this back in
        }

        public async Task<bool> RecordBarcode()
        {
            await waiter.WaitAsync();
            {
                var time = this._ScanTime;
                if (this.Settings.BoolUseLoggedEntryTime)
                {
                    time = DateTime.Now;
                }
                // TODO: add in the extra text fields
                if (DependencyService.Get<ISaveAndLoad>().AppendText($"{this.UserID}, {this.Barcode}, {time}, {this.Status}, {this.StatusReason},\r\n"))
                {
                    this.ClearStoredBarcodeData();
                    waiter.Release();
                    return true;
                }
                waiter.Release();
                return false;
            }
        }

        #endregion
    }
}