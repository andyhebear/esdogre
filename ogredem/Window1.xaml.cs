using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using Mogre;
using System.Windows.Input;


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
        DemData dm = null;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dlg.FileName;
               //int md = Convert.ToInt32(maxdem.Text);
                int id = Convert.ToInt32(mindem.Text);
                int sd = Convert.ToInt32(step.Text);
                CloseDem();
                dm = new DemData();
                dm.LoadDemFile(filename);
                dm.SetDemP(0, id, sd);
                int j= dm.CreateMesh( _ogreImage.SceneManager);
                
            }
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            CloseDem();
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (dm == null)
            {
                System.Windows.MessageBox.Show("没有打开dem文件");
                return;
            }
           // int md = Convert.ToInt32(maxdem.Text);
            int id = Convert.ToInt32(mindem.Text);
            int sd = Convert.ToInt32(step.Text);       
            dm.SetDemP(0, id, sd);
            CloseDem();
            int j = dm.CreateMesh(_ogreImage.SceneManager);
        }
        private void CloseDem()
        {
            if (dm != null)
                dm.Close();
        }
        void _image_InitScene(object sender, RoutedEventArgs e)
        {
           // _ogreImage.Camera.Position = new Vector3(32000, 32000, 9000);

            // Look back along -Z
           // _ogreImage.Camera.LookAt(new Vector3(32000, 32000, 10));

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
            float moveScale = 300 * 0.04f;

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
        /// <summary>
        /// 当前操作
        /// 1:移动
        /// 2:旋转俯视
        /// </summary>
        int CurrentOperate = 1;
        /// <summary>
        /// 当前鼠标是否按下
        /// </summary>
        bool MouseDownFlag = false;
        /// <summary>
        /// 鼠标按下点
        /// </summary>
        Point MouseDownPt;
        /// <summary>
        /// 用来保存鼠标按下状态.该值直到鼠标抬起进才改变
        /// </summary>
        Point MouseDownPtTemp;
        private void RenterTargetControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //得到当前按下的是左键还是右键，以便看是漫游还是透视或俯视
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CurrentOperate = 1;
            }
            else
            {
                CurrentOperate = 2;
            }
            //记录当前鼠标状态，
            //鼠标是否按下
            MouseDownFlag = true;
            MouseDownPt = e.GetPosition(sender as System.Windows.Controls.Image);// e.Location;
            MouseDownPtTemp = e.GetPosition(sender as System.Windows.Controls.Image);// e.Location;
           
        }

        private void RenterTargetControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var temppoint = e.GetPosition(sender as System.Windows.Controls.Image);
            MouseDownFlag = false;
           
         
        }

        private void RenterTargetControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var temppoint = e.GetPosition(sender as System.Windows.Controls.Image);
            if (MouseDownFlag)
            {
                switch (CurrentOperate)
                {
                    case 1://移动，此功能同地图漫游算法相同，
                        {
                                                      
                            //将屏幕点转换成三维点，
                            Vector3 v1 = _ogreImage.ScreenPtToSpaceVector(temppoint);
                            Vector3 v2 = _ogreImage.ScreenPtToSpaceVector(MouseDownPt);
                            //得到三维空间中的漫游偏移
                            float offsetx = v1.x - v2.x;
                            float offsety = v1.y - v2.y;
                            //得到当前摄像机的位置，
                            Vector3 position = _ogreImage.Camera.Position;
                            //加上偏移
                            position.x -= offsetx;
                            position.y -= offsety;
                            //更新摄像机的位置
                            _ogreImage.Camera.Position = position;
                            //更新摄像机观看位置
                            Vector3 lockat = _ogreImage.LockAt;
                            lockat.x -= offsetx;
                            lockat.y -= offsety;
                            _ogreImage.LockAt = lockat;
                            //更新摄像机
                            _ogreImage.UpdataCamera();
                        }
                        break;
                    case 2://俯视旋转
                        {

                            //得到俯视，透视的角度
                            float offsetx = (float)(temppoint.X - MouseDownPt.X) / (float)_ogreImage.PixelWidth;// MainOgreForm.Singleton.panel1.Width;
                            float offsety = (float)(temppoint.Y - MouseDownPt.Y) / (float)_ogreImage.PixelHeight;// MainOgreForm.Singleton.panel1.Height;
                            //更新角度
                            _ogreImage.CamerRoateDegree += offsetx;
                            _ogreImage.CamerLookdownDegree += offsety;
                            _ogreImage.UpdataCamera();
                        }
                        break;
                    default:
                        break;
                }
                MouseDownPt = temppoint;
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
          
            if (e.Delta > 0)//放大
            {
                if (_ogreImage.CamerDistanceLock - _ogreImage.CamerDistanceLock * 0.05f < 10)
                    return;
                _ogreImage.CamerDistanceLock -= _ogreImage.CamerDistanceLock * 0.05f;
                _ogreImage.UpdataCamera();
            }
            else//缩小
            {
                if (_ogreImage.CamerDistanceLock + _ogreImage.CamerDistanceLock * 0.05f > 100000)
                    return;
                _ogreImage.CamerDistanceLock += _ogreImage.CamerDistanceLock * 0.05f;
                _ogreImage.UpdataCamera();
            }
        }

      

      
        //bool mousestate = false;
        //Point prepoint;
        //private void RenterTargetControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    mousestate = true;
        //    var point = e.GetPosition(this);

        //}

        //private void RenterTargetControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    mousestate = false;
        //}

        //private void RenterTargetControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    Vector3 translateVector = Vector3.ZERO;
        //    var ept = e.GetPosition(this);
        //    float relx, rely;
        //    relx = (float)(ept.X - prepoint.X);
        //    rely = (float)(ept.Y - prepoint.Y);
        //    if (mousestate)
        //    {
        //        Degree cameraYaw = -relx * .13f;
        //        Degree cameraPitch = -rely * .13f;
        //        var camera = _ogreImage.Camera;
        //        camera.Yaw(cameraYaw);
        //        camera.Pitch(cameraPitch);
        //    }
        //    prepoint = ept;
        //}
    }
}
