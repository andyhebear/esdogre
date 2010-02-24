using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace OgreTubes
{
    class TextureFXApp : Mogre.Demo.ExampleApplication.Example
    {
       
        // Just override the mandatory create scene method
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20, 80, 50);

        }
    }
}
