using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.MEF
{
    [MetadataAttribute, AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ExportAttributeEx : ExportAttribute
    {
        public ExportAttributeEx(Type t):base(t)
        {
            
        }
    }
}
