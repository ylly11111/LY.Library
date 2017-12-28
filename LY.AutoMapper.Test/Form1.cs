using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LY.AutoMapper;

namespace LY.AutoMapper.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StudentEntity stuEntity = new StudentEntity(){Name="ly",Age=29};

          StudentModel stuModel=  stuEntity.MapTo<StudentModel>();

          List<StudentEntity> strEntityList = new List<StudentEntity>()
              {
                  new StudentEntity(){Name="aa",Age=15}
              };

          List<StudentModel> stuModelList = strEntityList.MapTo<StudentModel>();
        }
    }
}
