using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel.Composition;
using System.IO;

namespace LY.MEF.AOP
{
   public class MefBase
    {
       public static void ComposeParts(object o)
       {
           ComposeParts(o,"");
       }

        public static  void ComposeParts(object o,string PluginPath)
        {
            Func<ComposablePartCatalog> catalogResolver = () =>
            {
                var catalog = new AggregateCatalog();
                AssemblyCatalog assemblyCatalog = new AssemblyCatalog(Assembly.GetEntryAssembly());
                if (Directory.Exists(PluginPath))
                {
                    DirectoryCatalog directoryCatalog = new DirectoryCatalog(PluginPath);
                    catalog.Catalogs.Add(directoryCatalog);
                }
               
                catalog.Catalogs.Add(assemblyCatalog);
               
                return catalog;
            };

            AOPExportProvider provider = new AOPExportProvider(catalogResolver);
            CompositionContainer container = new CompositionContainer(provider);
            provider.SourceProvider = container;
            container.ComposeParts(o);
        }
    }
}
