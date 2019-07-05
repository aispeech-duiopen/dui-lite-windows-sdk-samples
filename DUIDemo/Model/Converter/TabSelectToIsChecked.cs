using DUIDemo.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DUIDemo.Model.Converter
{
    public class TabSelectToIsChecked : IValueConverter
    {
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (PublicFunction.intParse(value) == PublicFunction.intParse(parameter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class ComboBoxSelectToIsChecked : IValueConverter
    {
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (PublicFunction.intParse(value) == PublicFunction.intParse(parameter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class PopupIsOpenConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return PublicFunction.boolParse(values[0]) && PublicFunction.boolParse(values[1]);

            switch (parameter as string)
            {
                case "IsKeyboardFocused":
                    return PublicFunction.boolParse(values[0])&& PublicFunction.boolParse(values[1]);
                case "IsChecked":
                    return values[1] + "," + values[0];
                default:
                    return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
