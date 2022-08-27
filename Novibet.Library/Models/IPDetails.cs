using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novibet.Library.Models
{
    public class IPDetails
    {
        public string Ip { get; set; }

        public string? City { get; set; }
        public string? Country { get; set; }

        public string? Continent { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public override bool Equals(Object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                IPDetails otherObj = (IPDetails)obj;
                return (this.Ip == otherObj.Ip) && (this.City == otherObj.City) && (this.Country == otherObj.Country) && (this.Continent == otherObj.Continent) && (this.Longitude == otherObj.Longitude) && (this.Latitude == otherObj.Latitude);
            }
        }
    }
}
