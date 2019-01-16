using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed_world = 7f;          // auto endless world movespeed
    public float moveSpeed_player = 7f;
    public float gravity = 10f;                 // simple gravity
    
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
        moveVector.x = Input.GetAxisRaw("Horizontal") * moveSpeed_player;

        // Player Gravity  
        if(pController.isGrounded)
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
        isDead = true;
        pointsSystem.OnPlayerDeath();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // TODO: fix collider such that only if player hits obstacle face on then gameover is run
        //  - currently player can still die if he hits obstacle sideways (should it be alright?)

        if(hit.collider.tag == "Obstacle" && hit.point.z > transform.position.z + pController.radius)
            GameOver();
    }
}
