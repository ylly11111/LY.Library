using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.MEF.Test
{
    [ExportMetadata("Key","1")]
    [Export(typeof(ClassA))]
    public class ClassA
    {       
       public string A { get; set; }
    }
}
