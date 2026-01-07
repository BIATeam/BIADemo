// <copyright file="AssemblyInfo.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

using Microsoft.VisualStudio.TestTools.UnitTesting;

// parallel execution
[assembly: Parallelize(Workers = 1, Scope = ExecutionScope.MethodLevel)]
