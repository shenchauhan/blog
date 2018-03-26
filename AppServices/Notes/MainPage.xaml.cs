using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Notes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AppServiceConnection appServiceConnection;

        public MainPage()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<ShoppingItem> Items { get; set; } = new ObservableCollection<ShoppingItem>();

        private async void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Items.Clear();

            this.appServiceConnection = new AppServiceConnection
            {
                AppServiceName = "com.shenchauhan.GroceryShop",
                PackageFamilyName = "292c6a2f-3028-412d-b530-23fbc868d2cb_8gdmnpqbw06hm"
            };

            var status = await this.appServiceConnection.OpenAsync();

            switch (status)
            {
                case AppServiceConnectionStatus.AppNotInstalled:
                    await LogError("The Grocery Shop application is not installed. Please install it and try again");
                    return;
                case AppServiceConnectionStatus.AppServiceUnavailable:
                    await LogError("The Grocery Shop application does not have the available feature");
                    return;
                case AppServiceConnectionStatus.Unknown:
                    await LogError("Uh-oh! erm.....");
                    return;
            }

            var items = this.NotesTextBox.Text.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);

            var message = new ValueSet();

            for (int i = 0; i < items.Length; i++)
            {
                message.Add(i.ToString(), items[i]);
            }

            var response = await this.appServiceConnection.SendMessageAsync(message);

            switch (response.Status)
            {
                case AppServiceResponseStatus.ResourceLimitsExceeded:
                    await LogError("Yikes! The resource for this device exceeded the limits so the app service was shutdown");
                    return;
                case AppServiceResponseStatus.Failure:
                    await LogError("Oh dear, we failed to get a response");
                    return;
                case AppServiceResponseStatus.Unknown:
                    await LogError("uh-oh... Not sure why we didn't get a response");
                    return;
            }

            foreach (var item in response.Message)
            {
                this.Items.Add(new ShoppingItem
                {
                    Name = item.Key,
                    Price = item.Value.ToString()
                });
            }
        }

        private async Task LogError(string errorMessage)
        {
            await new MessageDialog(errorMessage).ShowAsync();
        }
    }
}