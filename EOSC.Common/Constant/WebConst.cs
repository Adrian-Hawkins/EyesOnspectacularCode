using EOSC.Common.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSC.Common.Constant
{
    public class WebConst
    {
        private readonly string _webUrl;
        public WebConst()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<WebUrl>()
                .AddEnvironmentVariables()
                .Build();
            _webUrl = config["web:url"] ?? throw new Exception("Please provide web url");
        }

        public string GetWebUrl()
        {
            return _webUrl;
        }
    }
}
