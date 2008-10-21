using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A singleton class for accessing static properties. 
/// </summary>
public class Amp
{
    private static Amplify.Hash s_properties;

    static Amp()
    {

    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    /// <value>The properties.</value>
    public static Amplify.Hash Properties
    {
        get {
            if (s_properties == null)
                s_properties = new Amplify.Hash();
            return s_properties;
        }
    }
}
