using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace OgreTubes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TextureFXApp app = new TextureFXApp();
                app.Go();
            }
            catch (System.Runtime.InteropServices.SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    Mogre.Demo.ExampleApplication.Example.ShowOgreException();
                else
                    throw;
            }
        }
    }
}
