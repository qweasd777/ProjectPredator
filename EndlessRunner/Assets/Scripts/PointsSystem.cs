using UnityEngine;

public class PointsSystem : MonoBehaviour       // aka GameManager
{
    #region Singleton
    private static PointsSystem _instance;
    public static PointsSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject.FindGameObjectWithTag("UI_MANAGER").AddComponent<PointsSystem>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if(_instance == null)
            _instance = this;
    }

    public void DestroyInstance()
    {
        // TODO: check agn if correct way of implementing destroy
        _instance = null;
        Destroy(this);
    }
    #endregion
    
    public int maxLevel = 10;
    public int pointsToNextLevel = 10;
    public float speedIncrementPerLevel = 1f;
    public bool multiplySpeedIncWithLevel = true;

    private int level = 1;
    private float points = 0.0f;
    private float pointsHighScore = 0.0f;

    private MenuManager menuManager;
    private PlayerController playerController;

    void Start()
    {
        menuManager = GameObject.FindGameObjectWithTag("UI_MANAGER").GetComponent<MenuManager>();
        playerController = GetComponent<PlayerController>();

        pointsHighScore = PlayerPrefs.GetFloat("Highscore");
    }

    void Update()
    {
        if(playerController.isDead)
            return;

        points += Time.deltaTime;                        // simple update of score over time (1 pt / sec)

        if(points >= pointsToNextLevel)
            LevelUp();

        menuManager.UpdatePointsUI(points);
    }

    void LevelUp()
    {
        if(level == maxLevel)
            return;

        level++;
        pointsToNextLevel *= 2;                         // Lvl 1 = 20 pts, Lvl 2 = 40 pts, Lvl 3 = 80 pts, so on...

        float speedIncrement = multiplySpeedIncWithLevel ? speedIncrementPerLevel * level : speedIncrementPerLevel;

        playerController.IncreaseSpeed(speedIncrement);
    }

    public void OnPlayerDeath()
    {
        menuManager.ToggleMenuActive(points);
    }

    // TODO: 
    // 1) increase score only when after player passes obstacle
}
