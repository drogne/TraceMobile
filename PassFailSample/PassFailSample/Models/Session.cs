using PassFailSample.Utilities;

namespace PassFailSample.Models
{
    public class Session : NotifyPropertyChanged
    {
        private string _BadgeNumber;
        private string _OrderNumber;
        private string _ComponentNumber;
        private string _Result;
        private string _ResultDescription;

        public string BadgeNumber
        {
            get => this._BadgeNumber;
            set => this.SetProperty(ref this._BadgeNumber, value);
        }

        public string OrderNumber
        {
            get => this._OrderNumber;
            set => this.SetProperty(ref this._OrderNumber, value);
        }

        public string ComponentNumber
        {
            get => this._ComponentNumber;
            set => this.SetProperty(ref this._ComponentNumber, value);
        }

        public string Result
        {
            get => this._Result;
            set => this.SetProperty(ref this._Result, value);
        }

        public string ResultDescription
        {
            get => this._ResultDescription;
            set => this.SetProperty(ref this._ResultDescription, value);
        }
    }
}