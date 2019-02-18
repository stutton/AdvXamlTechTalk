using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragAndDrop
{
    public partial class SimpleDragDrop : UserControl
    {
        private readonly DragAndDropVm _vm;
        private ListBox _sourceList;

        public SimpleDragDrop()
        {
            InitializeComponent();

            DataContext = new DragAndDropVm();
        }

        private void List_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Save the source list based on which list was clicked
            _sourceList = sender as ListBox;

            // Traverse the Visual Tree to get the data object bound to the list item that was clicked
            var data = GetDataFromListBoxPoint(_sourceList, e.GetPosition(_sourceList));

            // If we found some data...
            if (data != null)
            {
                // ...start a Drag and Drop move operation
                DragDrop.DoDragDrop(_sourceList, data, DragDropEffects.Move);
            }
        }

        private void DropList_DragOver(object sender, DragEventArgs e)
        {
            // If the data being dragged is NOT the type we expect...
            if (!e.Data.GetDataPresent(typeof(DragableData)))
            {
                // ...indicate that it cannot be dropped here.
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            // If the data is the type we expect...
            if (e.Data.GetDataPresent(typeof(DragableData)))
            {
                // ...unpack the data
                var data = e.Data.GetData(typeof(DragableData)) as DragableData;
                
                // Get the ListBox being dropped onto
                var targetList = sender as ListBox;

                // Remove the dragged item from the source list
                ((IList)_sourceList.ItemsSource).Remove(data);

                // Add the dragged item to the target list
                ((IList)targetList.ItemsSource).Add(data);

                _sourceList = null;
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
            while(data == DependencyProperty.UnsetValue)
            {
                // try to get any data associated with the current element
                data = source.ItemContainerGenerator.ItemFromContainer(element);

                // If this element doesn't have any data...
                if(data == DependencyProperty.UnsetValue)
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
