using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manlike.BLL.Infrastructure
{
    public abstract class DataService : IDataService
    {
        protected readonly ILogger logger;
        protected DataServiceResult Success => DataServiceResult.Success();

        public DataService(ILogger logger)
        {
            this.logger = logger;
        }

        protected DataServiceResult CommonError(string message, Exception e)
        {
            var msg = $"{message}: {e.Message}";
            if (e.InnerException != null)
                msg += $";{e.InnerException.Message}";
            logger.LogError(msg);
            return DataServiceResult.Failed(message);
        }
    }
}
