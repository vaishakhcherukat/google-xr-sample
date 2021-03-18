using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidWrapper
{
    public static void GetCredential()
    {
        AndroidJavaClass helper = new AndroidJavaClass("net.rapidvalue.laureate.vr.FirebaseHelper");

        helper.CallStatic("getCredential");
    }
}
