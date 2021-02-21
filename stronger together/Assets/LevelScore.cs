using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScore : MonoBehaviour
{
    // Start is called before the first frame update
    public Text TimerText;
    public float Timer;
    public GameObject canvas;
    GameOver gameOver;
    // Update is called once per frame
    void Start()
    {
        gameOver = canvas.GetComponent<GameOver>();
    }
    void Update()
    {
        
        TimerText.text = gameOver.minutes.ToString("00") + ":" + gameOver.seconds.ToString("00") + ":" + gameOver.milliseconds.ToString("00");

    }
}
