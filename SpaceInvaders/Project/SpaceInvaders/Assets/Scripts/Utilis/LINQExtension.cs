using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Utilis
{
    public static class LINQExtension
    {
        static Random random = new Random();

        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("Cannot get random element from empty set");
            } 

            return source.ToArray()[random.Next(0, source.Count())];
        }
    }
}
