using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject symbiotePrefab;
    STPlayerInfo playerHealth;
    public bool playerIsDead = false;
    //public Text TimerText;
    public float Timer;
    public int minutes;
    public int seconds;
    public int milliseconds;
    //public static bool GameIsOver = false;
    // Update is called once per frame
    void Start()
    {
        playerHealth = symbiotePrefab.GetComponent<STPlayerInfo>();
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
    }
    void Update()
    {

        if(playerIsDead == false)
        {
            Timer += Time.deltaTime;
            minutes = Mathf.FloorToInt(Timer / 60F);
            seconds = Mathf.FloorToInt(Timer % 60F);
            milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
            //TimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        }
        //if player health is below 5
        if(playerIsDead == true)
        {
            
            EndGame();
        }
        //End Game
    }
    void EndGame()
    {
        //bring up menu
        gameOverUI.SetActive(true);
        //freeze time
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
