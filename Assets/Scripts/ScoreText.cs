using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private int score;
    private int highScore;
    public TMP_Text cS;
    public TMP_Text hS;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HS", 0);
        hS.text = "HighScore: " + highScore;
        //text = GetComponent<TMPro.TextMeshProUGUI>();
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
    }

    private void OnDestroy()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    private void GameManager_OnCubeSpawned()
    {
        score++;
        cS.text = "Score: " + score;
        if(score >= highScore)
        {
            highScore = score;
            hS.text = "HighScore: " + highScore;
        }
    }
}
