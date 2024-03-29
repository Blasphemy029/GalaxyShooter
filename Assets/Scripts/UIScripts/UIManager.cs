﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Sprite[] livesSprites;
    [SerializeField] private Image livesImage;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text restartText;
    [SerializeField] private Text ammoCount;
    [SerializeField] private Image fuelAmount;

    private GameManager gameManager;
    private Player player;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }

        player = GameObject.Find("Player").GetComponent<Player>();

        restartText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        scoreText.text = "Score: " + 0;
        ammoCount.text = player.currentAmmo.ToString();
    }

    private void Update()
    {
        ammoCount.text = player.currentAmmo.ToString();
        fuelAmount.fillAmount = player.thrusterFuel / player.maxFuel;
    }

    public void UpdateScoreText(int PlayerScore)
    {
        scoreText.text = "Score: " + PlayerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        livesImage.sprite = livesSprites[currentLives];

        if (currentLives <= 0)
        {
            gameManager.GameOver();
            restartText.gameObject.SetActive(true);
            StartCoroutine(GameoverFlickerRoutine());

        }
    }

    IEnumerator GameoverFlickerRoutine()
    {
       while (true)
        {
            gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
