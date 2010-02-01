using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using System.Drawing;

namespace MyOgre
{
    public class OgreView
    {
        static OgreView singleton;

        static public OgreView Singleton
        {
            get
            {
                return singleton;
            }
        }

        #region OGRE变量
        /// <summary>
        /// ogre渲染根对象
        /// </summary>
        public Root root;
        /// <summary>
        /// 场景管理器
        /// </summary>
        public SceneManager sceneMgr;
        //主节点 主要用来存放模型
        public SceneNode mainNode = null;
        /// <summary>
        /// 注记节点，广告牌
        /// </summary>
        public SceneNode billNode = null;
        //当前操作节点
        public SceneNode CurrentOperateNode = null;
        //当前操作水和草的模型
        public SceneNode CurrentWgOperageNode = null;
        //场景地面节点
        public SceneNode floorNode = null;
        /// <summary>
        /// 增加模型时的临时节点
        /// </summary>
        public SceneNode addmodelNode = null;
        /// <summary>
        /// 亮度节点
        /// </summary>
        public SceneNode sunNode = null;
        /// <summary>
        /// 雪节点
        /// </summary>
        public SceneNode snowNode = null;
        /// <summary>
        /// 雪粒子渲染引擎
        /// </summary>
        public ParticleSystem snowps = null;
        /// <summary>
        /// 雨节点
        /// </summary>
        public SceneNode rainNode = null;
        /// <summary>
        /// 雨粒子渲染引擎
        /// </summary>
        public ParticleSystem rainps = null;

        public SceneNode WRnode = null;
        /// <summary>
        /// 场景中的摄像机
        /// </summary>
        public Camera camera;
        /// <summary>
        /// 显示视口
        /// </summary>
        public Viewport viewport;
        /// <summary>
        /// 渲染窗口，当应用程序窗口改变时，会重新创建该窗口
        /// </summary>
        public RenderWindow window = null;
        protected IntPtr hWnd;
        #endregion
        //渲染窗口的高和宽
        public int PtrWidth;
        public int PtrHeight;
        /// <summary>
        /// 坐标转换时，所参照的基础面
        /// </summary>
        public Plane PublicPlane;

       
        #region 用来记录设像机状态的变量
        public double oneDegree = System.Math.PI / 180;

