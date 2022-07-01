using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    [SerializeField] GameObject[] lives;
    [SerializeField] Text score;
    public bool isPaused;
    
    void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
    void Start()
    {
        
    }
    
    public void Pause()
    {
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
    }

    public void Boost()
    {

    }

    public void SetLives(int lives)
    {
        lives = Mathf.Clamp(lives, 0, 3);
        for (int i = 0; i < this.lives.Length; ++i)
        {
            this.lives[i].SetActive(i < lives);
        }
    }

    public void SetScore(int score)
    {
        this.score.text = $"{score}";
    }

   
}
