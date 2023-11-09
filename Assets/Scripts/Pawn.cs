using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [HideInInspector]
    private bool bTower = false;
    public bool Tower => bTower;
    [SerializeField, Tooltip("ForDefenderOnly")]
    private GameObject TowerPromotion;

    public void Promote()
    {
        bTower = true;
        TowerPromotion.SetActive(true);
    }
}
