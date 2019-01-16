using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour    // Menus and UI manager
{
    public GameObject gameOverScreen;

    public Text pointsUIText;               // points shown on in-game UI
    public Text finalPointsText;            // points shown on gameOverScreen

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "menu")
            return;

        gameOverScreen.SetActive(false);

        if (pointsUIText == null)
            Debug.LogWarning("GameOverMenu script warning... pointsUIText is null!");

        if (finalPointsText == null)
            Debug.LogWarning("GameOverMenu script warning... finalScoreText is null!");
    }

    public void UpdatePointsUI(float points)
    {
        pointsUIText.text = ((int)points).ToString();
    }

    public void ToggleMenuActive(float points)
    {
        finalPointsText.text = ((int)points).ToString();

        gameOverScreen.SetActive(true);
    }

    public void btnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void btnLoadMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void btnLoadMenuScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
