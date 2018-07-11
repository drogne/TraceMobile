using PassFailSample.Utilities;

namespace PassFailSample.Models
{
    public class Session : NotifyPropertyChanged
    {
        private string _OrderNumber;

        public string OrderNumber
        {
            get => this._OrderNumber;
            set => this.SetProperty(ref this._OrderNumber, value);
        }
    }
}