using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private ArenaManager _arenaManager;

    [Header("Portal")]
    [Tooltip("Exists as a reference for collision bounds")] 
    public GameObject MasterPortalBlueprint;
    public LayerMask Layers_BoxCast;
    public LayerMask TeleportObjectsOfLayers;

    [Header("Portal Restrictions")]
    [Tooltip("Absolute value of 1 is fully to the right or left, 0 - approaching front or back")] 
    [Range(0, 1)] public float MaxSideAngle = .9f;

    [Header("Portal Prefabs")]
    public GameObject[] Portals = new GameObject[2];
    public GameObject[] PortalBlueprints = new GameObject[2];

    private List<GameObject> _currentPortals = new List<GameObject>();
    private List<GameObject> _currentPortalBlueprints = new List<GameObject>();

    private Bounds PortalBounds;
    private bool _portalSpawnAllowed = false;
    private Vector3 _tempPortalPosition = Vector3.zero;
    private Quaternion _tempPortalRotation = new Quaternion();

    private void OnEnable()
    {
        PortalScript.UponCollision += AssessAndTeleport;
    }

    private void OnDisable()
    {
        PortalScript.UponCollision -= AssessAndTeleport;
    }

    private void Awake()
    {
        PortalBounds = MasterPortalBlueprint.GetComponent<BoxCollider2D>().bounds;
        _arenaManager = GameObject.Find("ArenaManager").GetComponent<ArenaManager>();
    }

    public void SetAllowSpawn(bool aBool)
    { _portalSpawnAllowed = aBool; }

    private void RenderPortal(GameObject aPortal, bool aOnOff)
    { aPortal.GetComponent<Renderer>().enabled = aOnOff; }

    public void RenderPortalBlueprints(bool aBool, Vector3 aPosition, Quaternion aRotation)
    {
        foreach (GameObject go in _currentPortalBlueprints) 
        { RenderPortal(go, aBool); go.transform.rotation = aRotation; }

        _currentPortalBlueprints[_arenaManager.GetCurrentArena() - 1].transform.position = aPosition;
        _currentPortalBlueprints[_arenaManager.GetOppositeArena() - 1].transform.position = aPosition + _arenaManager.OffsetOfOpositeArena();

        _tempPortalPosition = aPosition; _tempPortalRotation = aRotation;
    }

    public void SpawnPortalBlueprints()
    {
        for (int i = 0; i < PortalBlueprints.Length; i++)
        {
            _currentPortalBlueprints.Add(Instantiate(PortalBlueprints[i], transform.position, new Quaternion(), GameObject.Find("PortalsHolder").transform));
            RenderPortal(_currentPortalBlueprints[i], false);
        }
    }

    public void DespawnPortalBlueprints()
    {
        foreach (GameObject go in _currentPortalBlueprints) { Destroy(go); }
        _currentPortalBlueprints.Clear();
    }

    public void TrySpawnPortals()
    {
        if (_portalSpawnAllowed)
        {
            for (int i = 0; i < Portals.Length; i++)
            {
                _currentPortals.Add(Instantiate(Portals[i], transform.position, _tempPortalRotation, GameObject.Find("PortalsHolder").transform));
            }
            _currentPortals[_arenaManager.GetCurrentArena() - 1].transform.position = _tempPortalPosition;
            _currentPortals[_arenaManager.GetOppositeArena() - 1].transform.position = _tempPortalPosition + _arenaManager.OffsetOfOpositeArena();
        }
        else { Debug.LogWarning("Portal spawn unsuccessful. No valid spot"); }
    }

    public void DespawnPortals()
    {
        foreach (GameObject go in _currentPortals) { Destroy(go); }
        _currentPortals.Clear();
    }

    public bool CrossArenaBoxCastCheck(Vector3 aOriginPoint, float aAngle, Vector3 aDirection)
    {
        if (BoxCastCheck(aOriginPoint, aAngle, aDirection, Layers_BoxCast) == false &&
        BoxCastCheck((aOriginPoint + _arenaManager.OffsetOfOpositeArena()), aAngle, aDirection, Layers_BoxCast) == false)
        { return false; }
        else return true;
    }

    private bool BoxCastCheck(Vector3 aOriginPoint, float aAngle, Vector3 aDirection, LayerMask aLayers)
    {
        RaycastHit2D rch2d = Physics2D.BoxCast(aOriginPoint, PortalBounds.size, aAngle, aDirection, 0.01f, aLayers);
        BoxCastDrawer.Draw(rch2d, aOriginPoint, PortalBounds.size, aAngle, aDirection);

        if (rch2d.collider == null && UtilityClass.IsPointInsideCollider(aOriginPoint, aDirection, Layers_BoxCast) == false) { return false; }
        else { return true; }
    }

    public void AssessAndTeleport(Collision2D aCollision, int aArena, GameObject aRequestingPortalObj)
    {
        if (UtilityClass.IsLayerInLayerMask(aCollision.gameObject.layer, TeleportObjectsOfLayers))
        {
            Vector3 rotatedPosition = UtilityClass.ReturnRotatedPosition(aCollision.gameObject.transform.position, aRequestingPortalObj.transform.position, 180) + _arenaManager.OffsetOfOpositeArena(aArena);
            Vector2 newVelocity = aCollision.gameObject.GetComponent<Rigidbody2D>().velocity * -1;

            Debug.LogWarning(Mathf.Abs(UtilityClass.GetRelativeDirectionV3(aCollision.transform.position, aRequestingPortalObj).x));

            if (UtilityClass.IsPointInsideCollider(rotatedPosition, Vector3.up, Layers_BoxCast) == false &&
                Physics2D.OverlapCircle(rotatedPosition, aCollision.gameObject.GetComponent<CircleCollider2D>().bounds.size.y / 2, Layers_BoxCast) == null &&
                Mathf.Abs(UtilityClass.GetRelativeDirectionV3(aCollision.transform.position, aRequestingPortalObj).x) < MaxSideAngle
                )
            { 
                aCollision.transform.position = rotatedPosition;
                aCollision.gameObject.GetComponent<Rigidbody2D>().velocity = newVelocity;
                aCollision.gameObject.GetComponent<TeleportationEntityListener>().BroadcastTeleportationEvent(true);
            }
            else aCollision.gameObject.GetComponent<TeleportationEntityListener>().BroadcastTeleportationEvent(false);
        }
    }

}
