// Copyright (c) 2020, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;

namespace SectorDirector.Core.Tests
{
    public static class TestExtensions
    {
        public static void Shuffle<T>(this List<T> list, int seed)
        {
            var rand = new Random(seed);

            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rand.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}