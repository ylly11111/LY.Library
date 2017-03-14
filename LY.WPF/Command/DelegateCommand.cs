using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LY.WPF.Command
{
    public class DelegateCommand : ICommand
    {
        Action _action;
        Func<bool>  _canAction;
        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public DelegateCommand(Action action, Func<bool> canAction)
        {
            _action = action;
            _canAction = canAction;
        }

        public bool CanExecute(object parameter)
        {
            if (_canAction == null)
            {
                return true;
            }
            return _canAction.Invoke();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                _action.Invoke();
            }
           
        }
    }
}
