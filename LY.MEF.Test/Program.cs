using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.MEF.Test
{
    class Program
    {

        static void Main(string[] args)
        {
            MefBase<ClassA> tmpMef = new MefBase<ClassA>();
            MefBase<ClassB> tmpMefB = new MefBase<ClassB>();
            List<ClassA> tmpA= tmpMef.GetPlugin("1");
            tmpA[0].A = "";
        }
    }
}