        private float camerdistancelock = 100;
        /// <summary>
        /// 摄像机到lock的距离
        /// </summary>
        public float CamerDistanceLock
        {
            get
            {
                return camerdistancelock;
            }
            set
            {
                camerdistancelock = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 lockat = new Vector3(0, 0, 0);
        /// <summary>
        /// 摄像机看的方向
        /// </summary>
        public Vector3 LockAt
        {
            get
            {
                return lockat;
            }
            set
            {
                lockat = value;
                camera.LookAt(value);
            }
        }
        private double camerlookdowndegree = System.Math.PI / 4;
        /// <summary>
        /// 摄像机俯视角度
        /// </summary>
        public double CamerLookdownDegree
        {
            get
            {
                return camerlookdowndegree;
            }
            set
            {
                if (value < oneDegree * 90 && value > oneDegree * 15)
                {
                    camerlookdowndegree = value;
                }
            }
        }
        private double camerroatedegree = 0;
        /// <summary>
        /// 摄像机旋转角度
        /// </summary>
        public double CamerRoateDegree
        {
            get
            {
                return camerroatedegree;
            }
            set
            {
                camerroatedegree = value;
                while (camerroatedegree >= oneDegree * 360)
                    camerroatedegree -= oneDegree * 360;
                while (camerroatedegree < 0)
                    camerroatedegree += oneDegree * 360;
            }
        }

        #endregion

        public OgreView(IntPtr hWnd)
        {
            this.hWnd = hWnd;

            singleton = this;
        }
        /// <summary>
        /// 创建渲染窗口
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateWindows(uint width, uint height)
        {
            if (root == null)
                return;
            PtrWidth = (int)width;
            PtrHeight = (int)height;
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = hWnd.ToString();
            //如果窗口已经创建过，这销毁
            if (window != null)
            {
                root.DetachRenderTarget(window);
                window.Dispose();
                window = null;

                sceneMgr.DestroyAllEntities();
                sceneMgr.RootSceneNode.DetachAllObjects();
                root.DestroySceneManager(sceneMgr);
                sceneMgr.Dispose();
                sceneMgr = null;
                CurrentOperateNode = null;
            }
            window = root.CreateRenderWindow("Simple Mogre Form Window", width, height, false, misc);
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
            //----------------------------------------------------- 
            // 4 Create the SceneManager
            // 
            //		ST_GENERIC = octree
            //		ST_EXTERIOR_CLOSE = simple terrain
            //		ST_EXTERIOR_FAR = nature terrain (depreciated)
            //		ST_EXTERIOR_REAL_FAR = paging landscape
            //		ST_INTERIOR = Quake3 BSP
            //----------------------------------------------------- 

            sceneMgr = root.CreateSceneManager(SceneType.ST_EXTERIOR_REAL_FAR, "SceneMgr");
            sceneMgr.AmbientLight = new ColourValue(1f,1f, 1f);

            // Create a skydome
            sceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);
         
            //----------------------------------------------------- 
            // 5 Create the camera 
            //----------------------------------------------------- 
            camera = sceneMgr.CreateCamera("SimpleCamera");
            camera.Position = new Vector3(0, 0, 100);

            // Look back along -Z
            camera.LookAt(lockat);

            camera.NearClipDistance = 5;
            camera.FarClipDistance = 3000f;

            viewport = window.AddViewport(camera);

            //初始化平面
            PublicPlane.normal = Vector3.UNIT_Z;
            PublicPlane.d = 0;
            //当窗口改变大小时。会重表创建渲染窗口，这时，场景管理器中的所有结点会被销毁，所以这需重新创建。
            mainNode = sceneMgr.RootSceneNode.CreateChildSceneNode("ogreNode");
            //广告牌节点，其它如上节点
            billNode = sceneMgr.RootSceneNode.CreateChildSceneNode("billset");
            WRnode = sceneMgr.RootSceneNode.CreateChildSceneNode("wrnode");
            foreach (ModelEntryStruct entity in MainOgreForm.Singleton.ModelDataManage.modelEntry.模型链表)
            {
                //循环创建场景结点
                SceneNode node = mainNode.CreateChildSceneNode();

                Entity ent = OgreView.Singleton.sceneMgr.CreateEntity(entity.实体名, entity.模型名);
                if (entity.材质 != "")
                {
                    ent.SetMaterialName(entity.材质);
                }
                node.AttachObject(ent);
                node.Position = entity.位置;
                node.Scale(entity.缩放比例);
                Quaternion x = new Quaternion(new Radian(entity.旋转角度.x), Vector3.UNIT_X);
                Quaternion y = new Quaternion(new Radian(entity.旋转角度.y), Vector3.UNIT_Y);
                Quaternion z = new Quaternion(new Radian(entity.旋转角度.z), Vector3.UNIT_Z);
                node.Orientation *= x * y * z;
                //en.RenderQueueGroup = (byte)80;
            }
            //广告牌
            foreach (BillBoardstruct billentry in MainOgreForm.Singleton.ModelDataManage.modelEntry.广告牌)
            {
                FontStyle fs=new FontStyle();
                if (billentry.粗)
                {
                    fs=FontStyle.Bold;
                }
                if (billentry.下划线)
                {
                    fs=fs|FontStyle.Underline;
                }
                if (billentry.斜体)
                {
                    fs=fs|FontStyle.Italic;
                }
                CreateBillBoard(billentry.位置,new System.Drawing.Font(billentry.字体名,billentry.字体大小,fs),Color.FromArgb(billentry.颜色),billentry.注记名,billentry.实体名);
            }
            //水和草
            foreach (WaterGrass wg in MainOgreForm.Singleton.ModelDataManage.modelEntry.水和草)
            {
                DrawLine3DClass  dlc = new DrawLine3DClass(wg.PtList, wg.MaterialName, wg.Name);
                dlc.ManualLineObject.RenderQueueGroup = (byte)80;
                SceneNode node = WRnode.CreateChildSceneNode();
                node.AttachObject(dlc.ManualLineObject);
            }
            //路
            foreach (RoadStruct road in MainOgreForm.Singleton.ModelDataManage.modelEntry.路)
            {
                AddRoadTool.DrawRoad(road, 1000, 1000);
            }

            //载入场景的地面
            string planename = MainOgreForm.Singleton.ModelDataManage.modelEntry.模型名;
            //载入场景的地面
            string filename = MainOgreForm.Singleton.ModelDataManage.modelEntry.场景地面图片;
            if (planename != null && planename != "")
            {
                // 创建场景地表
                Entity ent = OgreView.Singleton.sceneMgr.CreateEntity("floor", planename);
                OgreView.Singleton.floorNode = OgreView.Singleton.sceneMgr.RootSceneNode.CreateChildSceneNode();
                OgreView.Singleton.floorNode.AttachObject(ent);
                ent.SetMaterialName(filename);
            }

            addmodelNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
           


            //更新摄像机
            OgreView.Singleton.UpdataCamera();
           // camera.PolygonMode = PolygonMode.PM_WIREFRAME;
            InitSun();
            InitPs();
            //初始化动画

        }
        //初始化ogre环境。
        public void InitMogre()
        {
            //----------------------------------------------------- 
            // 1 enter ogre 
            //----------------------------------------------------- 
            root = new Root();
            //----------------------------------------------------- 
            // 2 configure resource paths
            //----------------------------------------------------- 
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);
            // Go through all sections & settings in the file
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            String secName, typeName, archName;
            // Normally we would use the foreach syntax, which enumerates the values, but in this case we need CurrentKey too;
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }

