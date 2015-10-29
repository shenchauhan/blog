using System;
using System.ComponentModel;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Beacons
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Publish : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private BluetoothLEAdvertisementPublisher publisher;
        private string manufacturer;

        public Publish()
        {
            this.InitializeComponent();
        }

        public string Manufacturer
        {
            get { return manufacturer; }
            set
            {
                manufacturer = value;
                this.RaisePropertyChange("Manufacturer");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += Publish_BackRequested;
            }

            this.publisher = new BluetoothLEAdvertisementPublisher();

            ushort id = 0x1234;
            var manufacturerDataWriter = new DataWriter();
            manufacturerDataWriter.WriteUInt16(id);

            var manufacturerData = new BluetoothLEManufacturerData
            {
                CompanyId = 0xFFFE,
                Data = manufacturerDataWriter.DetachBuffer()
            };

            publisher.Advertisement.ManufacturerData.Add(manufacturerData);

            this.Manufacturer = "12-34";

            publisher.Start();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= Publish_BackRequested;
            }

            publisher.Stop();
        }

        private void Publish_BackRequested(object sender, BackRequestedEventArgs e)
        {
            this.Frame.GoBack();
            e.Handled = true;
        }

        private void RaisePropertyChange(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
