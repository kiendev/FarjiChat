﻿#pragma checksum "C:\Users\kiennt45\Downloads\ChatRooms0307\ChatRooms\Control\SendPmControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4F9F49E7B77C8B6FF60C8FD344CEFBDB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace ChatRooms.Control {
    
    
    public partial class SendPmControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard FocusAnimation;
        
        internal System.Windows.Media.Animation.Storyboard LostFocusAnimation;
        
        internal System.Windows.Media.Animation.Storyboard AnimationShow;
        
        internal System.Windows.Media.Animation.Storyboard AnimationClose;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel ChildControl;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock TitleTextBlock;
        
        internal System.Windows.Controls.TextBlock ContentTextBlock;
        
        internal System.Windows.Controls.TextBox TxtMessage;
        
        internal System.Windows.Controls.Button LeftButton;
        
        internal System.Windows.Controls.Button RightButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ChatRooms;component/Control/SendPmControl.xaml", System.UriKind.Relative));
            this.FocusAnimation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("FocusAnimation")));
            this.LostFocusAnimation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("LostFocusAnimation")));
            this.AnimationShow = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AnimationShow")));
            this.AnimationClose = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AnimationClose")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ChildControl = ((System.Windows.Controls.StackPanel)(this.FindName("ChildControl")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.TitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("TitleTextBlock")));
            this.ContentTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ContentTextBlock")));
            this.TxtMessage = ((System.Windows.Controls.TextBox)(this.FindName("TxtMessage")));
            this.LeftButton = ((System.Windows.Controls.Button)(this.FindName("LeftButton")));
            this.RightButton = ((System.Windows.Controls.Button)(this.FindName("RightButton")));
        }
    }
}

