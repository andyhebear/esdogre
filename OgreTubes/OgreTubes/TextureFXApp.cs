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

            sceneMgr.SetSkyBox(true, "Examples/CloudyNoonSkyBox");

            SceneNode pNode = sceneMgr.RootSceneNode.CreateChildSceneNode();

            SeriesOfTubes mTubes = new SeriesOfTubes(sceneMgr, 16, 10.0, 12, 12, 12.0,0.0);

           // SeriesOfTubes* mTubes = new SeriesOfTubes(mSceneMgr, 16, 10.0, 12, 12, 12.0);

            mTubes.addPoint(new Vector3(0, 0, 0));
            mTubes.addPoint(new Vector3(100, 0, 200));
            mTubes.addPoint(new Vector3(0, 200, 400));
            mTubes.addPoint(new Vector3(50, 340, 300));
            mTubes.addPoint(new Vector3(500, 340, 200));
            mTubes.addPoint(new Vector3(400, 100, 100));
            mTubes.addPoint(new Vector3(50, -20, 10));
            mTubes.addPoint(new Vector3(0, -100, -300));

            mTubes.setSceneNode(pNode);
            var tt = mTubes.createTubes("MyTubes", "OceanHLSL_GLSL", false, false, false, false);
          
        }
    }
}
