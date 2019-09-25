using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTesting
{
    [Serializable]
    class Item
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public Item()
        {
        }
    }
}
