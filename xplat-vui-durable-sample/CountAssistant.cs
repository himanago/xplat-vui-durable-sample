using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XPlat.VUI.DurableSample
{
    public class CountAssistant : AssistantBase, IDurableAssistant
    {
        public ILogger Logger { get; set; }
        public IDurableOrchestrationClient DurableOrchestrationClient { get; set; }

        protected override async Task OnLaunchRequestAsync(
            Dictionary<string, object> session, CancellationToken cancellationToken)
        {
            Logger.LogInformation("LaunchRequest");

            var status = await DurableOrchestrationClient.GetStatusAsync(Request.UserId);

            if (status?.RuntimeStatus == OrchestrationRuntimeStatus.ContinuedAsNew ||
                status?.RuntimeStatus == OrchestrationRuntimeStatus.Pending ||
                status?.RuntimeStatus == OrchestrationRuntimeStatus.Running)
            {
                Response.Speak("まだ数え終わっていません。");
            }
            else
            {
                Response
                    .Speak("いくつ数えますか？")
                    .KeepListening("いくつ数えるか教えてください。");
            }
        }

        protected override async Task OnIntentRequestAsync(
            string intent, Dictionary<string, object> slots, Dictionary<string, object> session,
            CancellationToken cancellationToken)
        {
            Logger.LogInformation("IntentRequest");

            if (intent == "CountIntent" && slots.TryGetValue("count", out var slot) &&
                int.TryParse(slot.ToString(), out var count))
            {
                await DurableOrchestrationClient.StartNewAsync("XPlatDurableFunction_RunOrchestrator", Request.UserId, count);
                Response.Speak("数え始めます。しばらくお待ちください。");
            }
            else
            {
                Response
                    .Speak("いくつ数えますか？")
                    .KeepListening("いくつ数えるか教えてください。");
            }
        }
    }
}
