using LY.WPF.Command;
using LY.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.WPF.Test
{
   public class VM:ViewModelBase
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged("Name"); }
        }

        private DelegateCommand _Cmd;

        public DelegateCommand Cmd
        {
            get { return _Cmd; }
            set { _Cmd = value; RaisePropertyChanged("Cmd"); }
        }

        public VM()
        {
            Cmd = new DelegateCommand(ExecuteCmd);
        }

        private void ExecuteCmd()
        {
            Name = DateTime.Now.ToString();
        }
    }
}
