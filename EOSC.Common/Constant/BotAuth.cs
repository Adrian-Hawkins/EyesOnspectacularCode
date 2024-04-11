using EOSC.Common.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EOSC.Common.Constant
{
    public class BotAuth
    {
        private readonly string _botToken;
        public BotAuth()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<BotToken>()
                .AddEnvironmentVariables()
                .Build();
            _botToken = config["bot:token"] ?? throw new Exception("Please provide bot token");
        }

        public string GetBotToken()
        {
            return _botToken;
        }
    }
}
