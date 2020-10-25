using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsimulation.Entities
{
    public class Person
    {
        public int BirthYear { get; set; }
        public Gender Gender { get; set; }
        public int nbrOfChildren { get; set; }
        public bool isAlive { get; set; }

        public Person()
        {
            isAlive = true;
        }
    }
}
