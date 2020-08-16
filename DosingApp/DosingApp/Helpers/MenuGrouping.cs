using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Helpers
{
    public class MenuGrouping<K, MenuItemViewModel> : ObservableCollection<MenuItemViewModel>
    {
        public K Name { get; private set; }
        public MenuGrouping(K name, IEnumerable<MenuItemViewModel> items)
        {
            Name = name;
            foreach (MenuItemViewModel item in items)
                Items.Add(item);
        }
    }
}
