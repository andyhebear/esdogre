using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using Mogre;


namespace OgreDem
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dlg.FileName;
                DemData dm = new DemData(filename);
                dm.createMesh();
                var sceneMgr = _ogreImage.SceneManager;
                Entity thisEntity = sceneMgr.CreateEntity("cc", "ColourCube");
                thisEntity.SetMaterialName("Examples/GrassFloor");
                SceneNode thisSceneNode = sceneMgr.RootSceneNode.CreateChildSceneNode();

                thisSceneNode.AttachObject(thisEntity);
               thisSceneNode.ShowBoundingBox = true;
               _ogreImage.Camera.Position = new Vector3((dm.max.x - dm.min.x) / 2, (dm.max.y - dm.min.y) / 2,- dm.max.z+1000);

               // Look back along -Z
               _ogreImage.Camera.LookAt(new Vector3((dm.max.x - dm.min.x) / 2, (dm.max.y - dm.min.y) / 2, dm.max.z));
                //_ogreImage.Camera.NearClipDistance = 5;
            }
        }
        void _image_InitScene(object sender, RoutedEventArgs e)
        {
            _ogreImage.Camera.Position = new Vector3(32000, 32000, 9000);

            // Look back along -Z
            _ogreImage.Camera.LookAt(new Vector3(32000, 32000, 10));

            _ogreImage.Camera.NearClipDistance = 5;
            _ogreImage.Camera.FarClipDistance = 300000f;

 
            var sceneMgr = _ogreImage.SceneManager;
            sceneMgr.AmbientLight = new ColourValue(1f, 1f, 1f);
            // Create a skydome
            sceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);
        }
       

        void _image_PreRender(object sender, System.EventArgs e)
        {

        }
        private void RenterTargetControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_ogreImage == null) return;

            _ogreImage.ViewportSize = e.NewSize;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ogreImage.InitOgreAsync();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        { // Move about 100 units per second,
            float moveScale = 3000 * 0.04f;

            Vector3 translateVector = Vector3.ZERO;
            switch (e.Key)
            {
                case System.Windows.Input.Key.A:
                    translateVector.x = -moveScale;
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
