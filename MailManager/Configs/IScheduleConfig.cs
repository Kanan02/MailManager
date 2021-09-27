using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailManager.Configs
{
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
