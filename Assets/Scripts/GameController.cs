using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Game Over Elements")]
    public static GameController instance;
    [SerializeField] RectTransform gameScreen, gameOverScreen, restartButton;
    [SerializeField] TMP_Text gameScoreText, lifeText, gameOverText, scoreNumber, overScoreText, overScoreNumber;
    [SerializeField]  public int lifes = 2;
    [SerializeField] Image background, heart;
    private bool isAlive = true;
    private int score;
    [SerializeField] int scored;
    private float scoreTimer;
    

    private void OnEnable() => EventManager.SpeedIncrease += SpeedButton;
    private void OnDisable() => EventManager.SpeedIncrease -= SpeedButton;



    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
  
    public void SpeedButton()
    {
        score += 1;
        scored += 4;
    }

   
    
    public void OnHit(int damage)
    {
        lifes-=damage;
        lifeText.text = $"{lifes}";
        StartCoroutine(OnHitRoutine());
        if (lifes <= 0)
        {
            isAlive = false;
            ObsSpawner.instance.OnDeath();
            OnDeath();
            StartCoroutine(GameOverRoutine());
        }
            
    }

    private void OnDeath()
    {
        StartCoroutine(GameOverRoutine());
    }
    
    IEnumerator OnHitRoutine()
    {
        for(float t = 0; t < 1; t+= Time.deltaTime / 0.05f)
        {
            heart.enabled = !heart.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        heart.enabled = true;
    }
    
    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameOverScreen.gameObject.SetActive(true);
        Color initialColor = background.color;
        Color finalColor = background.color;
        finalColor.a = 1;
        gameScreen.gameObject.SetActive(false);
        for(float t = 0; t < 1; t+= Time.deltaTime * 2)
        {
            background.color = Color.Lerp(initialColor, finalColor, t);
            yield return null;
        }
        gameOverText.gameObject.SetActive(true);
        overScoreText.gameObject.SetActive(true);
        overScoreNumber.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        yield return null;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
