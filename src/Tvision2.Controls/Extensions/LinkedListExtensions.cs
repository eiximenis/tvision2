using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Extensions
{
    static class LinkedListExtensions
    {
        public static LinkedListNode<T> NodeAt<T>(this LinkedList<T> list, int index)
        {
            if (index >= list.Count)
            {
                return list.Last;
            }

            if (index <= 0)
            {
                return list.First;
            }

            var current = list.First;
            var remaining = index;
            while (remaining > 0)
            {
                current = current.Next;
                remaining--;
            }

            return current;

        }

    }
}
