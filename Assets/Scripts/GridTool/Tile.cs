using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Tile : MonoBehaviour
{
    private Pawn m_activePawn;
    public bool Tower = false;
    public GameObject TowerPawn;
    public Pawn GetPawn() => m_activePawn;
    [HideInInspector]
    public UnityEvent<Vector2Int> TileSelected = new UnityEvent<Vector2Int>();
    [HideInInspector]
    public UnityEvent<Vector2Int> RightClicked = new UnityEvent<Vector2Int>();
    [HideInInspector]
    public Vector2Int ID;
    public void OnMouseDown()
    {
        TileSelected?.Invoke(ID);
    }
    public void RemovePawn()
    {
        m_activePawn = null;
    }
    public void SetPawn(Pawn t_pawn)
    {
        m_activePawn = t_pawn;
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RightClicked?.Invoke(ID);
        }
    }
}
