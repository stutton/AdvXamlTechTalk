using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AttachedProperties
{
    // This example is from https://wpfplayground.wordpress.com/2014/12/16/attached-properties-vs-behaviors/
    public static class FocusOnLoad
    {
        private static readonly DependencyProperty CanFocusOnLoadProperty =
            DependencyProperty.RegisterAttached(
                "CanFocusOnLoad", 
                typeof(bool), 
                typeof(FocusOnLoad), 
                new PropertyMetadata(FocusOnLoadChanged));

        public static bool GetCanFocusOnLoad(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanFocusOnLoadProperty);
        }

        public static void SetCanFocusOnLoad(DependencyObject obj, bool value)
        {
            obj.SetValue(CanFocusOnLoadProperty, value);
        }

        public static void FocusOnLoadChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if(obj is FrameworkElement element)
            {
                element.Loaded += (s, e) => element.Focus();
            }
        }
    }
}
