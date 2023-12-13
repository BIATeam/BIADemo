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

        /// <summary>
        /// Return all classes of an assembly with the name end by a string.
        /// </summary>
        /// <param name="assembly">the assembly to parse.</param>
        /// <param name="endString">the end of the string.</param>
        /// <returns>List of classes with the name end by a string.</returns>
        public static List<Type> GetTypesNameEndWith(Assembly assembly, string endString)
        {
            return assembly.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.Name.EndsWith(endString))
                .ToList();
        }

        /// <summary>
        /// Return all classes of an assembly with derive of a type.
        /// </summary>
        /// <param name="assembly">the assembly to parse.</param>
        /// <param name="baseType">base type of the class to find.</param>
        /// <returns>List of classes with the name end by a string.</returns>
        public static List<Type> GetTypesDerivedOfBaseTypeFromAssembly(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes()
                .Where(type => baseType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .ToList();
        }

        /// <summary>
        /// Return all classes of an assembly with derive of a generic type.
        /// </summary>
        /// <param name="assembly">the assembly to parse.</param>
        /// <param name="baseGenericType">base type of the generic class to find.</param>
        /// <returns>List of classes with the name end by a string.</returns>
        public static List<Type> GetTypesDerivedOfBaseGenericTypeFromAssembly(Assembly assembly, Type baseGenericType)
        {
            return assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && IsSubclassOfRawGeneric(type, baseGenericType))
                .ToList();
        }

        private static bool IsSubclassOfRawGeneric(Type type, Type generic)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (generic == cur)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }
    }
}
