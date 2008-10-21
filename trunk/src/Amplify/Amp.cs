using System;
using System.Collections.Generic;
using System.Text;

public class Amp
{
    private static Amplify.Hash s_properties;

    public static Amplify.Hash Properties
    {
        get {
            if (s_properties == null)
                s_properties = new Amplify.Hash();
            return s_properties;
        }
    }

    



}
