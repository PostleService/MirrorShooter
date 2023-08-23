using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically requests offset from ArenaManager and follows a specified object
/// </summary>
public class FollowObjectAtOffset : MonoBehaviour
{
    private ArenaManager _arenaManager;

    [Tooltip("Left = 1, Right = 2")]
    [Range(1, 2)] public int LeftRight;

    public GameObject ObjectToFollow;

    private void Start()
    { _arenaManager = GameObject.Find("ArenaManager").GetComponent<ArenaManager>(); }

    // Update is called once per frame
    void Update()
    {
        FollowSpecifiedObjectAtOffset(ObjectToFollow);
    }

    private void FollowSpecifiedObjectAtOffset(GameObject aGameObject)
    {
        if (ObjectToFollow != null) transform.position = GetFinalPosition(aGameObject.transform.position);
    }

    private Vector3 GetFinalPosition(Vector3 aVec3)
    {
        Vector3 finalOffset;
        if (LeftRight == _arenaManager.GetCurrentArena()) { finalOffset = Vector3.zero; }
        else { finalOffset = _arenaManager.OffsetOfOpositeArena(LeftRight); }

        return new Vector3(aVec3.x, aVec3.y, -10f) - finalOffset; 
    }

}