            //----------------------------------------------------- 
            // 3 Configures the application and creates the window
            //----------------------------------------------------- 
            bool foundit = false;
            foreach (RenderSystem rs in root.GetAvailableRenderers())
            {
                root.RenderSystem = rs;
                String rname = root.RenderSystem.Name;
                if (rname == "Direct3D9 Rendering Subsystem")
                {
                    foundit = true;
                    break;
                }
            }

            if (!foundit)
                return; //we didn't find it... Raise exception?

            //we found it, we might as well use it!
            root.RenderSystem.SetConfigOption("Full Screen", "No");
            root.RenderSystem.SetConfigOption("Video Mode", "1024 x 768 @ 32-bit colour");

            root.Initialise(false);     
        }

        public void RenderView()
        {
           
            root.RenderOneFrame();

        }
        /// <summary>
        /// 空间坐标转换成屏幕坐标，
        /// 原理：同下。返解
        /// </summary>
        /// <param name="verctor"></param>
        /// <returns></returns>
        public PointF SpaceVectorToScreenPt(Vector3 verctor)
        {
            Vector4 v4 = new Vector4(verctor.x, verctor.y, verctor.z, 1);
            Matrix4 viewMatrix = camera.ViewMatrix;
            Matrix4 projectMatrix = camera.ProjectionMatrix;
            Matrix4 eyeMatrix = (new Matrix4(0.5f, 0, 0, 0.5f, 0, -0.5f, 0, 0.5f, 0, 0, 1, 0, 0, 0, 0, 1) * (projectMatrix * viewMatrix));
            Vector4 temp = eyeMatrix * v4;
            Vector3 ScreenPos = new Vector3(temp.x / temp.w, temp.y / temp.w, temp.z / temp.w);
            float xx = ScreenPos.x * PtrWidth;
            float yy = ScreenPos.y * PtrHeight;
            return new PointF(xx, yy);
        }
        /// <summary>
        /// 屏幕坐标转换成空间坐标
        /// 原理：根据鼠标点击到渲染窗口的位置和摄像机的位置，两点构成一条方向向量，求出向量与参考平面的交点，即空间坐标。，
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public Vector3 ScreenPtToSpaceVector(PointF pt)
        {
            float screenx = ((float)pt.X) / ((float)PtrWidth);
            float screeny = ((float)pt.Y) / ((float)PtrHeight);
            //创建射线
            Ray ray = camera.GetCameraToViewportRay(screenx, screeny);
            Pair<bool, float> pp = ray.Intersects(PublicPlane);
            Vector3 temp3 = ray.GetPoint(pp.second);
            return temp3;

        }

        /// <summary>
        ///当控制摄像机变量更改时， 更新摄像机和方向和位置，
        /// </summary>
        public void UpdataCamera()
        {
            Vector3 position = camera.Position;
            float z = (float)(System.Math.Sin(this.CamerLookdownDegree) * this.camerdistancelock);

            float tempxheight = (float)(System.Math.Cos(this.CamerLookdownDegree) * this.camerdistancelock);
            float y = this.LockAt.y - (float)(System.Math.Cos(this.CamerRoateDegree) * tempxheight);
            float x = this.LockAt.x - (float)(System.Math.Sin(this.CamerRoateDegree) * tempxheight);

            position.x = x;
            position.z = z;
            position.y = y;

            camera.Position = position;

            this.LockAt = this.LockAt;

            Quaternion q;
            if (this.CamerLookdownDegree >= System.Math.PI / 180 * 89)
            {
                q = new Quaternion(new Radian((float)(this.CamerRoateDegree)), camera.Direction);
            }
            else
            {
                Vector3 pointOnBase = new Vector3(position.x, position.y, this.LockAt.z);	// 摄像机点在基准面上的投影
                Plane p = new Plane(this.LockAt, position, pointOnBase);
                Vector3 pp = p.normal;
                Vector3 r = camera.Right;
                q = r.GetRotationTo(pp);
            }
            camera.Rotate(q);
        }

