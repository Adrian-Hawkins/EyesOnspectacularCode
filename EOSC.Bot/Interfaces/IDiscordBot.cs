//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace EOSC.Bot.Interfaces
{
    internal interface IDiscordBot
    {
        Task StartAsync(/*ServiceProvider services*/);

        Task StopAsync();
    }
}
