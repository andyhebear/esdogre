using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Windows.Forms;

namespace MyOgreBsp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string bspname=Application.StartupPath+ "\\bsp1.cfg";
                if (args.Length > 0)
                    bspname = args[0];
                BspApplication app = new BspApplication(bspname);
                app.Go();
            }
            catch (System.Runtime.InteropServices.SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    Example.ShowOgreException();
                else
                    throw;
            }
        }
    }
}
