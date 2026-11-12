using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MurzayanovChudoObuv
{
    internal class Manager
    {
        public static Frame MainFrame { get; set; }
        public static Users currentUser { get; set; }

        public static TextBlock currentUserTextBlock { get; set; }
    }
}
