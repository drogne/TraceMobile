using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PassFailSample.Utilities
{
	/// <summary>
	/// Extention methods for INotifyPropertyChangedExtension 
	/// </summary>
	public static class NpcExtensions
	{
		/// <summary>
		/// Fires the PropertyChanged event for the <paramref name="property"/>.
		/// </summary>
		/// <typeparam name="T">Any object</typeparam>
		/// <param name="sender">The INotifyPropertyChangedExtension implementor</param>
		/// <param name="handler">The PropertyChanged event</param>
		/// <param name="original">Reference to the current value of the <paramref name="property"/></param>
		/// <param name="value">Requested new value of the <paramref name="property"/></param>
		/// <param name="property">The name of the property</param>
		/// <returns>Returns <c>true</c> if the event is fired successfully. Otherwise returns <c>false</c>.</returns>
		public static bool SetProperty<T>(this INotifyPropertyChangedExtension sender, ref T original, T value, [CallerMemberName] string property = null)
		{
			if (NpcExtensions.AreValuesEqual(original, value))
			{
				return false;
			}
			else
			{
				original = value;
				return sender.FirePropertyChanged(new PropertyChangedEventArgs(property));
			}
		}

		/// <summary>
		/// Fires the property changed event for the <paramref name="property"/>.
		/// </summary>
		/// <param name="property">The name of the property being changed.</param>
		/// <returns>Returns <c>true</c> if the event is fired successfully. Otherwise returns <c>false</c>.</returns>
		public static bool FirePropertyChanged(this INotifyPropertyChangedExtension sender, [CallerMemberName] string property = null)
		{
			return sender.FirePropertyChanged(new PropertyChangedEventArgs(property));
		}

		/// <summary>
		/// Check if the two values are equal.
		/// </summary>
		/// <typeparam name="T">Type of value being compared</typeparam>
		/// <param name="original">The original value.</param>
		/// <param name="value">The new value.</param>
		/// <returns>Returns <c>true</c> if the two values are equal and <c>false</c> if they are not.</returns>
		internal static bool AreValuesEqual<T>(T original, T value)
		{
			if (Object.Equals(original, null) && Object.Equals(value, null))
			{
				// Both objects are null
				return true;
			}
			else if (Object.Equals(original, null) || Object.Equals(value, null))
			{
				// One object is null and other is not
				return false;
			}
			else if (Object.Equals(original, value))
			{
				// Objects are same reference
				return true;
			}
			else
			{
				// Value equality
				return original.Equals(value);
			}
		}

	}
}
