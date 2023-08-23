using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [Range(1, 2)] public int ArenaOfOrigin;

    public delegate void CollisionHandler(Collision2D aCollision, int aArena, GameObject aRequestingPortalObj);
    public static event CollisionHandler UponCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UponCollision?.Invoke(collision, ArenaOfOrigin, this.gameObject);
    }
}
