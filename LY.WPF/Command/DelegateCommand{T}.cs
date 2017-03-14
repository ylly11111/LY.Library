using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LY.WPF.Command
{
    public class DelegateCommand<T> :ICommand
    {

        Action<T> _action;
        Func<T, bool> _canAction;
        public DelegateCommand(Action<T> action)
        {
            _action = action;

        }

        public DelegateCommand(Action<T> action, Func<T, bool> canAction)
        {
            _action = action;
            _canAction = canAction;
        }

        public bool CanExecute(object parameter)
        {
            if (_canAction==null)
            {
                return true;
            }
            return _canAction.Invoke((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                _action.Invoke((T)parameter);
            }
           
        }


        public event EventHandler CanExecuteChanged;
    }
}
