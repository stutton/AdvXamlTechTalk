using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragAndDropBehavior
{
    internal class DragAndDropVm : Observable
    {
        public DragAndDropVm()
        {
            ListOne.Add(new DragableData { Name = "Steve" });
            ListOne.Add(new DragableData { Name = "Fred" });
            ListOne.Add(new DragableData { Name = "Sally" });
            ListOne.Add(new DragableData { Name = "Hank" });
            ListOne.Add(new DragableData { Name = "Jane" });

            ListTwo.Add(new DragableData { Name = "Zach" });
        }

        public ObservableCollection<DragableData> ListOne { get; } = new ObservableCollection<DragableData>();
        public ObservableCollection<DragableData> ListTwo { get; } = new ObservableCollection<DragableData>();


        #region MoveFromListOneToListTwo Command

        private ICommand _moveFromListOneToListTwoCommand;
        public ICommand MoveFromListOneToListTwoCommand => _moveFromListOneToListTwoCommand ??
            (_moveFromListOneToListTwoCommand = new RelayCommand<DragableData>(MoveFromListOneToListTwoAsync));

        private void MoveFromListOneToListTwoAsync(DragableData item)
        {
            ListOne.Remove(item);
            ListTwo.Add(item);
        }

        #endregion


        #region MoveFromListTwoToListOne Command

        private ICommand _moveFromListTwoToListOneCommand;
        public ICommand MoveFromListTwoToListOneCommand => _moveFromListTwoToListOneCommand ??
            (_moveFromListTwoToListOneCommand = new RelayCommand<DragableData>(MoveFromListTwoToListOneAsync));

        private void MoveFromListTwoToListOneAsync(DragableData item)
        {
            ListTwo.Remove(item);
            ListOne.Add(item);
        }

        #endregion
    }
}
