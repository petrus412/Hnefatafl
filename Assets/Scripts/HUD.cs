using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TMP_Text NumberOfTurn;
    [SerializeField]
    private TMP_Text TurnOrder;
    [SerializeField]
    private GameManager m_GameManager;
    private TurnManager m_turnManager;

    private void Awake()
    {
        m_turnManager = m_GameManager.TurnManager;
        m_turnManager.NextTurn.AddListener(UpdateText);
        UpdateText();
    }

    private void UpdateText(bool bKingTurn = false)
    {
        NumberOfTurn.text = "Turn number: " + ((m_turnManager.TurnPlayed / 2) + 1 ).ToString();
        TurnOrder.text = m_turnManager.bKingTurn ? "King's Turn" : "Defender's Turn";
    }
}
