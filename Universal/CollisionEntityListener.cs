using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEntityListener : MonoBehaviour
{
    public delegate void CollisionHandler (Collision2D collision, CollisionEntityListener aCollisionEntityListener);
    public static event CollisionHandler OnCollisionInstance;

    private void OnCollisionEnter2D(Collision2D collision)
    { OnCollisionInstance?.Invoke(collision, this); }
}
