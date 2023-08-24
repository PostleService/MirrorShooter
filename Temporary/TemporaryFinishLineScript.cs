using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TemporaryFinishLineScript : MonoBehaviour
{
    public LayerMask DestroyOnContactWith;
    public GameObject Message;

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
        if (aCollisionEntityListener == gameObject.GetComponent<CollisionEntityListener>())
        {
            if (UtilityClass.IsLayerInLayerMask(aCollision.gameObject.layer, DestroyOnContactWith))
            { PerformAction(); }
        }
    }

    public void PerformAction()
    {
        TextMeshProUGUI text = Message.GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        Destroy(this.gameObject);
    }

}
