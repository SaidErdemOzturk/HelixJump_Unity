using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CanvasController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI myLevel;
    public TMPro.TextMeshProUGUI nextLevel;
    public Button nextLevelButton;
    public Button startButton;
    public Slider slider;
    public TMPro.TextMeshProUGUI myScore;
    public TMPro.TextMeshProUGUI bestScore;
    public int score;
    private BallController ball;
    void Start()
    {
        startButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(false);
        SetText();

        ball = GameObject.FindObjectOfType<BallController>();

        bestScore.SetText("Best Score: "+PlayerPrefs.GetInt("Best Score").ToString());
    }



    public void StartGame()
    {
        ball.GetComponent<Rigidbody>().isKinematic = false;
        startButton.gameObject.SetActive(false);
    }

    public void AddScore(int score)
    {
        this.score =this.score+ score;
        myScore.SetText(this.score.ToString());
        if(this.score>PlayerPrefs.GetInt("Best Score"))
        {
            PlayerPrefs.SetInt("Best Score", this.score);
            bestScore.SetText(this.score.ToString());
        }
    }



    public void NextLevel()
    {
        startButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(false);
        Time.timeScale = 1.0F;
        SetText();
        RestartGame();
    }

    public void NextLevelSetActiveButton()
    {
        nextLevelButton.gameObject.SetActive(true);
    }


    private void SetText()
    {
        myLevel.SetText(PlayerPrefs.GetInt("level").ToString());
        nextLevel.SetText((PlayerPrefs.GetInt("level") + 1).ToString());
    }

    public void UpdateSlider(int ringCount,int myRing)
    {
        slider.maxValue=ringCount;
        slider.value = myRing;
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
