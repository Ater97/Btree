using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_3.Utilities
{
    public class Entry<T, P> : IEquatable<Entry<T, P>>
    {
        public T Key { get; set; }

        public P Pointer { get; set; }

        public bool Equals(Entry<T, P> other)
        {
            return this.Key.Equals(other.Key) && this.Pointer.Equals(other.Pointer);
        }
    }
}
