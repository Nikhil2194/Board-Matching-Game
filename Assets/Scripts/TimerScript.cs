using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn ;

   [SerializeField] public TextMeshProUGUI timeText;
    [SerializeField] public TextMeshProUGUI timeUp;
    [SerializeField] GameObject TimeUpObject;
    void Start()
    {
        TimerOn = true;
    }

    private void Awake()
    {
      //  timeText = GetComponent<TextMeshProUGUI>();
    }
  
    void Update()
    {
    if(TimerOn)
        {
            if (TimeLeft >= 0)
            { 
                TimeLeft = TimeLeft - Time.deltaTime;
                UpdateTimer(TimeLeft);
             }
            else
            {
                Debug.Log("Time is Up!");
                TimeLeft = 0;
                TimerOn = false;
                TimeUpObject.SetActive(true);
                timeUp.text = "Your Time is Up!";
                StartCoroutine(RestartGame());
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime = currentTime + 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
       // timeText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2.5f);
        Board.instance.QuitGame();
    }
}
