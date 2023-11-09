using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResoult : MonoBehaviour
{
    [SerializeField]
    private GameManager m_GameManager;
    [SerializeField]
    private TMP_Text Text;
    [SerializeField]
    private GameObject Resoult;


    // Start is called before the first frame update
    private void Awake()
    {
        m_GameManager.EndGameResult.AddListener(ShowEndGame);
    }

    private void ShowEndGame(bool bDefenderWin)
    {
        if(bDefenderWin)
        {
            Text.text = "DEFENDER WIN!!!";
        }
        else
        {
            Text.text = "KING WIN!!!";
        }
        Resoult.SetActive(true);
    }
    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
