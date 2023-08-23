using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Managers")]
    private PortalManager _portalManager;

    private Rigidbody2D _rigidbody2D;
    [HideInInspector] public Vector2 MovementDirection = Vector2.zero;

    [Header("Player Details")]
    public float PlayerMovementSpeed = 5f;
    public float PlayerMaxSpeed = 10f;

    [Header("Portal")]
    public float MaxPortalSpawnDistance = 5f;
    public LayerMask Layers_RayCast;
    private bool _portalPlacementInitiated = false;

    [Header("Projectile")]
    public GameObject GunBarrel;
    public GameObject Projectile;
    public float ProjectileSpeed = 5f;

    public delegate void TPHandlerPlayer();
    public static event TPHandlerPlayer OnPlayerTP;
        
    private void OnEnable()
    {
        PlayerControls.OnMovementDirectionChange += ChangeCurrentDirection;
        PlayerControls.OnShootButton += SpawnProjectile;
        PlayerControls.OnPortalButton += PortalPlacementDirectives;
        TeleportationEntityListener.OnTeleportationInstance += ReactToTeleportationEvent;
    }

    private void OnDisable()
    {
        PlayerControls.OnMovementDirectionChange -= ChangeCurrentDirection;
        PlayerControls.OnShootButton -= SpawnProjectile;
        PlayerControls.OnPortalButton -= PortalPlacementDirectives;
        TeleportationEntityListener.OnTeleportationInstance -= ReactToTeleportationEvent;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _portalManager = GameObject.Find("PortalManager").GetComponent<PortalManager>();
    }

    private void Update()
    {
        RotateTowardsMouse();
        LimitMovementSpeed();
        MovePlayer(MovementDirection);

        PortalPrePlacement();

    }

    #region Movement
    private void LimitMovementSpeed()
    { _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, PlayerMaxSpeed); }

    private void ChangeCurrentDirection(Vector2 aDirection)
    { MovementDirection = aDirection; }

    private void MovePlayer(Vector2 aDirection)
    {
        _rigidbody2D.AddForce(aDirection * PlayerMovementSpeed, ForceMode2D.Force);
    }

    private void RotateTowardsMouse()
    {
        transform.rotation = UtilityClass.GetRotation(UtilityClass.GetMousePosition_World(Camera.main), transform.position);
    }
    #endregion Movement

    private void SpawnProjectile()
    {
        GameObject projectile = Instantiate(Projectile, GunBarrel.transform.position, new Quaternion(), null);
        projectile.GetComponent<Rigidbody2D>().AddForce(
            (UtilityClass.GetDirectionV3(GunBarrel.transform.position, transform.position) * ProjectileSpeed), ForceMode2D.Impulse);
    }

    #region PortalDirectives

    private void PortalPlacementDirectives(bool aStartStop)
    {
        _portalPlacementInitiated = aStartStop;
        
        if (aStartStop == true) 
        {
            _portalManager.DespawnPortals();
            _portalManager.SpawnPortalBlueprints();
            _portalPlacementInitiated = true; 
        }
        
        else 
        {
            _portalManager.DespawnPortalBlueprints();
            _portalManager.TrySpawnPortals();
            _portalPlacementInitiated = false; 
        }
    }

    private void PortalPrePlacement()
    {
        if (_portalPlacementInitiated)
        {
            float OriginalHitDistance = Vector3.Distance(transform.position, RayCastToObstacle());
            float StepDistance = 0.1f;

            float TempDistance = OriginalHitDistance;
            for (float Dist = OriginalHitDistance; Dist >= 0; Dist -= StepDistance)
            {
                Vector3 endPoint = UtilityClass.GetEndPoint(transform.position, UtilityClass.GetDirectionV3(GunBarrel.transform.position, transform.position), TempDistance);
                Vector3 direction = UtilityClass.GetDirectionV3(GunBarrel.transform.position, transform.position);
                float angle = UtilityClass.GetAngle(GunBarrel.transform.position, transform.position) - 90;
                bool result = _portalManager.CrossArenaBoxCastCheck(endPoint, angle, direction);

                if (result == false) {
                    _portalManager.SetAllowSpawn(true);
                    break; }
                else {
                    _portalManager.SetAllowSpawn(false);
                    TempDistance -= StepDistance; }
            }

            bool render = false;
            if (TempDistance > 0) render = true;

            Vector3 renderPoint = UtilityClass.GetEndPoint(transform.position, UtilityClass.GetDirectionV3(GunBarrel.transform.position, transform.position), TempDistance);
            _portalManager.RenderPortalBlueprints(render, renderPoint, transform.rotation);

        }
    }

    public void ReactToTeleportationEvent(bool aSuccessful, TeleportationEntityListener aTPListener)
    {
        if (aTPListener == gameObject.GetComponent<TeleportationEntityListener>())
        {
            if (aSuccessful == true) 
            { OnPlayerTP?.Invoke(); }
            else
            { }
        }
    }

    #endregion PortalDirectives

    #region Casts

    private Vector3 RayCastToObstacle()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast
            (transform.position, UtilityClass.GetDirectionV3(GunBarrel.transform.position, transform.position), MaxPortalSpawnDistance, Layers_RayCast);

        if (raycastHit.collider != null) { return raycastHit.point; }
        else { return ((UtilityClass.GetDirectionV3(GunBarrel.transform.position, transform.position) * MaxPortalSpawnDistance) + transform.position); }
    }

    #endregion Casts

}
