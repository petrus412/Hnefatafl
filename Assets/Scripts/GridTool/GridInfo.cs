using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid/GridData")]
public class GridInfo : ScriptableObject
{
    public Vector2Int Size;
    public Vector2 Offset;
    public Vector2 TileSize;
    public GameObject TilePrefab;
}
