using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text roundOverText;
    public UnityEngine.UI.Text restartText;

    static int roundNumber = 1;
    static int marioSocre;
    static int luigiSocre;

    bool roundOver;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(roundOver && Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene("SampleScene");
    }

    public void RoundEnds(Character deadcharacter) // we do not need to make static method cuz we just matter to the fields amounts
    {
        if (!roundOver)
        {
            roundOver = true;

            if (deadcharacter == Character.Luigi)
                marioSocre++;
            else
                luigiSocre++;

            roundOverText.text = (deadcharacter == Character.Luigi ? "Mario" : "Luigi") + " is the winnter of round " + roundNumber + "!";
            scoreText.text = "Score\nMario: " + marioSocre + "\nLuigi: " + luigiSocre;

            restartText.enabled = true;
            roundOverText.enabled = true;
            scoreText.enabled = true;
            roundNumber++;
        }
    }    
}
