using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove
{
    private struct Direction
    {
        public float Distance;
        public int Index;
        public Direction(float t_distance, int t_Index)
        {
            Distance = t_distance;
            Index = t_Index;
        }
    }
    public Tile MoveKing(GridManager t_Grid, Pawn t_King)
    {
        Tile KingTile = t_Grid.GetTileByPawn(t_King);
        Direction[] directions =
        {
            new Direction(Vector2Int.Distance(KingTile.ID, new Vector2Int(t_Grid.GetGridSize().x, KingTile.ID.y)), 0),
            new Direction(Vector2Int.Distance(KingTile.ID, new Vector2Int(0, KingTile.ID.y)), 1),
            new Direction(Vector2Int.Distance(KingTile.ID, new Vector2Int(KingTile.ID.x, t_Grid.GetGridSize().y)), 2),
            new Direction(Vector2Int.Distance(KingTile.ID, new Vector2Int(KingTile.ID.x, 0)), 3)
        };

        Direction temp;
        bool swapped;
        for (int i = 0; i < directions.Length - 1; i++)
        {
            swapped = false;
            for (int j = 0; j < directions.Length - i - 1; j++)
            {
                if (directions[j].Distance > directions[j + 1].Distance)
                {
                    temp = directions[j];
                    directions[j] = directions[j + 1];
                    directions[j + 1] = temp;
                    swapped = true;
                }
            }
            if (swapped == false)
                break;
        }
        for(int i = 0; i < 4; i++)
        {
            Tile Output = t_Grid.GetTileByID(KingTile.ID + Utils.FullDirection[directions[i].Index]);
            if (Output.GetPawn() == null && !Output.Tower)
            {
                return Output;
            }
        }
        return null;
    }
    public Vector2Int MoveDefender()
    {
        return new Vector2Int();
    }
}
