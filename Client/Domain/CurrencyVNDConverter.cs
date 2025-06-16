using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Domain
{
    public class CurrencyVNDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (decimal.TryParse(value.ToString(), out decimal number))
            {
                // Làm tròn, phân tách hàng nghìn, không hi?n th? ph?n th?p phân, thêm "?"
                return string.Format("{0:N0} đ", Math.Round(number, 0, MidpointRounding.AwayFromZero));
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            if (decimal.TryParse(value.ToString().Replace(" đ", "").Replace(",", ""), out decimal number))
            {
                return number;
            }
            return 0; // Hoặc throw exception nếu cần thiết
        }
    }
}
