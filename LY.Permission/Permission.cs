﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LY.Permission
{
    public class Permission
    {
        public virtual Dictionary<string, bool> GetPermissions(string moduleName)
        {
            Dictionary<string, bool> permissions = new Dictionary<string, bool>();
            permissions.Add("button1",false);
            return permissions;
        }

        public void SetPermission(Form form)
        {
            Type t = form.GetType();
            FieldInfo[] fs = t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            IDictionary<string, bool> permissions = GetPermissions(form.Name);

            foreach (FieldInfo f in fs)
            {
                dynamic component = f.GetValue(form);
                if (component is System.ComponentModel.Component && permissions.ContainsKey(f.Name))
                {
                    try
                    {
                        component.Enabled = permissions[f.Name];
                    }
                    catch { }
                }
            }
        }
    }
}
