﻿#pragma checksum "..\..\Window1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BFF0066DC8509562941FB1C00E66FA12"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4927
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using OgreLib;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Esd {
    
    
    /// <summary>
    /// Window1
    /// </summary>
    public partial class Window1 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\Window1.xaml"
        internal System.Windows.Controls.Menu menu1;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button1;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button2;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button3;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button4;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button5;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button6;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button7;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button8;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button9;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button10;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button11;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button12;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button13;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\Window1.xaml"
        internal System.Windows.Controls.Button Button14;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\Window1.xaml"
        internal System.Windows.Controls.Image RenterTargetControl;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\Window1.xaml"
        internal OgreLib.OgreImage _ogreImage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Esd;component/window1.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Window1.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\Window1.xaml"
            ((Esd.Window1)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window1_OnLoaded);
            
            #line default
            #line hidden
            
            #line 6 "..\..\Window1.xaml"
            ((Esd.Window1)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window1_OnClosing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.menu1 = ((System.Windows.Controls.Menu)(target));
            return;
            case 3:
            
            #line 11 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NewScene_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Button1 = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\Window1.xaml"
            this.Button1.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Viewport_MouseDown);
            
            #line default
            #line hidden
            
            #line 53 "..\..\Window1.xaml"
            this.Button1.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Viewport_MouseLeave);
            
            #line default
            #line hidden
            
            #line 53 "..\..\Window1.xaml"
            this.Button1.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Viewport_MouseUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Button2 = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.Button3 = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.Button4 = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.Button5 = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.Button6 = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.Button7 = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.Button8 = ((System.Windows.Controls.Button)(target));
            return;
            case 12:
            this.Button9 = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.Button10 = ((System.Windows.Controls.Button)(target));
            return;
            case 14:
            this.Button11 = ((System.Windows.Controls.Button)(target));
            return;
            case 15:
            this.Button12 = ((System.Windows.Controls.Button)(target));
            return;
            case 16:
            this.Button13 = ((System.Windows.Controls.Button)(target));
            return;
            case 17:
            this.Button14 = ((System.Windows.Controls.Button)(target));
            return;
            case 18:
            this.RenterTargetControl = ((System.Windows.Controls.Image)(target));
            
            #line 105 "..\..\Window1.xaml"
            this.RenterTargetControl.SizeChanged += new System.Windows.SizeChangedEventHandler(this.RenterTargetControl_SizeChanged);
            
            #line default
            #line hidden
            
            #line 105 "..\..\Window1.xaml"
            this.RenterTargetControl.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.OgreMouseDown);
            
            #line default
            #line hidden
            return;
            case 19:
            this._ogreImage = ((OgreLib.OgreImage)(target));
            
            #line 109 "..\..\Window1.xaml"
            this._ogreImage.Initialised += new System.Windows.RoutedEventHandler(this._ogre_OnInitialised);
            
            #line default
            #line hidden
            
            #line 109 "..\..\Window1.xaml"
            this._ogreImage.PreRender += new System.EventHandler(this._image_PreRender);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
