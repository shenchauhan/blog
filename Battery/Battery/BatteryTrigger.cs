using System;
using System.Threading.Tasks;
using Windows.System.Power;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Battery
{
	public class BatteryTrigger : StateTriggerBase
	{
		public bool Charging { get; set; }

		public BatteryTrigger()
		{
			Windows.Devices.Power.Battery.AggregateBattery.ReportUpdated 
				+= async (sender, args) => await UpdateStatus();

			UpdateStatus();
		}
		

			private async Task UpdateStatus()
			{
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					var batteryReport = Windows.Devices.Power.Battery.AggregateBattery.GetReport();
					var percentage = (batteryReport.RemainingCapacityInMilliwattHours.Value /
									  (double)batteryReport.FullChargeCapacityInMilliwattHours.Value);

					if (percentage < 0.85)
					{
						switch (batteryReport.Status)
						{
							case BatteryStatus.Charging:
								SetActive(!this.Charging);
								break;
							case BatteryStatus.Discharging:
								SetActive(this.Charging);
								break;
						}
					}
					else
					{
						SetActive(!this.Charging);
					}
				});
			}
	}
}