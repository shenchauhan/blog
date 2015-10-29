using System;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Beacons
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Watch : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private BluetoothLEAdvertisementWatcher watcher;
        private int rssi;
        private string beaconData;

        public Watch()
        {
            this.InitializeComponent();
        }

        public int Rssi
        {
            get { return rssi; }
            set
            {
                rssi = value;
                this.RaisePropertyChange("Rssi");
            }
        }

        public string BeaconData
        {
            get { return beaconData; }
            set
            {
                beaconData = value;
                this.RaisePropertyChange("BeaconData");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += Publish_BackRequested;
            }

            this.watcher = new BluetoothLEAdvertisementWatcher();
            var manufacturerDataWriter = new DataWriter();
            manufacturerDataWriter.WriteUInt16(0x1234);

            var manufacturerData = new BluetoothLEManufacturerData
            {
                CompanyId = 0xFFFE,
                Data = manufacturerDataWriter.DetachBuffer()
            };

            this.watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);

            watcher.Received += Watcher_Received;
            watcher.Start();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= Publish_BackRequested;
            }

            watcher.Received -= this.Watcher_Received;
            watcher.Stop();
        }

        private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var manufacturerData = args.Advertisement.ManufacturerData;
            if (manufacturerData.Any())
            {
                var manufacturerDataSection = manufacturerData[0];
                var data = new byte[manufacturerDataSection.Data.Length];

                using (var reader = DataReader.FromBuffer(manufacturerDataSection.Data))
                {
                    reader.ReadBytes(data);
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.Rssi = (int)args.RawSignalStrengthInDBm;
                    this.BeaconData = BitConverter.ToString(data);
                });
            }
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
