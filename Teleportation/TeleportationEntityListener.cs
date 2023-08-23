using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationEntityListener : MonoBehaviour
{
    public delegate void TeleportationHandler(bool aBool, TeleportationEntityListener aTeleportationEntityCaller);
    public static event TeleportationHandler OnTeleportationInstance;

    public void BroadcastTeleportationEvent(bool aSuccessful)
    {
        OnTeleportationInstance?.Invoke(aSuccessful, this);
    }
}
