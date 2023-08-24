using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchScript : MonoBehaviour
{
    public LayerMask ReactToLayers;
    public GameObject TiedDoor;

    private void OnEnable()
    {
        CollisionEntityListener.OnCollisionInstance += ReactToCollisionEvent;
    }

    private void OnDisable()
    {
        CollisionEntityListener.OnCollisionInstance -= ReactToCollisionEvent;
    }

    public void ReactToCollisionEvent(Collision2D aCollision, CollisionEntityListener aCollisionEntityListener)
    {
        Debug.LogWarning("interaction");

        if (aCollisionEntityListener == gameObject.GetComponent<CollisionEntityListener>())
        {
            if (UtilityClass.IsLayerInLayerMask(aCollision.gameObject.layer, ReactToLayers))
            { PerformAction(); }
        }
    }

    public void PerformAction()
    {
        if (TiedDoor != null) Destroy(TiedDoor);
        Destroy(gameObject);
    }
}
