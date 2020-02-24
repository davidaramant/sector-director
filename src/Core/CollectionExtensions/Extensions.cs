// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using System.Collections.Generic;
using System.Linq;

namespace SectorDirector.Core.CollectionExtensions
{
    public static class Extensions
    {
        public static T FirstOr<T>(this List<T> list, Func<T, bool> predicate, T fallback)
        {
            foreach (T item in list)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return fallback;
        }

        public static IEnumerable<(int Index, T Value)> Indexed<T>(this IEnumerable<T> sequence)
        {
            return sequence.Select((value, i) => (i, value));
        }

        public static T TakeFirst<T>(this LinkedList<T> list)
        {
            var value = list.First();
            list.RemoveFirst();
            return value;
        }

        public static T TakeFirst<T>(this LinkedList<T> list, Func<T,bool> predicate)
        {
            var value = list.First(predicate);
            list.Remove(value);
            return value;
        }
    }
}