        /// <summary>
        /// 根据起点与方向向量，求在线上指定距离的点
        /// </summary>
        /// <param name="pt">起点</param>
        /// <param name="x">方向向量x</param>
        /// <param name="y">方向向量y</param>
        /// <param name="z">方向向量z</param>
        /// <param name="d">距离</param>
        /// <returns>计算点</returns>
        public Vector3 GetPointOnLine(Vector3 pt, double x, double y, double z, double d)
        {
            double max = System.Math.Max(System.Math.Abs(x), System.Math.Abs(y));
            max = System.Math.Max(max, System.Math.Abs(z));
            x /= max;
            y /= max;
            z /= max;

            x = pt.x + x * d;
            y = pt.y + y * d;
            z = pt.z + z * d;
            return new Vector3((float)x, (float)y, (float)z);
        }

        public void Dispose()
        {
            if (root != null)
            {
                root.Dispose();
                root = null;
            }
        }

        /// <summary>
        /// 复位摄像机
        /// </summary>
        public void RestCamer()
        {
            camerdistancelock = 100;
            lockat = new Vector3(0, 0, 0);
            camerlookdowndegree = System.Math.PI / 4;
            camerroatedegree = 0;
            camera.Position = new Vector3(0, 0, CamerDistanceLock);
            camera.LookAt(lockat);
            UpdataCamera();
        }

        public void InitSun()
        {
            sunNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            //sunNode.Position = new Vector3(0, 0, 200);

            //create a light, use default parameters
            Light l = sceneMgr.CreateLight("Light11");
            l.Type = Light.LightTypes.LT_POINT;
            l.Position = new Vector3(0, 0,2000);
            l.DiffuseColour = new ColourValue(1f, 1f, 1f);
            l.SpecularColour = new ColourValue(1f, 1f, 1f);
            l.Visible = true;
            //attach light to pivot
            sunNode.AttachObject(l);

            BillboardSet bst = sceneMgr.CreateBillboardSet("Flare1");
            bst.MaterialName = "Examples/Flare";
            sunNode.AttachObject(bst);
            Billboard bill = bst.CreateBillboard(l.Position);

            bst.Visible = true;

            sunNode.SetVisible(false);
        }
        //初始雨雪
        public void InitPs()
        {
          
            snowNode = sceneMgr.RootSceneNode.CreateChildSceneNode();  
            snowps = sceneMgr.CreateParticleSystem("snow", "Examples/Snow");

            rainNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
            rainps = sceneMgr.CreateParticleSystem("rain", "Examples/Rain");
           
           
        }
        /// <summary>
        /// 是否显示雪
        /// </summary>
        /// <param name="show"></param>
        public void ShowSnow(bool show)
        {
            if (show)
            {
                snowNode.AttachObject(snowps);
            }
            else
            {
                snowNode.DetachObject(snowps);
            }
        }
        //设置是否显示雨
        public void ShowRain(bool show)
        {
            if (show)
            {
                rainNode.AttachObject(rainps);
            }
            else
            {
                rainNode.DetachObject(rainps);
            }
        }
        /// <summary>
        /// 创建广告牌
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="font">字体</param>
        /// <param name="color">颜色</param>
        /// <param name="text">文本</param>
        /// <param name="enstring">实体名称</param>
        public void CreateBillBoard(Vector3 pos,System.Drawing.Font font,Color color,string text ,string enstring)
        {
            Graphics g = MainOgreForm.Singleton.CreateGraphics();
            SizeF size = g.MeasureString(text, font);
            g.Dispose();

            System.Drawing.Image billboard = new Bitmap((int)size.Width, (int)size.Height);

            g = Graphics.FromImage(billboard);

            SolidBrush brush = new SolidBrush(color);

            g.DrawString(text, font, brush, new PointF());

            string texturefilename = System.Windows.Forms.Application.StartupPath + "\\TempBillBoard\\" + MainOgreForm.GetDateName() + ".png";

            billboard.Save(texturefilename, System.Drawing.Imaging.ImageFormat.Png);

            billboard.Dispose();
            g.Dispose();
            brush.Dispose();

            TextureManager.Singleton.Load(texturefilename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);

            MaterialPtr materialptr = MaterialManager.Singleton.Load(texturefilename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            materialptr.GetTechnique(0).GetPass(0).CreateTextureUnitState(texturefilename);

            materialptr.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;

            materialptr.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendFactor.SBF_SOURCE_ALPHA, SceneBlendFactor.SBF_ONE_MINUS_SOURCE_ALPHA);



            //创建广告牌集合
            BillboardSet bbs = OgreView.Singleton.sceneMgr.CreateBillboardSet(enstring);
            bbs.MaterialName = texturefilename;

           
            Billboard bill = bbs.CreateBillboard(new Vector3());

            SceneNode node =billNode.CreateChildSceneNode();

            node.AttachObject(bbs);

            node.Scale(0.1f, 0.1f, 0.1f);
            node.Position = pos;
        }
       
    }
}
