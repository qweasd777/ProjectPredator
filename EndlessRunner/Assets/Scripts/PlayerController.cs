using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float sceneMoveSpeed = 7f;          // auto endless world movespeed
    public float playerMoveSpeed = 7f;
    public float playerJumpForce = 10f;
    public float playerFallMultiplier = 2f;
    public float gravity = 14f;                // simple gravity

    public float touch_speedReduction = 0.25f;

    public bool debug_devMode = false;
    public bool debug_godMode = false;
    public bool isDead { get; private set; }

    private bool runPlayerAnimBounce = false;
    private float vertical_vel;
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
        if(debug_devMode)
            DebugInput();

        //#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //#if UNITY_ANDROID || UNITY_IOS
        //#endif
        //#endif

        if (isDead)
            return;

        if(Time.timeSinceLevelLoad < cameraMotor.animDuration || runPlayerAnimBounce)
        {
            // DO ANIM STUFF HERE,
            // DONT LET PLAYER MOVE SLIME
            VerticalVelUpdate(false);

            // ### SIMPLE 'BOUNCE' ANIM ###
            if(pController.isGrounded)
            {
                if(!runPlayerAnimBounce)
                { 
                    runPlayerAnimBounce = true;
                    vertical_vel = playerJumpForce;
                }
                else
                    runPlayerAnimBounce = false;

            }

            Vector3 animMoveVector = Vector3.up * vertical_vel + Vector3.forward * sceneMoveSpeed;  // vert movement + fwd movement

            pController.Move(animMoveVector * Time.deltaTime);

            return;
        }

        Vector3 moveVector = Vector3.zero;

        moveVector.x = Input.GetAxisRaw("Horizontal") * playerMoveSpeed;
        
        if(Input.GetMouseButton(0))
        {
            if(Input.mousePosition.x > Screen.width * 0.5f)     // RIGHT TOUCH MOVEMENT
                moveVector.x = playerMoveSpeed * touch_speedReduction;
            else                                                // LEFT TOUCH MOVEMENT
                moveVector.x = -playerMoveSpeed * touch_speedReduction;              
        }

        VerticalVelUpdate();

        moveVector.y = vertical_vel;
        moveVector.z = sceneMoveSpeed;

        pController.Move(moveVector * Time.deltaTime);
    }

    void VerticalVelUpdate(bool letPlayerJump = true)
    {
        if(pController.isGrounded)
        {
            vertical_vel = -gravity * Time.deltaTime;

            if(letPlayerJump && Input.GetKeyDown(KeyCode.Space))
                vertical_vel = playerJumpForce;
        }
        else
        {
            vertical_vel -= gravity * playerFallMultiplier * Time.deltaTime;
        }
    }

    void GameOver()
    {
        if(debug_godMode)
        {
            print("player just died...");
            return;
        }

        isDead = true;
        pointsSystem.OnPlayerDeath();
    }

    void DebugInput()
    {
        if(Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IncreaseSpeed(float increment, bool modifyAllSpeeds = true)
    {
        if (modifyAllSpeeds)
            playerMoveSpeed += increment;

        sceneMoveSpeed += increment;
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
// 2) JUMP FIX
//   - jumping mechanic may give problems when player is too fast