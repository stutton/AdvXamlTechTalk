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

            _vm = new DragAndDropVm();
            DataContext = _vm;
        }

        private void List_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _sourceList = sender as ListBox;
            var data = GetDataFromListBoxPoint(_sourceList, e.GetPosition(_sourceList));
            if (data != null)
            {
                DragDrop.DoDragDrop(_sourceList, data, DragDropEffects.Move);
            }
        }

        private void DropList_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(DragableData)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DragableData)))
            {
                var data = e.Data.GetData(typeof(DragableData)) as DragableData;
                var targetList = sender as ListBox;
                ((IList)_sourceList.ItemsSource).Remove(data);
                ((IList)targetList.ItemsSource).Add(data);
                _sourceList = null;
            }
        }

        private static object GetDataFromListBoxPoint(ListBox source, Point point)
        {
            if (!(source.InputHitTest(point) is UIElement element))
            {
                return null;
            }

            object data = DependencyProperty.UnsetValue;
            while(data == DependencyProperty.UnsetValue)
            {
                data = source.ItemContainerGenerator.ItemFromContainer(element);
                if(data == DependencyProperty.UnsetValue)
                {
                    element = VisualTreeHelper.GetParent(element) as UIElement;
                }

                if (element.Equals(source))
                {
                    return null;
                }
            }

            return data != DependencyProperty.UnsetValue ? data : null;
        }
    }
}
