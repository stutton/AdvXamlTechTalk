using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DragAndDropBehavior
{
    public class DragBehavior : Behavior<ItemsControl>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var list = (ItemsControl)sender;

            var data = GetDataFromListBoxPoint(list, e.GetPosition(list));

            if(data != null)
            {
                DragDrop.DoDragDrop(list, data, DragDropEffects.Move);
            }
        }

        private static object GetDataFromListBoxPoint(ItemsControl source, Point point)
        {
            // Get the last item in the Visual Tree at the specified point
            if (!(source.InputHitTest(point) is UIElement element))
            {
                return null;
            }

            // Traverse up the Visual Tree until we find an element with "data"
            object data = DependencyProperty.UnsetValue;
            while (data == DependencyProperty.UnsetValue)
            {
                // try to get any data associated with the current element
                data = source.ItemContainerGenerator.ItemFromContainer(element);

                // If this element doesn't have any data...
                if (data == DependencyProperty.UnsetValue)
                {
                    // ...get this elements parent
                    element = VisualTreeHelper.GetParent(element) as UIElement;
                }

                // If our current element is the ListBox we have search the relevant part
                // of the Visual Tree and didn't find any data
                if (element.Equals(source))
                {
                    return null;
                }
            }

            return data != DependencyProperty.UnsetValue ? data : null;
        }
    }
}
