using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.Messager
{
    public class Messager
    {
        private static Messager _MessageInstance;
        private Dictionary<string, Action> _MessageCollection = new Dictionary<string, Action>();
        private Dictionary<string, ActionClass> _MessageTCollection = new Dictionary<string, ActionClass>();
        public static Messager Default
        {
            get
            {
                if (_MessageInstance == null)
                {
                    _MessageInstance = new Messager();
                }
                return _MessageInstance;
            }
        }
        public void Register(string key, Action action)
        {
            _MessageCollection.Add(key, action);
        }
        public void Register<T>(string key, Action<T> action)
        {
            ActionClass<T> actionClass = new ActionClass<T>();
            actionClass.action = action;
            _MessageTCollection.Add(key, actionClass);
        }
        public void Send(string key)
        {
            if (_MessageCollection.Keys.Contains(key))
            {
                _MessageCollection[key].Invoke();
            }
        }
        public void Send<T>(string key, T para)
        {
            if (_MessageTCollection.Keys.Contains(key))
            {
                ActionClass<T> actionClass = (ActionClass<T>)_MessageTCollection[key];
                actionClass.action.Invoke(para);
            }
        }
    }
}
