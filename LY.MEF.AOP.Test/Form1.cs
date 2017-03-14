using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LY.MEF.AOP;

namespace LY.MEF.AOP.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MefBase.ComposeParts(this);
        }

        [Import(typeof(ClassA))]
        private ClassA C1;

        private void button1_Click(object sender, EventArgs e)
        {
            C1.GetA();
        }
    }
}
