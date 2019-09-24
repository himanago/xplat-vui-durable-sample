using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using XPlat.VUI;

namespace XPlat.VUI.DurableSample
{
    public interface IDurableAssistant : IAssistant
    {
        ILogger Logger { get; set; }
        IDurableOrchestrationClient DurableOrchestrationClient { get; set; }
    }
}