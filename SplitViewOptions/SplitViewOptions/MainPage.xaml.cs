using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SplitViewOptions
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //this.PaneOpenButton.Content = this.MySplitView.IsPaneOpen ? "Open" : "Closed";
            //this.MySplitView.PaneClosed += MySplitView_PaneClosed;
        }

        //private void MySplitView_PaneClosed(SplitView sender, object args)
        //{
        //    this.PaneOpenButton.Content =  "Closed";
        //}

        //private void ChangeDisplayModeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.MySplitView.DisplayMode == SplitViewDisplayMode.CompactInline)
        //    {
        //        this.MySplitView.DisplayMode = SplitViewDisplayMode.Inline;
        //        this.ChangeDisplayModeButton.Content = "Inline";
        //    }
        //    else if (this.MySplitView.DisplayMode == SplitViewDisplayMode.Inline)
        //    {
        //        this.MySplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
        //        this.ChangeDisplayModeButton.Content = "CompactOverlay";
        //    }
        //    else if (this.MySplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay)
        //    {
        //        this.MySplitView.DisplayMode = SplitViewDisplayMode.Overlay;
        //        this.ChangeDisplayModeButton.Content = "Overlay";
        //    }
        //    else if (this.MySplitView.DisplayMode == SplitViewDisplayMode.Overlay)
        //    {
        //        this.MySplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
        //        this.ChangeDisplayModeButton.Content = "CompactInline";
        //    }

        //    this.MySplitView.IsPaneOpen = this.MySplitView.IsPaneOpen ? false : true;
        //    this.PaneOpenButton.Content = this.MySplitView.IsPaneOpen ? "Open" : "Closed";
        //}

        //private void PaneOpenButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.MySplitView.IsPaneOpen = this.MySplitView.IsPaneOpen ? false : true;
        //    this.PaneOpenButton.Content = this.MySplitView.IsPaneOpen ? "Open" : "Closed";
        //}
    }
}
