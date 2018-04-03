using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationDrawerPopUpMenu2.Class
{
    class MenuTree
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public ObservableCollection<MenuTree> Items { get; set; }
        public MenuTree()
        {
            this.Items = new ObservableCollection<MenuTree>();
        }
    }
}
