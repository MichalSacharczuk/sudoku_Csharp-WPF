using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class ElementsForPermutation
    {
        public int HowMany { get; set; }
        public int UsedGroupNr { get; set; }
        public List<string> ElementsList { get; set; }

        public ElementsForPermutation()
        {
            HowMany = 0;
            UsedGroupNr = 0;
            //ElementsList = new List<string>();
        }
    }
}
