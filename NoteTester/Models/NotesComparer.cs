using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTester.Models
{
    public class NotesComparer : IEqualityComparer<Note>
    {
        public bool Equals(Note x, Note y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(Note obj)
        {
            return obj.GetHashCode() ^ obj.GetHashCode();
        }
    }
}
