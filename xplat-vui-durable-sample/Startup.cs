using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using XPlat.VUI;

[assembly: FunctionsStartup(typeof(XPlat.VUI.DurableSample.Startup))]
namespace XPlat.VUI.DurableSample
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAssistant<IDurableAssistant, CountAssistant>();
        }
    }
}