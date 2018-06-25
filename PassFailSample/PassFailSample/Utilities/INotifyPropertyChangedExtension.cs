using System.ComponentModel;

namespace PassFailSample.Utilities
{
	/// <summary>
	/// Interface that allows for easy implementation of INotifyPropertyChanged via extension methods
	/// </summary>
	public interface INotifyPropertyChangedExtension : INotifyPropertyChanged
	{

		/// <summary>
		/// Throws the PropertyChanged event
		/// </summary>
		/// <param name="e">PropertyChanged event arguments</param>
		bool FirePropertyChanged(PropertyChangedEventArgs e);

	}
}
