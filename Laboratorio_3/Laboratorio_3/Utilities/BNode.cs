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
            this.Children = new List<BNode<T, P>>(degree);
            this.Entries = new List<Entry<T, P>>(degree);
        }

        public List<BNode<T, P>> Children { get; set; }

        public List<Entry<T, P>> Entries { get; set; }

        public bool IsLeaf
        {
            get
            {
                return this.Children.Count == 0;
            }
        }

        public bool HasReachedMaxEntries
        {
            get
            {
                return this.Entries.Count == (2 * this.degree) - 1;
            }
        }

        public bool HasReachedMinEntries
        {
            get
            {
                return this.Entries.Count == this.degree - 1;
            }
        }
    }
}
