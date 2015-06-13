#region

using System;
using System.Web.Http.ExceptionHandling;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Logging;

#endregion

namespace ESS.Framework.UI.Tracing
{
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private readonly ILogger _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.Error(string.Format("Unhandled exception thrown in {0} for request {1}: {2}",
                                        context.Request.Method, context.Request.RequestUri, context.Exception.Message));
        }
    }
}