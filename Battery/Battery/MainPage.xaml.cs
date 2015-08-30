using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Battery
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

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Windows.Devices.Power.Battery.AggregateBattery.ReportUpdated += AggregateBatteryOnReportUpdated;

			await UpdatePercentage(Windows.Devices.Power.Battery.AggregateBattery);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			Windows.Devices.Power.Battery.AggregateBattery.ReportUpdated -= AggregateBatteryOnReportUpdated;
		}

		private async void AggregateBatteryOnReportUpdated(Windows.Devices.Power.Battery sender, object args)
		{
			await UpdatePercentage(sender);
		}

		private async Task UpdatePercentage(Windows.Devices.Power.Battery sender)
		{
			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				var batteryReport = sender.GetReport();
				var percentage = (batteryReport.RemainingCapacityInMilliwattHours.Value /
								  (double)batteryReport.FullChargeCapacityInMilliwattHours.Value);
				this.BatteryPercentageTextBlock.Text = percentage.ToString("##%");
			});
		}
	}
}
