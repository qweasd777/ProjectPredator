using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour    // Menus and UI manager
{
    public GameObject gameOverScreen;

    [Header("MainMenu UI")]
    public Text highScoreText;

    [Header("Game UI")]
    public Text pointsUIText;               // points shown on in-game UI
    public Text finalPointsText;            // points shown on gameOverScreen

    [Header("Debug UI")]
    public bool showFPS = false;
    public Text fpsText;
    public float deltaTime;

    private float pointsHighScore = 0.0f;
       
    void Start()
    {
        pointsHighScore = PlayerPrefs.GetFloat("Highscore");

        if(SceneManager.GetActiveScene().name == "menu")
        {
            // ### ATTACHED SCRIPT CHECKS ###
            if(highScoreText == null)
                Debug.LogWarning("MenuManager script warning... highScoreText is null!");
            else
                highScoreText.text = "Highscore: " + pointsHighScore;

            return;
        }

        // ### ATTACHED SCRIPT CHECKS ###
        if(pointsUIText == null)
            Debug.LogWarning("MenuManager script warning... pointsUIText is null!");
        if(finalPointsText == null)
            Debug.LogWarning("MenuManager script warning... finalPointsText is null!");

        gameOverScreen.SetActive(false);

        if(!showFPS)
            fpsText.gameObject.SetActive(false);
    }

    void Update()
    {
        if(showFPS)
            DisplayFPS();
    }

    void DisplayFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void UpdatePointsUI(float points)
    {
        pointsUIText.text = ((int)points).ToString();
    }

    public void ToggleMenuActive(float points)
    {
        gameOverScreen.SetActive(true);

        if(points > pointsHighScore)
            PlayerPrefs.SetFloat("Highscore", points);

        finalPointsText.text = ((int)points).ToString();
    }

    // #####################
    //      UI BUTTONS
    // #####################
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
