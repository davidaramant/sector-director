// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using System;
using System.Collections.Generic;

namespace SectorDirector.Core.CollectionExtensions
{
    public static class Extensions
    {
        public static T FirstOr<T>(this List<T> list, Func<T,bool> predicate, T fallback )
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(predicate(list[i]))
                {
                    return list[i];
                }
            }
            return fallback;
        }
    }
}