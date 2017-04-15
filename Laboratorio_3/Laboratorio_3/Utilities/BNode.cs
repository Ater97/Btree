using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_3.Utilities
{
    class BNode<T, P>
    {
        private int degree;

        public BNode(int degree)
        {
            this.degree = degree;
            Children = new List<BNode<T, P>>(degree);
            Entries = new List<Entry<T, P>>(degree);
        }

        public List<BNode<T, P>> Children { get; set; }

        public List<Entry<T, P>> Entries { get; set; }

        public bool IsLeaf
        {
            get
            {
                return Children.Count == 0;
            }
        }

        public bool HasReachedMaxEntries
        {
            get
            {
                return Entries.Count == (2 * degree) - 1;
            }
        }

        public bool HasReachedMinEntries
        {
            get
            {
                return Entries.Count == degree - 1;
            }
        }
    }
}
