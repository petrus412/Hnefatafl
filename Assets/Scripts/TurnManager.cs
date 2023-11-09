using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager
{
    public UnityEvent<bool> NextTurn = new UnityEvent<bool>();
    private bool m_kingTurn;
    public bool bKingTurn => m_kingTurn;
    private int m_turnPlayed = 0;
    public int TurnPlayed => m_turnPlayed;

    public void ChangeTurn()
    {
        m_turnPlayed++;
        m_kingTurn = !m_kingTurn;
        NextTurn?.Invoke(m_kingTurn);
    }
}
public static class Utils
{
    public static Vector2Int[] FullDirection =
    {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,-1)
        };
    public static Vector2Int[] Border =
    {
            new Vector2Int(1,0),
            new Vector2Int(1,2),
            new Vector2Int(0,1),
            new Vector2Int(2,1)
        };
}
