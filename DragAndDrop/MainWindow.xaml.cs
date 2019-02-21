using Shared;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DragAndDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ListBox _sourceList;
        private readonly DragAndDropVm _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new DragAndDropVm();
            DataContext = _vm;
        }

        private void ListOne_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Save the source list based on which list was clicked
            var sourceList = sender as ListBox;

            // Traverse the Visual Tree to get the data object bound to the list item that was clicked
            var data = GetDataFromListBoxPoint(sourceList, e.GetPosition(sourceList));

            // If we found some data...
            if (data != null)
            {
                // ...start a Drag and Drop move operation
                DragDrop.DoDragDrop(sourceList, data, DragDropEffects.Move);
            }
        }

        private void ListTwo_DragOver(object sender, DragEventArgs e)
        {
            // Unpack the data
            var data = e.Data.GetData(typeof(DragWrapper)) as DragWrapper;

            // If the VM cannot handle the data being dragged...
            if (!_vm.MoveFromListOneToListTwoCommand.CanExecute(data.Data))
            {
                // ...indicate that it cannot be dropped here.
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void ListTwo_Drop(object sender, DragEventArgs e)
        {
            // Unpack the data
            var data = e.Data.GetData(typeof(DragWrapper)) as DragWrapper;

            // Invoke the drop command to allow the VM to handle the move
            if (_vm.MoveFromListOneToListTwoCommand != null && _vm.MoveFromListOneToListTwoCommand.CanExecute(data.Data))
            {
                _vm.MoveFromListOneToListTwoCommand.Execute(data.Data);
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

            return data != DependencyProperty.UnsetValue ? new DragWrapper(data) : null;
        }

        private class DragWrapper
        {
            public DragWrapper(object data)
            {
                Data = data;
            }

            public object Data { get; }
        }
    }
}
