using DUIDemo.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DUIDemo.UerControls
{
    public enum EMode
    {
        MInt = 0,
        MDecimal = 1
    }

    /// <summary>
    /// NumericUpDown.xaml 的交互逻辑
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// 紀錄原始的值，不能變更，不然設定Value及DecimalPlaces屬性的先後順序會對最後取值的結果有影響。
        /// </summary>
        Decimal _dNumericValue;
        int _nDecimalPlaces;


        public NumericUpDown()
        {
            InitializeComponent();

            _dNumericValue = 0M;
            _nDecimalPlaces = 1;
            Increment = 1.0M;
            MinValue = 0.0M;
        }

        public delegate void IValueChanged();
        public event IValueChanged ValueChanged;

        public delegate bool IValueBeforeChanged(decimal NewValue);
        public event IValueBeforeChanged ValueBeforeChanged;


        public Decimal Value
        {
            get
            {
                return Decimal.Round(_dNumericValue, _nDecimalPlaces);
            }
            set
            {
                if (ValueBeforeChanged != null)
                {
                    if (ValueBeforeChanged(value) != true)
                    {
                        return;
                    }
                }
                _dNumericValue = value;
                NotifyPropertyChanged("Value");
                if (Mode == EMode.MInt)
                {
                    tbValue.Text = String.Format("{0:0}", Decimal.Round(_dNumericValue, _nDecimalPlaces));
                }
                else if (Mode == EMode.MDecimal)
                {
                    tbValue.Text = String.Format("{0:0.0}", Decimal.Round(_dNumericValue, _nDecimalPlaces));
                }

                if (ValueChanged != null)
                    ValueChanged();
            }
        }

        #region DependencyProperty(依赖属性)

        /// <summary>
        /// value值
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NumericUpDown)
            , new PropertyMetadata("0", OnTextChanged, TextCoerceValue));

        private static void OnTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as NumericUpDown;
            if (element != null)
            {
                element.tbValue.Text = e.NewValue.ToString();
            }
        }

        private static object TextCoerceValue(DependencyObject obj, object value)
        {
            var element = obj as NumericUpDown;
            if (element != null)
            {
                //如果是默认值则赋值给text
                if (String.IsNullOrEmpty(element.tbValue.Text))
                {
                    element.tbValue.Text = value.ToString();
                }
            }
            return value;
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                tbValue.Text = value.ToString();
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
    DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(Int32.MaxValue));

        /// <summary>
        /// 設定或取得最大容許值。
        /// </summary>
        public int MaxValue
        {
            get
            {
                return (int)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        /// <summary>
        /// 背景色
        /// </summary>
        public static readonly DependencyProperty BackColorProperty =
            DependencyProperty.Register("BackColor", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(Brushes.White, OnBackColorChanged));

        public Brush BackColor
        {
            get
            {
                return (Brush)GetValue(BackColorProperty);
            }
            set
            {
                tbValue.Background = BackColor;
                SetValue(BackColorProperty, value);
            }
        }

        private static void OnBackColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as NumericUpDown;
            if (element != null)
            {
                element.tbValue.Background = (Brush)e.NewValue;
            }
        }

        public static readonly DependencyProperty ModeProperty =
    DependencyProperty.Register("Mode", typeof(EMode), typeof(NumericUpDown), new PropertyMetadata(EMode.MInt, OnModeChanged));

        public EMode Mode
        {
            get
            {
                return (EMode)GetValue(ModeProperty);
            }
            set
            {
                SetValue(ModeProperty, value);
            }
        }

        private static void OnModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as NumericUpDown;
            if (element != null)
            {
                element.Mode = (EMode)Enum.Parse(typeof(EMode), e.NewValue.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 設定或取得增量值。
        /// </summary>
        public Decimal Increment { get; set; }

        /// <summary>
        /// 設定或取得最小容許值。
        /// </summary>
        public Decimal MinValue { get; set; }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            Decimal newValue = (Value + Increment);
            if (newValue > MaxValue)
            {
                Value = MaxValue;
            }
            else
            {
                Value = newValue;
            }

            // Text = (int)Value; 由于设置了value的值触发Textchanged的事件对Text赋值了

        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            Decimal newValue = (Value - Increment);
            if (newValue < MinValue)
            {
                Value = MinValue;
            }
            else
            {
                Value = newValue;
            }
            // Text = (int)Value; 由于设置了value的值触发Textchanged的事件对Text赋值了
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void tbValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbValue.Text))
            {
                this.Value = PublicFunction.decimalParse(tbValue.Text);

                if (Value > MaxValue) Value = MaxValue;
                if (Value < MinValue) Value = MinValue;

                Text = this.Value.ToString();
            }
        }

        /// <summary>
        /// input test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !RegexHelper.IsTextAllowedPositiveInt(e.Text);
        }
    }//class ucNumericUpDown
}
