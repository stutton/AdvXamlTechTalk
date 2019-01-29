using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragAndDrop
{
    internal class DragAndDropVm : Observable
    {
        public DragAndDropVm()
        {
            SourceList.Add(new DragableData { Name = "Steve" });
            SourceList.Add(new DragableData { Name = "Fred" });
            SourceList.Add(new DragableData { Name = "Sally" });
            SourceList.Add(new DragableData { Name = "Hank" });
            SourceList.Add(new DragableData { Name = "Jane" });

            TargetList.Add(new DragableData { Name = "Zach" });
        }

        public ObservableCollection<DragableData> SourceList { get; } = new ObservableCollection<DragableData>();
        public ObservableCollection<DragableData> TargetList { get; } = new ObservableCollection<DragableData>();
    }
}
