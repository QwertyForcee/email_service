using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WepApp.Api.EmailManager
{
    public interface ICsvManager
    {
        public string WriteCsv(object obj);
    }
}
