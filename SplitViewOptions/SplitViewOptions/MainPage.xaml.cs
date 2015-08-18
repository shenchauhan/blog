using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            this.MySplitView.IsPaneOpen = this.MySplitView.IsPaneOpen ? false : true;
        }

		private void RadioButtonPaneItem_Click(object sender, RoutedEventArgs e)
		{
			var radioButton = sender as RadioButton;

			if (radioButton != null)
			{
				switch (radioButton.Tag.ToString())
				{
					case "Map":
						this.MainFrame.Navigate(typeof(MapPage));
						break;				
				}
			}
		}
	}
}
