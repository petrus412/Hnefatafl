using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private GridManager m_grid;
    [SerializeField]
    private GameObject KingPrefab;
    [SerializeField]
    private GameObject DefenderPrefab;
    [SerializeField]
    private GameObject TowerPrefab;
    private List<Pawn> m_defender = new List<Pawn>();
    private Pawn m_king;
    [SerializeField, Range(4, 12)]
    private int m_numberOfDefender;
    private float m_defenderCalc;
    private TurnManager m_turns = new TurnManager();
    public TurnManager TurnManager => m_turns;
    private AIMove m_AI = new AIMove();
    private Pawn SelectedPawn;
    [HideInInspector]
    public UnityEvent<bool> EndGameResult = new UnityEvent<bool>();
    private void Awake()
    {
        m_grid = GetComponent<GridManager>();
        m_defenderCalc = m_numberOfDefender;
    }
    public void StartPVPGame(bool Towers)
    {
        m_turns.NextTurn.AddListener(SwitchTurn);
        SpawnPawns(Towers);
        StartDefenderTurn();
    }
    public void StartPVEGame(bool Towers)
    {
        m_turns.NextTurn.AddListener(AITurn);
        SpawnPawns(Towers);
        StartDefenderTurn();
    }
    //FixRange;
    private void SpawnPawns(bool Towers)
    {
        Pawn NewPawn;
        Tile CenterTile = m_grid.GetCenterTile();
        NewPawn = Instantiate(KingPrefab, CenterTile.transform).GetComponent<Pawn>();
        NewPawn.transform.localPosition = new Vector3(0, 0, 0);
        m_grid.AddNewPawn(NewPawn, CenterTile);
        m_king = NewPawn;
        //int ExtraDefender = Mathf.FloorToInt(m_defenderCalc % 4);
        float DifenderInARow = m_defenderCalc / 4;
        for (int i = 0; i < 4; i++)
        {
            Vector2Int Border = CenterTile.ID * Utils.Border[i];
            for (int j = -Mathf.FloorToInt(DifenderInARow / 2); j <= Mathf.FloorToInt(DifenderInARow / 2); j++)
            {
                Vector2Int TileID = Border + new Vector2Int(Utils.FullDirection[i].x * j, Utils.FullDirection[i].y * j);
                Tile DefenderTile = m_grid.GetTileByID(TileID);
                NewPawn = Instantiate(DefenderPrefab, DefenderTile.transform).GetComponent<Pawn>();
                NewPawn.transform.localPosition = new Vector3(0, 0, 0);
                m_grid.AddNewPawn(NewPawn, DefenderTile);
                m_defender.Add(NewPawn);
            }
        }
        if (Towers)
        {
            for (int i = 0; i < 2; i++)
            {
                int x = Random.Range(0, m_grid.GetGridSize().x);
                int y = Random.Range(0, m_grid.GetGridSize().y);
                while (m_grid.GetTileByID(new Vector2Int(x, y)).GetPawn() != null)
                {
                    x = Random.Range(0, m_grid.GetGridSize().x);
                    y = Random.Range(0, m_grid.GetGridSize().y);
                }
                NewPawn = Instantiate(TowerPrefab, m_grid.GetTileByID(new Vector2Int(x, y)).transform).GetComponent<Pawn>();
                NewPawn.transform.localPosition = new Vector3(0, 0, 0);
                m_grid.GetTileByID(new Vector2Int(x, y)).Tower = true;
                m_grid.GetTileByID(new Vector2Int(x, y)).TowerPawn = NewPawn.gameObject;
            }
        }
    }
    private void StartDefenderTurn()
    {
        m_grid.RefreshTile();
        foreach (Pawn defender in m_defender)
        {
            m_grid.GetTileByPawn(defender).LeftClicked.AddListener(PawnSelection);
            m_grid.ChangeTileColor(m_grid.GetTileByPawn(defender), "Selectable");
        }
    }
    private void StartKingTurn()
    {
        SelectedPawn = m_king;
        m_grid.RefreshTile();
        m_grid.ChangeTileColor(m_grid.GetTileByPawn(m_king), "Pawn");
        foreach (Tile tile in m_grid.GetAdiacentNeighbors(m_grid.GetTileByPawn(m_king)))
        {
            if (tile.GetPawn() == null && !tile.Tower)
            {
                m_grid.ChangeTileColor(tile, "Selectable");
                tile.LeftClicked.AddListener(MovePawn);
            }
        }
    }
    private void PawnSelection(Vector2Int t_pawn)
    {
        m_grid.RefreshTile();
        m_grid.ChangeTileColor(m_grid.GetTileByID(t_pawn), "Pawn");
        SelectedPawn = m_grid.GetTileByID(t_pawn).GetPawn();
        m_grid.GetTileByID(t_pawn).RightClicked.AddListener(Back);
        if (!SelectedPawn.Tower)
        {
            foreach (Tile tile in m_grid.GetAdiacentNeighbors(m_grid.GetTileByID(t_pawn)))
            {
                if (tile.GetPawn() == null)
                {
                    m_grid.ChangeTileColor(tile, "Selectable");
                    tile.LeftClicked.AddListener(MovePawn);
                }
            }
        }
        else
        {
            Tile CheckTile;
            for (int i = 0; i < 4; i++)
            {
                CheckTile = m_grid.GetTileByID(t_pawn + Utils.FullDirection[i]);
                if (CheckTile != null && CheckTile.GetPawn() == null && !CheckTile.Tower)
                {
                    m_grid.ChangeTileColor(CheckTile, "Selectable");
                    CheckTile.LeftClicked.AddListener(MovePawn);
                    CheckTile = m_grid.GetTileByID(CheckTile.ID + Utils.FullDirection[i]);
                    if (CheckTile != null && CheckTile.GetPawn() == null && !CheckTile.Tower)
                    {
                        m_grid.ChangeTileColor(CheckTile, "Selectable");
                        CheckTile.LeftClicked.AddListener(MovePawn);
                    }

                }
            }
        }
    }
    private void Back(Vector2Int t_id)
    {
        StartDefenderTurn();
    }
    private void MovePawn(Vector2Int t_end)
    {
        m_grid.RefreshTile();
        Tile EndTile = m_grid.GetTileByID(t_end);
        SelectedPawn.GetComponent<Tweener>().OnComplete.RemoveAllListeners();
        SelectedPawn.transform.parent = null;
        SelectedPawn.GetComponent<Tweener>().SetUp(m_grid.GetTileByPawn(SelectedPawn).transform.position, EndTile.transform.position, 0.2f);
        SelectedPawn.GetComponent<Tweener>().Play();
        SelectedPawn.GetComponent<Tweener>().OnComplete.AddListener(CheckWinCon);
        m_grid.MovePawn(SelectedPawn, m_grid.GetTileByID(t_end));
        if (EndTile.Tower && SelectedPawn != m_king)
        {
            SelectedPawn.Promote();
            EndTile.Tower = false;
            EndTile.TowerPawn.SetActive(false);
        }
        SelectedPawn = null;
    }
    private void CheckWinCon()
    {
        if (CheckDefenderWincon())
        {
            EndGameResult?.Invoke(true);
            return;
        }
        else if (CheckKingWincon())
        {
            EndGameResult?.Invoke(false);
            return;
        }
        m_turns.ChangeTurn();
    }
    private bool CheckDefenderWincon()
    {
        foreach (Tile tile in m_grid.GetAdiacentNeighbors(m_grid.GetTileByPawn(m_king)))
        {
            if (tile.GetPawn() == null && !tile.Tower)
            {
                return false;
            }
        }
        return true;
    }
    private bool CheckKingWincon()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_grid.GetTileByID(m_grid.GetTileByPawn(m_king).ID + Utils.FullDirection[i]) == null)
            {
                return true;
            }
        }
        return false;
    }
    private void SwitchTurn(bool bKingTurn)
    {
        if (bKingTurn)
        {
            StartKingTurn();
        }
        else
        {
            StartDefenderTurn();
        }
    }
    private void AITurn(bool bKingTurn)
    {
        if (bKingTurn)
        {
            if (CheckDefenderWincon())
            {
                EndGameResult?.Invoke(true);
                return;
            }
            Tile Target = m_AI.MoveKing(m_grid, m_king);
            SelectedPawn = m_king;
            MovePawn(Target.ID);
        }
        else
        {
            if (CheckKingWincon())
            {
                EndGameResult?.Invoke(false);
                return;
            }
            StartDefenderTurn();
        }
    }

}

