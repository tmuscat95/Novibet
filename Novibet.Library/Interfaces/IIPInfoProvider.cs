using Novibet.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novibet.Library.Interfaces
{
    public interface IIPInfoProvider
    {
        IPDetails GetDetails(string ip);
    }
}
