using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WepApp.Api.Entities
{
    public interface ICronJob
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LastTime { get; set; }
        public string CronExpression { get; set; }
        public TimeSpan TimeToExec();
    }
}
