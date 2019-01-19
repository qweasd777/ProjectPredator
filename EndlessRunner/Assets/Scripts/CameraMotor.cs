using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public float animDuration = 2f;
    public Vector3 animOffset = new Vector3(0, 5, 5);

    private float transitionTime;

    private Transform playerTransform;                      // player's Transform
    private Vector3 startOffset;                            // starting cam offset from player    
    private Vector3 moveVector;                             // cam positioning modifiers
    //private CameraAspectRatioScaler camAspectRatioScaler;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //camAspectRatioScaler = GetComponent<CameraAspectRatioScaler>();

        startOffset = transform.position - playerTransform.position;

        transitionTime = 0.0f;
    }

    void LateUpdate()
    {
        moveVector = playerTransform.position + startOffset;

        moveVector.x = 0;
        //moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);         // limit Y offset (might not be needed)

        transform.position = moveVector;

        // TRANSITION ANIM
        /*
        if(transitionTime > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            // Animation Transition
            transform.position = Vector3.Lerp(moveVector + animOffset, moveVector, transitionTime);    // move cam from animOffset pos to follow behind player as per normal
            transitionTime += Time.deltaTime / animDuration;

            //transform.LookAt(lookAt.position + Vector3.up);         // PROBLEM1: cam wont rotate back to origin rot
        }
        */
    }
}
