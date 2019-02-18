using Microsoft.Xaml.Behaviors;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DragAndDropBehavior
{
    public class DropBehavior : Behavior<ItemsControl>
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register(nameof(DropCommand),
                typeof(ICommand),
                typeof(DropBehavior),
                new PropertyMetadata(default(ICommand)));

        public ICommand DropCommand
        {
            get => (ICommand)GetValue(DropCommandProperty);
            set => SetValue(DropCommandProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.AllowDrop = true;

            AssociatedObject.DragOver += OnDragOver;
            AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.DragOver -= OnDragOver;
            AssociatedObject.Drop -= OnDrop;

            AssociatedObject.AllowDrop = false;
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            // If the data being dragged is NOT the type we expect...
            if (!e.Data.GetDataPresent(typeof(DragableData)))
            {
                // ...indicate that it cannot be dropped here.
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            // If the data is the type we expect...
            if (e.Data.GetDataPresent(typeof(DragableData)))
            {
                // ...unpack the data
                var data = e.Data.GetData(typeof(DragableData)) as DragableData;

                // Invoke the drop command to allow the VM to handle the move
                if(DropCommand != null && DropCommand.CanExecute(data))
                {
                    DropCommand.Execute(data);
                }
            }
        }
    }
}
