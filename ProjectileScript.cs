using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public LayerMask DestroyOnContactWith;

    private void OnEnable()
    {
        TeleportationEntityListener.OnTeleportationInstance += ReactToTeleportationEvent;
        CollisionEntityListener.OnCollisionInstance += ReactToCollisionEvent;
    }

    private void OnDisable()
    {
        TeleportationEntityListener.OnTeleportationInstance -= ReactToTeleportationEvent;
        CollisionEntityListener.OnCollisionInstance -= ReactToCollisionEvent;
    }

    private void Update()
    {
        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        transform.rotation = UtilityClass.GetRotation((Vector2)transform.position+GetComponent<Rigidbody2D>().velocity.normalized, transform.position);
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

    public void ReactToCollisionEvent(Collision2D aCollision, CollisionEntityListener aCollisionEntityListener)
    {
        if (aCollisionEntityListener == gameObject.GetComponent<CollisionEntityListener>())
        {
            if (UtilityClass.IsLayerInLayerMask(aCollision.gameObject.layer, DestroyOnContactWith))
            { DestroyEntity(); }
        }
    }

    public void DestroyEntity()
    { Destroy(gameObject); }
}
