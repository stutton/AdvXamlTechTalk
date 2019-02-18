using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Behaviors
{
    // To get access to the Behavior<T> base class:
    // - Add reference to System.Windows.Interactivity (deprecated)
    // - Install the Microsoft.Xaml.Behaviors.Wpf NuGet package and use Microsoft.Xaml.Behaviors
    // See https://blogs.msdn.microsoft.com/dotnet/2018/12/10/open-sourcing-xaml-behaviors-for-wpf/ for more info
    public class FocusOnLoadBehavoir : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Focus();
        }
    }
}
