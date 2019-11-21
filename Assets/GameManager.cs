using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;

    private void Update()
    {
        if (Input.GetButtonDown("Restart") && isGameOver == true)
        {
            SceneManager.LoadScene(1); // Current Game scene
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
