// <copyright file="ReflectiveEnumerator.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<Type> GetTypesNameEndWith(Assembly assembly, string endString)
        {
            List<Type> objects = new List<Type>();
            foreach (Type type in assembly.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.Name.EndsWith(endString)))
            {
                objects.Add(type);
            }

            return objects;
        }
    }
}
