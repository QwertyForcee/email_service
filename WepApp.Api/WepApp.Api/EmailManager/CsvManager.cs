using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.Entities;

namespace WepApp.Api.EmailManager
{
    public class CsvManager:ICsvManager
    {
        public CsvManager(IConfiguration config)
        {
            this.directory = config.GetSection("Storage").Value;
        }
        private string directory;
        public string WriteCsv(object obj)
        {
            string name = this.directory + Guid.NewGuid() + ".csv";
            using (var writer = new StreamWriter(name))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    //csv.Context.RegisterClassMap<CronJobMap>();
                    csv.WriteRecord(obj);
                    return name;
                }
            }
        }
    }
    /*public class CronJobMap : ClassMap<ICronJob>
    {
        public CronJobMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(j => j.Email).Ignore();
            Map(j => j.Name).Ignore();
            Map(j => j.Description).Ignore();
            Map(j => j.CronExpression).Ignore();
            Map(j => j.Id).Ignore();
            Map(j => j.UserId).Ignore();
            Map(j => j.LastTime).Ignore();
        }
    }*/
}
