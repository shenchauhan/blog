using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace GroceryShop.Background
{
    public sealed class Grocery : IBackgroundTask
    {
        private BackgroundTaskDeferral deferral;
        private AppServiceConnection appServiceConnection;
        private Dictionary<string, string> groceries = new Dictionary<string, string> { { "Apples", "$799" }, { "Oranges", "$1" }, { "Bananas", "$1.99" }, { "Pears", "$0.50" } };

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            this.deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += TaskInstance_Canceled;

            var trigger = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            this.appServiceConnection = trigger.AppServiceConnection;
            this.appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
        }

        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var deferral = args.GetDeferral();
            var requestMessage = args.Request.Message;
            var responseMessage = new ValueSet();

            foreach (var item in requestMessage)
            {
                if (groceries.ContainsKey(item.Value.ToString()))
                {
                    responseMessage.Add(item.Value.ToString(), groceries[item.Value.ToString()]);
                }
            }

            await args.Request.SendResponseAsync(responseMessage);

            deferral.Complete();
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (this.deferral != null)
            {
                this.deferral.Complete();
            }
        }
    }
}
