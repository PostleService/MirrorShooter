using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class UtilityClass
{
    public static Vector3 GetMousePosition_World(Camera aCurrentCamera)
    {
        return aCurrentCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public static Vector3 GetEndPoint(Vector3 aOriginPoint, Vector3 aDirection, float aDistance)
    { return aOriginPoint + (aDirection * aDistance); }

    public static Vector3 GetDirectionV3(Vector3 aTargetPosition, Vector3 aOriginPoint)
    {
        return GetRotation(aTargetPosition, aOriginPoint) * Vector3.up;
    }

    public static Vector3 GetRelativeDirectionV3(Vector3 aTargetPosition, GameObject aOriginObject)
    {
        GameObject RotatableEntity = new GameObject("RotatableEntity");
        RotatableEntity.transform.parent = aOriginObject.transform;
        RotatableEntity.transform.rotation = GetRotation(aTargetPosition, aOriginObject.transform.position);
        Vector3 relativeRotation = RotatableEntity.transform.localRotation * Vector3.up;
        GameObject.Destroy(RotatableEntity);

        return relativeRotation;
    }

    public static float GetAngle(Vector3 aTargetPosition, Vector3 aOriginalPoint)
    {
        Vector3 direction = (aTargetPosition - aOriginalPoint);
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public static float GetAngleOfCollision(Collision2D aCollision)
    {
        return Vector3.Angle(aCollision.gameObject.GetComponent<Rigidbody2D>().velocity.normalized, aCollision.contacts[0].normal);
    }

    public static Quaternion GetRotation(Vector3 aTargetPosition, Vector3 aOriginPoint)
    {
        float angle = GetAngle(aTargetPosition, aOriginPoint);
        return Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    public static Vector3 ReturnRotatedPosition(Vector3 aRotationObject, Vector3 aRotateAround, double aAngle)
    {
        double radians = (Math.PI / 180) * aAngle;
        double sin = Math.Sin(radians);
        double cos = Math.Cos(radians);

        float translatedX = aRotationObject.x - aRotateAround.x;
        float translatedY = aRotationObject.y - aRotateAround.y;

        double newX = translatedX * cos - translatedY * sin;
        double newY = translatedX * sin + translatedY * cos;

        Vector3 newPosition = new Vector3((float)newX + aRotateAround.x, (float)newY + aRotateAround.y);

        return newPosition;
    }

    public static bool IsLayerInLayerMask(int aLayer, LayerMask aLayerMask)
    { return (aLayerMask == (aLayerMask | 1 << aLayer)); }

    public static Vector3 GetArenaOffset(TilemapRenderer aArena1, TilemapRenderer aArena2)
    {
        aArena1.gameObject.GetComponent<Tilemap>().CompressBounds();
        aArena2.gameObject.GetComponent<Tilemap>().CompressBounds();

        return aArena2.bounds.min - aArena1.bounds.min; ;
    }

    public static bool IsPointInsideCollider(Vector3 aOriginPoint, Vector3 aDirection, LayerMask aLayers)
    {
        int numberOfCollisions = 0;
        Vector3 TempOriginPoint = aOriginPoint;
        bool aHitDetected;
        LayerMask layer = aLayers;

        do
        {
            RaycastHit2D rch2D = Physics2D.Raycast(TempOriginPoint, aDirection, 1000f, layer);
            Debug.DrawRay(TempOriginPoint, aDirection * .5f, Color.red, 0.2f);

            if (rch2D.collider != null)
            {
                numberOfCollisions += 1;
                TempOriginPoint = (Vector3)rch2D.point + (aDirection / 50f);
                layer = (1 << rch2D.collider.gameObject.layer);
                aHitDetected = true;
            }
            else aHitDetected = false;
        } while (aHitDetected == true);

        if (numberOfCollisions % 2 == 0) return false;
        else return true;
    }

}

