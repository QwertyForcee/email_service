using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WepApp.Api.Entities
{
    public class CoinTask: ICronJob
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int CoinId { get; set; }
        public string CronExpression { get; set; }
        public string LastTime { get; set; }

        public string Email { get; set; }
        public TimeSpan TimeToExec()
        {
            return Cronos.CronExpression.Parse(this.CronExpression).GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local).Value - DateTimeOffset.Now;
        }
        //todo cron
    }
}
