//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

/// <summary>
/// A singleton class for accessing static properties. 
/// </summary>
[SuppressMessage("Microsoft.Design", "CA1050", Justification = "Amp should be a singleton accessable without forcing a namespace to be specified")]
public class Amp : IAmp 
{
    private static Amp amp;

    private Amplify.Hash properties;
 
    /// <summary>
    /// Gets the properties.
    /// </summary>
    /// <value>The properties.</value>
    public Amplify.Hash Properties
    {
        get 
        {
            if (this.properties == null)
                this.properties = new Amplify.Hash();
            return this.properties;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is in test.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is in test; otherwise, <c>false</c>.
    /// </value>
    public bool IsInTest { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is in development.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is in development; otherwise, <c>false</c>.
    /// </value>
    public bool IsInDevelopment { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is in staging.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is in staging; otherwise, <c>false</c>.
    /// </value>
    public bool IsInStaging { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is in production.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is in production; otherwise, <c>false</c>.
    /// </value>
    public bool IsInProduction 
    {
        get 
        { 
            return !this.IsInTest && !this.IsInDevelopment && !this.IsInStaging; 
        }
        set
        {
            if (value)
            {
                this.IsInTest = false;
                this.IsInDevelopment = false;
                this.IsInStaging = false;
            }
        }
    }

    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    /// <value>The connection string.</value>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets this instance.
    /// </summary>
    /// <returns></returns>
    public static IAmp Get()
    {
        if (amp == null)
            amp = new Amp();
        return amp;
    }
}
