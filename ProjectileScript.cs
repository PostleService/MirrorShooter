using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private void OnEnable()
    {
        TeleportationEntityListener.OnTeleportationInstance += ReactToTeleportationEvent;
    }

    private void OnDisable()
    {
        TeleportationEntityListener.OnTeleportationInstance -= ReactToTeleportationEvent;
    }

    public void ReactToTeleportationEvent(bool aSuccessful, TeleportationEntityListener aTPListener)
    {
        if (aTPListener == gameObject.GetComponent<TeleportationEntityListener>())
        {
            if (aSuccessful == true)
            {

            }
            else
            { DestroyEntity(); }
        }
    }

    public void DestroyEntity()
    { Destroy(gameObject); }
}
