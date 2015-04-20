#region

using System;

#endregion

namespace ESS.Framework.Common.Scheduling
{
    public interface IScheduleService
    {
        int ScheduleTask(string actionName, Action action, int dueTime, int period);
        void ShutdownTask(int taskId);
    }
}