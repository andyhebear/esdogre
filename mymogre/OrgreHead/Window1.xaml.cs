using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Mogre;

using OgreLib;

namespace OgreHead
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            App.Current.Exit += Current_Exit;

            InitializeComponent();
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            RenterTargetControl.Source = null;

            _ogreImage.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ogreImage.InitOgreAsync();
        }

        private void RenterTargetControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_ogreImage == null) return;

            _ogreImage.ViewportSize = e.NewSize;
        }

        void _image_InitScene(object sender, RoutedEventArgs e)
        {
 	    // start the scene fade in animation
            var sb = (Storyboard)this.Resources["StartEngine"];
            sb.Begin(this);
            var sceneMgr = _ogreImage.SceneManager;
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
            var camera = _ogreImage.Camera;
            camera.NearClipDistance = 4F;
            camera.FarClipDistance = 4000F;

            ViewPoint vp = _ogreImage.SceneManager.GetSuggestedViewpoint(true);

            camera.Position = vp.Position;
            camera.Pitch(new Degree(90F));
            camera.Rotate(vp.Orientation);
            camera.SetFixedYawAxis(true, Vector3.UNIT_Z);
            _ogreImage.SceneManager.AmbientLight = new ColourValue(1f, 1f, 1f);

            // Create a skydome
           // _ogreImage.SceneManager.SetSkyDome(true, "Examples/CloudySky", 5, 8);
        }

        void _image_PreRender(object sender, System.EventArgs e)
        {
	   
        }

     
        private void OgreImage_ResourceLoadItemProgress(object sender, ResourceLoadEventArgs e)
        {
            _progressName.Text = e.Name;

            // scale the progress bar
            _progressScale.ScaleX = e.Progress;
        }

        private void StartEngine_Completed(object sender, EventArgs e)
        {
           
            _loading.Visibility = Visibility.Collapsed;
        }
        private String quakePk3;
        private String quakeLevel;
        private void _ogreImage_LoadResourceEvent()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load("bsp1.cfg", "\t:=", true);
            quakePk3 = cf.GetSetting("Pak0Location");
            quakeLevel = cf.GetSetting("Map");

            _ogreImage.LoadResource();
            ResourceGroupManager.Singleton.AddResourceLocation(quakePk3, "Zip", ResourceGroupManager.Singleton.WorldResourceGroupName, true);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        { // Move about 100 units per second,
            float moveScale = 300 * 0.04f;

            Vector3 translateVector = Vector3.ZERO;
            switch (e.Key)
            {
                case System.Windows.Input.Key.A:
                    translateVector.x =- moveScale;
                    break;
                case System.Windows.Input.Key.D:
                    translateVector.x = moveScale;
                    break;
                case System.Windows.Input.Key.W:
                    translateVector.z = -moveScale;
                    break;
                case System.Windows.Input.Key.S:
                    translateVector.z = moveScale;
                    break;
            }
            var camera = _ogreImage.Camera;
            // move the camera based on the accumulated movement vector
            camera.MoveRelative(translateVector);
        }
        bool mousestate = false;
        Point prepoint;
        private void RenterTargetControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mousestate = true;
            var point = e.GetPosition(this);
           
        }

        private void RenterTargetControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mousestate = false;
        }

        private void RenterTargetControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Vector3 translateVector = Vector3.ZERO;
            var ept = e.GetPosition(this);
            float relx, rely;
            relx = (float)(ept.X - prepoint.X);
            rely = (float)(ept.Y - prepoint.Y);
            if (mousestate)
            {
                Degree cameraYaw = -relx * .13f;
                Degree cameraPitch = -rely * .13f;
                var camera = _ogreImage.Camera;
                camera.Yaw(cameraYaw);
                camera.Pitch(cameraPitch);


            }
            prepoint = ept;
        }
    }
}
