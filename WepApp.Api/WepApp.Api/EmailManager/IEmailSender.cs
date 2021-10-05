using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.Entities;

namespace WepApp.Api.EmailManager
{
    public interface IEmailSender
    {
        void Send(ICronJob job, string filename);
    }
}
