//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

    [SuppressMessage("Microsoft.Design", "CA1050", Justification = "By Design")]
    public interface IAmp
    {
        Amplify.Hash Properties { get; }

        bool IsInTest { get; }

        bool IsInDevelopment { get; }

        bool IsInStaging { get; }

        bool IsInProduction { get; }

        string ConnectionString { get; }
    }

