using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.MEF.Test
{
   [ExportMetadata("Key","1")]
    [ExportAttributeEx(typeof(ClassB))]
    public class ClassB
    {
        private string _B;

        public string B
        {
            get { return _B; }
            set { _B = value; }
        }

    }

   [ExportMetadata("Key", "1")]
   [ExportAttributeEx(typeof(ClassB))]
   public class ClassC:ClassB
   {
      
   }
}
