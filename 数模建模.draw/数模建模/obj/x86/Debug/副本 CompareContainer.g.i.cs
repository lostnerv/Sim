﻿#pragma checksum "..\..\..\副本 CompareContainer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2074DE42F23EC94F4994B30BC3B854F5"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
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
using System.Windows.Shell;


namespace 数模建模 {
    
    
    /// <summary>
    /// CompareContainer
    /// </summary>
    public partial class CompareContainer : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\副本 CompareContainer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboTime;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\副本 CompareContainer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboCH;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\副本 CompareContainer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_draw;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\副本 CompareContainer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox drawtypeLeft;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\副本 CompareContainer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox drawtypeRight;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\副本 CompareContainer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas leftCanvas;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\副本 CompareContainer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas rightCanvas;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/数模建模;component/%e5%89%af%e6%9c%ac%20comparecontainer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\副本 CompareContainer.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.comboTime = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.comboCH = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.btn_draw = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\副本 CompareContainer.xaml"
            this.btn_draw.Click += new System.Windows.RoutedEventHandler(this.click_draw);
            
            #line default
            #line hidden
            return;
            case 4:
            this.drawtypeLeft = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.drawtypeRight = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.leftCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 25 "..\..\..\副本 CompareContainer.xaml"
            this.leftCanvas.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.canvas_down);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\副本 CompareContainer.xaml"
            this.leftCanvas.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.canvas_MouseWheel);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\副本 CompareContainer.xaml"
            this.leftCanvas.MouseMove += new System.Windows.Input.MouseEventHandler(this.canvas_move);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\副本 CompareContainer.xaml"
            this.leftCanvas.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.canvas_up);
            
            #line default
            #line hidden
            return;
            case 7:
            this.rightCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 36 "..\..\..\副本 CompareContainer.xaml"
            this.rightCanvas.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.canvas_down);
            
            #line default
            #line hidden
            
            #line 36 "..\..\..\副本 CompareContainer.xaml"
            this.rightCanvas.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.canvas_MouseWheel);
            
            #line default
            #line hidden
            
            #line 36 "..\..\..\副本 CompareContainer.xaml"
            this.rightCanvas.MouseMove += new System.Windows.Input.MouseEventHandler(this.canvas_move);
            
            #line default
            #line hidden
            
            #line 36 "..\..\..\副本 CompareContainer.xaml"
            this.rightCanvas.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.canvas_up);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

