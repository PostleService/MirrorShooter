using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArenaManager : MonoBehaviour
{
    public TilemapRenderer Arena1;
    public TilemapRenderer Arena2;

    [Range(1, 2)] public int CurrentArena = 1;

    private Vector3 _offset = Vector3.zero;

    private void OnEnable()
    {
        PlayerScript.OnPlayerTP += ChangeCurrentArena;
    }
    private void OnDisable()
    {
        PlayerScript.OnPlayerTP -= ChangeCurrentArena;
    }

    private void Awake()
    {
        _offset = UtilityClass.GetArenaOffset(Arena1, Arena2);
    }

    public Vector3 GetArenaOffset() { return _offset; }

    public Vector3 OffsetOfOpositeArena()
    {
        if (CurrentArena == 1) return _offset;
        else return new Vector3(_offset.x * -1, _offset.y, 0);
    }

    public Vector3 OffsetOfOpositeArena(int aInt)
    {
        if (aInt == 1) return _offset;
        else return new Vector3(_offset.x * -1, _offset.y, 0);
    }

    public int GetCurrentArena() { return CurrentArena; }

    public void ChangeCurrentArena()
    {
        if (CurrentArena == 1) CurrentArena = 2;
        else CurrentArena = 1;
    }

    public int GetOppositeArena()
    {
        if (CurrentArena == 1) return 2;
        else return 1;
    }

}
