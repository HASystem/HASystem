using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace HASystem.Desktop.Assembler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var catalog = new AggregateCatalog();

            Type type = typeof(App);
            catalog.Catalogs.Add(new AssemblyCatalog(type.Assembly));

            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
                container.ComposeExportedValue("container", container);

                Lazy<IModuleController> controller = container.GetExport<IModuleController>();
                if (controller == null) return;

                controller.Value.CompositionContainer = container;
                controller.Value.Run();
            }
            catch (CompositionException exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
    }
}
