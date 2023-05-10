using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratuber.Client
{
    public static class ListExtension
    {
        public static void Move<T>(this IList<T> values, T item, int newIndex)
        {
            newIndex = Math.Clamp(newIndex, 0, values.Count - 1);

            values.Remove(item);
            values.Insert(newIndex, item);
        }
    }
}
