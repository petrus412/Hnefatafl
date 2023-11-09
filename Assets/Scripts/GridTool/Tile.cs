using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Tile : MonoBehaviour
{
    private Pawn m_activePawn;
    public bool Tower = false;
    public GameObject TowerPawn;
    [HideInInspector]
    public Vector2Int ID;
    [HideInInspector]
    public UnityEvent<Vector2Int> LeftClicked = new UnityEvent<Vector2Int>();
    [HideInInspector]
    public UnityEvent<Vector2Int> RightClicked = new UnityEvent<Vector2Int>();
    private void OnMouseDown()
    {
        LeftClicked?.Invoke(ID);
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RightClicked?.Invoke(ID);
        }
    }
    public void RemovePawn()
    {
        m_activePawn = null;
    }
    public void SetPawn(Pawn t_pawn)
    {
        m_activePawn = t_pawn;
    }
    public Pawn GetPawn() => m_activePawn;
}
