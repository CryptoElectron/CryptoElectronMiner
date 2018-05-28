using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinerGUI.Gui.Bundles
{
    class Algo
    {
        public String Name { get; }
        public String HashrateSizer { get; }
        public Algo(String name, String hashrateSizer)
        {
            this.Name = name;
            this.HashrateSizer = hashrateSizer;
        }
    }
}
