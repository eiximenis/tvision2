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

            if (index > list.Count / 2)
            {
                return ReverseNodeAt(list, index);
            }
            else
            {
                return ForwardNodeAt(list, index);
            }

        }

        private static LinkedListNode<T> ForwardNodeAt<T>(LinkedList<T> list, int index)
        {
            var current = list.First;
            var remaining = index;
            while (remaining > 0)
            {
                current = current.Next;
                remaining--;
            }

            return current;
        }

        private static LinkedListNode<T> ReverseNodeAt<T>(LinkedList<T> list, int index)
        {
            var current = list.Last;
            var remaining = list.Count - (index + 1);
            while (remaining > 0)
            {
                current = current.Previous;
                remaining--;
            }

            return current;
        }
    }
}
