using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class FormattedAddress
    {
        public string InputAddress { get; set; }
        public string Unit { get; set; }
        public string House { get; set; }

        public string StreetName { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public int PinCode { get; set; }
        public string Country { get; set; }

    }
}