using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnTargetSet();
    public static event OnTargetSet onTargetSet;

    public static void RaiseOnTargetSet()
    {
        if (onTargetSet != null)
        {
            onTargetSet();
        }
    }


}
