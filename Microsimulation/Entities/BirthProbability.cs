using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsimulation.Entities
{
    public class BirthProbability
    {
        public int Age { get; set; }
        public int nbrOfChildren { get; set; }
        public double P { get; set; }
    }
}
