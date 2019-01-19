using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed_world = 7f;          // auto endless world movespeed
    public float moveSpeed_player = 7f;
    public float gravity = 10f;                 // simple gravity

    public bool debug_godMode = false;
    public bool isDead { get; private set; }

    private CharacterController pController;
    private CameraMotor cameraMotor;
    private PointsSystem pointsSystem;

    void Start()
    {
        pController = GetComponent<CharacterController>();
        cameraMotor = Camera.main.GetComponent<CameraMotor>();
        pointsSystem = PointsSystem.Instance;

        isDead = false;
    }

    void Update()
    {
        if(isDead)
            return;

        if(Time.timeSinceLevelLoad < cameraMotor.animDuration)
        {
            pController.Move(Vector3.forward * moveSpeed_world * Time.deltaTime);

            return;
        }

        Vector3 moveVector = Vector3.zero;


        // Player Input Movement

//#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        // KEYBOARD CONTROL
        moveVector.x = Input.GetAxisRaw("Horizontal") * moveSpeed_player;

        // MOUSE TOUCH CONTROL  
        if(Input.GetMouseButton(0))
        {
            if(Input.mousePosition.x > Screen.width * 0.5f)     // RIGHT TOUCH MOVEMENT
                moveVector.x = moveSpeed_player;
            else                                                // LEFT TOUCH MOVEMENT
                moveVector.x = -moveSpeed_player;              
        }

//#endif
//#if UNITY_ANDROID || UNITY_IOS
//#endif

        // Player Gravity  
        if (pController.isGrounded)
            moveVector.y = -0.1f;          
        else
            moveVector.y -= gravity;       

        // Endless World Auto Movement
        moveVector.z = moveSpeed_world;


        pController.Move(moveVector * Time.deltaTime);
    }

    public void IncreaseSpeed(float increment, bool modifyAllSpeeds = true)
    {
        if (modifyAllSpeeds)
            moveSpeed_player += increment;

        moveSpeed_world += increment;
    }

    void GameOver()
    {
        if(debug_godMode)
            return;

        isDead = true;
        pointsSystem.OnPlayerDeath();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // TODO: improve collider such that only if player hits obstacle face on then gameover is run
        //  - currently player can still die if he hits obstacle sideways (should it be alright?)
        //  - might want to improve on logic to make it more sense and reasonable

        if(hit.collider.tag == "Obstacle" && hit.point.z > transform.position.z + pController.radius)
            GameOver();
    }
}

// TODO: 
// 1)PLATFORM DEPENDENT CONTROLS
//   - current one is a simple 'hack'