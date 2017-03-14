using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace LY.MEF
{
    public class MefBase<T>
    {
        [ImportMany]
        List<Lazy<T, IMefMetaData>> _PluginList=new List<Lazy<T,IMefMetaData>> ();

        private string _PluginPath = AppDomain.CurrentDomain.BaseDirectory + "\\Addin";

        public MefBase()
        {
            ComposeParts();
        }

        public MefBase(string path)
        {
            _PluginPath = path;
            ComposeParts();
        }

        public List<T> GetPlugin(string key)
        {
           return _PluginList.Where(p=>p.Metadata.Key==key).Select(a=>a.Value).ToList();
        }

        private void ComposeParts()
        {
            //AggregateCatalog用来合并多个catalog
            var catalog = new AggregateCatalog();

            ///使用当前程序集构造AssemblyCatalog,并添加
            ///AssemblyCatalog对于
            //AssemblyCatalog assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            AssemblyCatalog assemblyCatalog = new AssemblyCatalog(Assembly.GetEntryAssembly());
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(_PluginPath);
            catalog.Catalogs.Add(assemblyCatalog);
            //catalog.Catalogs.Add(directoryCatalog);
            ///初始化CompositionContainer
            var container = new CompositionContainer(catalog);

            ///要组合的特性化对象的数组

            container.ComposeParts(this);
        }

    }

    public interface IMefMetaData
    {
         string Key { get;  }
    }
}
