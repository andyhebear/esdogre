using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace MyOgreBsp
{
    class BspApplication :Example
    {
        private String quakePk3;
        private String quakeLevel;

        private LoadingBar loadingBar = new LoadingBar();
        private string bspname;
        public BspApplication(string bspname)
        {
            this.bspname = bspname;
        }

        public override void LoadResources()
        {
            loadingBar.Start(window, 1, 1, 0.75F);

            // Turn off rendering of everything except overlays
            sceneMgr.ClearSpecialCaseRenderQueues();
            sceneMgr.AddSpecialCaseRenderQueue((byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY); //fix: RenderQueueGroupID.RENDER_QUEUE_OVERLAY
            sceneMgr.SetSpecialCaseRenderQueueMode(SceneManager.SpecialCaseRenderQueueMode.SCRQM_INCLUDE);

            // Set up the world geometry link
            ResourceGroupManager.Singleton.LinkWorldGeometryToResourceGroup(ResourceGroupManager.Singleton.WorldResourceGroupName, quakeLevel, sceneMgr);

            // Initialise the rest of the resource groups, parse scripts etc
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
            ResourceGroupManager.Singleton.LoadResourceGroup(ResourceGroupManager.Singleton.WorldResourceGroupName, false, true);

            // Back to full rendering
            sceneMgr.ClearSpecialCaseRenderQueues();
            sceneMgr.SetSpecialCaseRenderQueueMode(SceneManager.SpecialCaseRenderQueueMode.SCRQM_EXCLUDE);

            loadingBar.Finish();
        }

        // Override resource sources (include Quake3 archives)
        public override void SetupResources()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load(bspname, "\t:=", true);
            quakePk3 = cf.GetSetting("Pak0Location");
            quakeLevel = cf.GetSetting("Map");

            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation(quakePk3, "Zip", ResourceGroupManager.Singleton.WorldResourceGroupName, true);
        }

        // Override scene manager (use indoor instead of generic)
        public override void ChooseSceneManager()
        {
            sceneMgr = Root.Singleton.CreateSceneManager("BspSceneManager");
        }

        // Scene creation
        public override void CreateScene()
        {
            camera.NearClipDistance = 4F;
            camera.FarClipDistance = 4000F;

            ViewPoint vp = sceneMgr.GetSuggestedViewpoint(true);

            camera.Position = vp.Position;
            camera.Pitch(new Degree(90F));
            camera.Rotate(vp.Orientation);
            camera.SetFixedYawAxis(true, Vector3.UNIT_Z);
        }
    }
}
