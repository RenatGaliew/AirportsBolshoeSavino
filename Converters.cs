using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using ReactiveUI;

namespace Airports
{
    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateConverter : IBindingTypeConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            return DependencyProperty.UnsetValue;
        }

        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            throw new NotImplementedException();
        }

        public bool TryConvert(object @from, Type toType, object conversionHint, out object result)
        {
            DateTime date = (DateTime)@from;
            result = date.ToString("dd.MM.yyyy HH:mm:ss");
            return true;
            //throw new NotImplementedException();
        }
    }

    public static class AnimatableDoubleHelper
    {
        // Это attached property OriginalProperty. К нему мы будем привязывать свойство из VM,
        // и получать нотификацию об его изменении
        public static double GetOriginalProperty(DependencyObject obj) =>
            (double)obj.GetValue(OriginalPropertyProperty);
        public static void SetOriginalProperty(DependencyObject obj, double value) =>
            obj.SetValue(OriginalPropertyProperty, value);
        public static readonly DependencyProperty OriginalPropertyProperty =
            DependencyProperty.RegisterAttached(
                "OriginalProperty", typeof(double), typeof(AnimatableDoubleHelper),
                new PropertyMetadata(OnOriginalUpdated));

        // это "производное" attached property, которое будет
        // анимированно "догонять" OriginalProperty
        public static double GetAnimatedProperty(DependencyObject obj) =>
            (double)obj.GetValue(AnimatedPropertyProperty);
        public static void SetAnimatedProperty(DependencyObject obj, double value) =>
            obj.SetValue(AnimatedPropertyProperty, value);
        public static readonly DependencyProperty AnimatedPropertyProperty =
            DependencyProperty.RegisterAttached(
                "AnimatedProperty", typeof(double), typeof(AnimatableDoubleHelper));

        // это вызывается когда значение OriginalProperty меняется
        static void OnOriginalUpdated(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            // находим элемент, на котором меняется свойство
            FrameworkElement self = (FrameworkElement)o;
            DoubleAnimation animation = // создаём анимацию...
                new DoubleAnimation(newValue, new Duration(TimeSpan.FromSeconds(0.3)));
            // и запускаем её на AnimatedProperty
            self.BeginAnimation(AnimatedPropertyProperty, animation);
        }
    }
}
