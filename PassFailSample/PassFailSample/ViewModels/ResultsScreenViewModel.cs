using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PassFailSample.Helpers;
using PassFailSample.Models;
using PassFailSample.TraceModis;
using Xamarin.Forms;

namespace PassFailSample.ViewModels
{
    public class ResultsScreenViewModel : BaseViewModel
    {
        #region Properties
        public Session Session { get; private set; }
        private IBarcodeScanner Scanner { get; set; }
        public Color ColorBackground { get; set; } = Color.FromHex("#D8DCD6");

        public List<string> PickerSource { get; set; } = new List<string>() { "<Empty>" };

        public GridLength OwerwriteHeight { get; set; } = new GridLength(0, GridUnitType.Auto);

        public GridLength OptionsHeight { get; set; } = new GridLength(0, GridUnitType.Star);

        public ICommand OwerWriteYesCommand { get; private set; }

        public ICommand OwerWriteNoCommand { get; private set; }

        public ICommand OptionsCommand { get; private set; }

        public ICommand NewScanCommand { get; private set; }

        public ICommand PickerSelectedChanged { get; private set; }

        public string SelectedOptionValue { get; set; }

        string selectedOption;
        public string SelectedOption
        {
            get
            {
                return selectedOption;
            }
            set
            {
                if(selectedOption != value)
                {
                    selectedOption = value;
                    SelectedOptionValue = selectedOption;
                }
            }
        }

        #endregion

        #region Constructor and Init/Deinit

        public ResultsScreenViewModel(IdleTimeoutTimer timer, Settings Settings, Session session) : base(timer, Settings)
        {
            this.Session = session;
            this.Scanner = DependencyService.Get<IBarcodeScanner>();
            this.OwerWriteYesCommand = new Command(() => this.OwerWriteYesMethod());
            this.OwerWriteNoCommand = new Command(() => this.OwerWriteNoMethod());
            this.OptionsCommand = new Command(() => this.OptionsMethod());
            this.NewScanCommand = new Command(() => this.NewScanMethod());
        }

        public override void Initialize()
        {
            RefreshProperties();
            base.Initialize();
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
        }

        #endregion

        #region Methods

        private void Client_ProcessBarCodeCompleted(object sender, ProcessBarCodeCompletedEventArgs e)
        {
            ObservableCollection<string> vals = e.Result;

            this.Session.Result = vals[0].ToString();
            this.Session.ResultDescription = vals[1].ToString();
            RefreshProperties();
        }

        private void OwerWriteYesMethod()
        {
            ObservableCollection<string> Pars = new ObservableCollection<string>() { this.Session.Result, "", "Test-Cognex", "" };
            Service1Client client = new Service1Client();
            client.ProcessBarCodeAsync(this.Session.OrderNumber, this.Session.ComponentNumber, Pars);
            client.ProcessBarCodeCompleted += Client_ProcessBarCodeCompleted;

            //RefreshProperties();
        }

        private void OwerWriteNoMethod()
        {
            this.Session.Result = "Error";
            this.Session.ResultDescription = "Duplicate Values. User Selected not to proceed";
            RefreshProperties();
        }

        private void OptionsMethod()
        {
            ObservableCollection<string> Pars = new ObservableCollection<string>() { this.Session.Result, SelectedOptionValue, "Test-Cognex", "" };
            Service1Client client = new Service1Client();
            client.ProcessBarCodeAsync(this.Session.OrderNumber, this.Session.ComponentNumber, Pars);
            client.ProcessBarCodeCompleted += Client_ProcessBarCodeCompleted;

            //RefreshProperties();
        }

        private void NewScanMethod()
        {
            this.Session.OrderNumber = string.Empty;
            this.Session.ComponentNumber = string.Empty;
            this.Session.Result = string.Empty;
            this.Session.ResultDescription = string.Empty;
            this.NavService.NavigateTo(EnabledScreens.GetSpecificScreen("PassFailSample.ViewModels.OrderViewModel"));
        }

        private void RefreshProperties()
        {
            if (!string.IsNullOrEmpty(this.Session.Result))
            {
                OwerwriteHeight = new GridLength(0, GridUnitType.Star);
                OptionsHeight = new GridLength(0, GridUnitType.Star);

                if (this.Session.Result.StartsWith("Success"))
                {
                    this.ColorBackground = Color.FromHex("#C6EFCD");
                }
                else if (this.Session.Result.StartsWith("Overwrite") || this.Session.Result.StartsWith("Options"))
                {
                    this.ColorBackground = Color.FromHex("#FEEB9C");
                    if (this.Session.Result.StartsWith("Overwrite"))
                    {
                        OwerwriteHeight = new GridLength(1, GridUnitType.Star);
                    }
                    else // Options
                    {
                        OptionsHeight = new GridLength(1, GridUnitType.Star);
                        string[] PickerValues = this.Session.ResultDescription.Split('|');

                        if (PickerValues.Length > 1)
                        {
                            this.PickerSource = new List<string>();

                            for (int i = 1; i < PickerValues.Length; i++)
                            {
                                this.PickerSource.Add(PickerValues[i]);
                            }
                        }
                    }
                }
                else if (this.Session.Result.StartsWith("Error"))
                {
                    this.ColorBackground = Color.FromHex("#FFC8CE");
                }
                else
                {
                    this.ColorBackground = Color.FromHex("#D8DCD6");
                }

                base.FirePropertyChanged("ColorBackground");
                base.FirePropertyChanged("OwerwriteHeight");
                base.FirePropertyChanged("OptionsHeight");
                base.FirePropertyChanged("PickerSource");
            }
        }

        #endregion
    }
}
