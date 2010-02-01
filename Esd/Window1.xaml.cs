using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mogre;
using System.ComponentModel;
using System.Windows.Media.Animation;
using Esd.Tool;

namespace Esd
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// 场景视图变换工具
        /// </summary>
        ViewportTool viewporttool = new ViewportTool();
        public Window1()
        {
            InitializeComponent();
        }
     
        private void _ogre_OnInitialised(object sender, RoutedEventArgs e)
        {
           var   sceneMgr = _ogreImage.SceneManager;
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20, 80, 50);


            // Set up a material for the skydome
            MaterialPtr skyMat = MaterialManager.Singleton.Create("SkyMat",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            // Perform no dynamic lighting on the sky
            skyMat.SetLightingEnabled(false);
            // Use a cloudy sky
            TextureUnitState t = skyMat.GetTechnique(0).GetPass(0).CreateTextureUnitState("clouds.jpg");
            // Scroll the clouds
            t.SetScrollAnimation(0.15f, 0f);

            // System will automatically set no depth write

            // Create a skydome
            sceneMgr.SetSkyDome(true, "SkyMat", -5, 2);
            EsdSceneManager.CreateSceneManager(_ogreImage);
        }
        private void RenterTargetControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_ogreImage == null) return;

            _ogreImage.ViewportSize = e.NewSize;
        }
        void _image_PreRender(object sender, System.EventArgs e)
        {
            //更新场景视图
            viewporttool.UpdateViewport();
        }
        private void Window1_OnLoaded(object sender, RoutedEventArgs e)
        {
            _ogreImage.InitOgreAsync();
              
        }
        private void Window1_OnClosing(object sender, CancelEventArgs e)
        {
            RenterTargetControl.Source = null;
            EsdSceneManager.Singleton.MaterialPtr = null;
            _ogreImage.Dispose();
        }

        private void OgreMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(RenterTargetControl);

            MessageBox.Show(position.ToString());
        }

        private void NewScene_Click(object sender, RoutedEventArgs e)
        {
            NewSceneTool tool = new NewSceneTool();
            tool.Click();
           
        }

        private void Viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Viewport_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Viewport_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
