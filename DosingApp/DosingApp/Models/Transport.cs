using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DosingApp.Models
{
    public class Transport
    {
        public int TransportId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public virtual List<TransportTank> TransportTanks { get; set; }
    }
}
