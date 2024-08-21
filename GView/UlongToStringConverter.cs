using System.Globalization;
using System.Windows.Data;

namespace GView;

/* Used
 * https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-convert-bound-data?view=netframeworkdesktop-4.8
 */
[ValueConversion(typeof(ulong), typeof(string))]
public class UlongToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ulong) 
            return value;
        
        return ((ulong) value == 0 
            ? string.Empty 
            : value.ToString()) 
               ?? string.Empty;    
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string || ReferenceEquals(value, string.Empty)) 
            return value;
        
        string? str = value.ToString();
        return str != null ? ulong.Parse(str) : value;
    }
}