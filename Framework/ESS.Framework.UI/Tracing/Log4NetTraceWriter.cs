using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;
using ESS.Framework.Common.Components;
using ESS.Framework.Common.Logging;

namespace ESS.Framework.UI.Tracing
{
    public sealed class Log4NetTraceWriter : ITraceWriter
    {
        public bool IsEnabled(string category, TraceLevel level)
        {
            return true;
        }

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level == TraceLevel.Off)
                return;

            var record = new TraceRecord(request, category, level);
            traceAction(record);
            Log(record);
        }

        private Dictionary<TraceLevel, Action<string>> Logger
        {
            get { return SLoggingMap.Value; }
        }

        private void Log(TraceRecord record)
        {
            var message = new StringBuilder();

            if (record.Request != null)
            {
                if (record.Request.Method != null)
                    message.Append(" ").Append(record.Request.Method.Method);

                if (record.Request.RequestUri != null)
                    message.Append(" ").Append(record.Request.RequestUri.AbsoluteUri);
            }

            if (!string.IsNullOrWhiteSpace(record.Category))
                message.Append(" ").Append(record.Category);

            if (!string.IsNullOrWhiteSpace(record.Operator))
                message.Append(" ").Append(record.Operator).Append(" ").Append(record.Operation);

            if (!string.IsNullOrWhiteSpace(record.Message))
                message.Append(" ").Append(record.Message);

            if (record.Exception != null && !string.IsNullOrEmpty(record.Exception.GetBaseException().Message))
                message.Append(" ").AppendLine(record.Exception.GetBaseException().Message);

            Logger[record.Level](message.ToString());
        }

        internal static readonly ILogger SLog = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Log4NetTraceWriter));
        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> SLoggingMap =
            new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>>
            {
                {TraceLevel.Info, SLog.Info},
                {TraceLevel.Debug, SLog.Debug},
                {TraceLevel.Error, SLog.Error},
                {TraceLevel.Fatal, SLog.Fatal},
                {TraceLevel.Warn, SLog.Warn}
            });
    }
}