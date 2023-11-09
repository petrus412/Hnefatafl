using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Tile[,] m_tiles;
    private Dictionary<Pawn, Tile> m_pawnsPosition;
    [SerializeField]
    private GridInfo m_gridData;
    //needed for serialization 
    [SerializeField, HideInInspector]
    private List<GameObject> m_tileList;
    private List<Tile> m_alteredTile = new List<Tile>();
    #region Editor
    public void ClearList()
    {
        m_tileList.Clear();
    }
    public void EditorCreateGrid()
    {
        if (m_tileList != null)
        {
            foreach (GameObject s in m_tileList)
            {
                DestroyImmediate(s);
            }
            m_tileList.Clear();
        }
        else
        {
            m_tileList = new List<GameObject>();
        }
        for (int i = 0; i < m_gridData.Size.x; i++)
        {
            for (int j = 0; j < m_gridData.Size.y; j++)
            {
                GameObject NewTile = Instantiate(m_gridData.TilePrefab, GetTileLocation(new Vector2Int(i, j)), Quaternion.identity);
                m_tileList.Add(NewTile);
                NewTile.transform.SetParent(transform);
                NewTile.GetComponent<Tile>().ID = new Vector2Int(i, j);
            }
        }
    }
    public Vector3 GetTileLocation(Vector2Int t_ID)
    {
        return new Vector3(transform.position.x + t_ID.x * (m_gridData.TileSize.x + m_gridData.Offset.x), 0, transform.position.z + t_ID.y * (m_gridData.TileSize.y + m_gridData.Offset.y));
    }
    #endregion

    private void Awake()
    {
        SaveMatrix();
    }
    private void SaveMatrix()
    {
        m_pawnsPosition = new Dictionary<Pawn, Tile>();
        m_tiles = new Tile[m_gridData.Size.x, m_gridData.Size.y];
        foreach (GameObject t_tile in m_tileList)
        {
            Vector2Int ID = t_tile.GetComponent<Tile>().ID;
            m_tiles[ID.x, ID.y] = t_tile.GetComponent<Tile>();
        }
    }
    public Vector2Int GetGridSize()
    {
        return m_gridData.Size;
    }
    public void RefreshTile()
    {
        foreach (Tile tile in m_alteredTile)
        {
            tile.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>("White");
            tile.LeftClicked.RemoveAllListeners();
            tile.RightClicked.RemoveAllListeners();
        }
        m_alteredTile.Clear();
    }
    public void ChangeTileColor(Tile t_tile, string Color)
    {
        t_tile.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>(Color);
        m_alteredTile.Add(t_tile);
    }
    public Tile GetCenterTile()
    {
        return GetTileByID(new Vector2Int(m_gridData.Size.x / 2, m_gridData.Size.y / 2));
    }
    public Tile GetTileByID(Vector2Int t_ID)
    {
        if (t_ID.x < 0 || t_ID.y < 0 || t_ID.x > m_gridData.Size.x - 1 || t_ID.y > m_gridData.Size.y - 1)
            return null;
        return m_tiles[t_ID.x, t_ID.y];
    }
    public Tile GetTileByPawn(Pawn t_pawn)
    {
        if (m_pawnsPosition.TryGetValue(t_pawn, out Tile output))
            return output;
        else
            return null;
    }
    public bool AddNewPawn(Pawn t_pawn, Tile t_tile)
    {
        if (t_tile.GetPawn() == null)
        {
            t_tile.SetPawn(t_pawn);
            m_pawnsPosition.Add(t_pawn, t_tile);
            t_pawn.gameObject.transform.position = t_tile.gameObject.transform.position;
            return true;
        }
        return false;
    }
    public bool MovePawn(Pawn t_pawn, Tile t_tileID)
    {
        Tile StartingTile = GetTileByPawn(t_pawn);
        if (m_pawnsPosition.TryGetValue(t_pawn, out Tile Value) && StartingTile != null && t_tileID != null)
        {
            t_tileID.SetPawn(t_pawn);
            StartingTile.RemovePawn();
            m_pawnsPosition[t_pawn] = t_tileID;
            return true;
        }
        else
        {
            return false;
        }
    }
    public Tile[] GetAdiacentNeighbors(Tile t_tileID)
    {
        List<Tile> Output = new List<Tile>();
        for (int i = 0; i < 4; i++)
        {
            Tile Check = GetTileByID(t_tileID.ID + Utils.FullDirection[i]);
            if (Check != null)
            {
                Output.Add(Check);
            }
        }
        return Output.ToArray();
    }
}
