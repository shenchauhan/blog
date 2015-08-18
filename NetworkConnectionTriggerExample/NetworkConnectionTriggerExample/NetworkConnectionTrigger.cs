using System;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace NetworkConnectionTriggerExample
{
	public class NetworkConnectionTrigger : StateTriggerBase
	{
		private bool requiresInternet;

		/// <summary>
		/// Constructor just wiring up the network changed event.
		/// </summary>
		public NetworkConnectionTrigger()
		{
			NetworkInformation.NetworkStatusChanged += NetworkInformationOnNetworkStatusChanged;
		}

		/// <summary>
		/// The property that the is set on the trigger to determine if active should be set on Offline or Online
		/// </summary>
		public bool RequiresInternet
		{
			get { return this.requiresInternet; }
			set
			{
				this.requiresInternet = value;
				if (NetworkInformation.GetInternetConnectionProfile() != null)
				{
					SetActive(value);
				}
				else
				{
					SetActive(!value);
				}
			}
		}

		/// <summary>
		/// When the network changes this events fires and the trigger is evaluated again.
		/// </summary>
		private async void NetworkInformationOnNetworkStatusChanged(object sender)
		{
			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				if (NetworkInformation.GetInternetConnectionProfile() != null)
				{
					SetActive(this.RequiresInternet);
				}
				else
				{
					SetActive(!this.RequiresInternet);
				}
			});
		}
	}
}
