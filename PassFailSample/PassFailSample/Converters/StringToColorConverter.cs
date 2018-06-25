using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PassFailSample.Converters
{   // TOOD add attribute
    //[ValueConversion(sourceType: typeof(string), targetType: typeof(Color))]
    public class StringToColorConverter : IValueConverter, IMarkupExtension
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: make sure these are all hex characters
            //return Color.FromHex((value as string) ?? "#CCCCCC");

            string valueAsString;
            try
            {
                valueAsString = value.ToString();
            }
            catch
            {
                valueAsString = "#DDDDDD";
            }
            return Color.FromHex(valueAsString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
