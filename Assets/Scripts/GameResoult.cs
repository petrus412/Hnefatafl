using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResoult : MonoBehaviour
{
    public GameManager m_GameManager;
    public TMP_Text Text;
    public GameObject Resoult;


    // Start is called before the first frame update
    void Awake()
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
