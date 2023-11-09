using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject GameModeMenu;
    [SerializeField]
    private GameObject NumberOfPlayerMenu;
    [SerializeField]
    private GameObject BackButton;
    [SerializeField]
    private GameManager m_gameManager;
    private bool bPVP;

    private void Awake()
    {
        BackButton.GetComponent<Button>().onClick.AddListener(ShowPlayerSelection);
    }
    private void ShowPlayerSelection()
    {
        NumberOfPlayerMenu.SetActive(true);
        BackButton.SetActive(false);
        GameModeMenu.SetActive(false);
    }
    public void ShowGameModeMenu()
    {
        GameModeMenu.SetActive(true);
        BackButton.SetActive(true);
        NumberOfPlayerMenu.SetActive(false);
    }
    public void HideSelf()
    {
        gameObject.SetActive(false);
    }
    public void StartGameTower()
    {
        if(bPVP)
        {
            m_gameManager.StartPVPGame(true);
        }
        else
        {
            m_gameManager.StartPVEGame(true);
        }
    } 
    public void StartGameStandard()
    {
        if(bPVP)
        {
            m_gameManager.StartPVPGame(false);
        }
        else
        {
            m_gameManager.StartPVEGame(false);
        }
    }
    public void PVP()
    {
        bPVP = true;
    }
    public void PVE()
    {
        bPVP = false;
    }
}
