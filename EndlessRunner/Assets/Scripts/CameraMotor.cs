using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform startTransform;                      // cam's first start transform focus before anim transition
    public float animDuration = 2f;
    public Vector3 animOffset = new Vector3(0, 5, 5);

    private float transitionTime;
    private Vector3 startOffset;                            // starting cam offset from player    
    private Vector3 moveVector;                             // cam positioning modifiers
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if(startTransform == null)
            startTransform = player;

        startOffset = transform.position - startTransform.position;

        transitionTime = 0.0f;
    }

    void Update()
    {
        moveVector = player.position + startOffset;

        moveVector.x = 0;                                       // use lerp if you want to remove this

        // TRANSITION ANIM
        if(transitionTime > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            // Animation Transition
            transform.position = Vector3.Lerp(moveVector + animOffset, moveVector, transitionTime);    // move cam from animOffset pos to follow behind player as per normal
            transitionTime += Time.deltaTime / animDuration;

            //transform.LookAt(startTransform.position + Vector3.up);         // PROBLEM1: cam wont rotate back to origin rot
        }
    }
}
