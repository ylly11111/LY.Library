using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Castle.DynamicProxy;


namespace LY.MEF.AOP
{
    public class AOPExportProvider : ExportProvider, IDisposable
    {

        private CatalogExportProvider _exportProvider;

        public AOPExportProvider(Func<ComposablePartCatalog> catalogResolver)
        {
            _exportProvider = new CatalogExportProvider(catalogResolver());
        }

        public ExportProvider SourceProvider
        {
            get
            {
                return _exportProvider.SourceProvider;
            }
            set
            {
                _exportProvider.SourceProvider = value;
            }
        }

      
        protected override IEnumerable<Export> GetExportsCore(
            ImportDefinition definition, AtomicComposition atomicComposition)
        {
            IEnumerable<Export> exports = _exportProvider.GetExports(definition, atomicComposition);
            return exports.Select(export => new Export(export.Definition, () => GetValue(export)));
        }

        private object GetValue(Export innerExport)
        {
            object value = innerExport.Value;

            Type t = value.GetType();
            IInterceptor[] attribs = t.GetCustomAttributes(typeof(IInterceptor), true).Cast<IInterceptor>().ToArray();

            ProxyGenerator generator = new ProxyGenerator();
            object proxy = generator.CreateClassProxy(value.GetType(), attribs);
            return proxy;
        }


        public void Dispose()
        {
            _exportProvider.Dispose();
        }
    }
}
