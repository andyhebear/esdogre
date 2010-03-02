using System.Windows;

namespace OgreHead
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string bspname;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string[] commands = e.Args;
            if (commands.Length > 0)
                bspname = commands[0];
            else
                bspname = "bsp1.cfg";
        }
        public string BspName { get { return bspname; } }
    }
}
