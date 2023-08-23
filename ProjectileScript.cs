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

    public void DestroyEntity()
    { Destroy(gameObject); }
}
