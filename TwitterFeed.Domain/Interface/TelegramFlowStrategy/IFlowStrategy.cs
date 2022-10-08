using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterFeed.Infra.Interface.TelegramFlowStrategy
{
    internal interface IFlowStrategy
    {
        public bool CanHandle(string action);
        public Task HandleAsync(string action);
    }
}
