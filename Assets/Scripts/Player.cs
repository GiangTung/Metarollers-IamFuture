using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dreamteck.Splines;

public class Player : MonoBehaviour
{
    public static Player instance;
    
    [Header("Move Settings")]
    public float jumpVelocity;
    public float acceleration;
    public float maxSpeed;
    public float downForce;
    public float minSpeedAfterFall;
    public float laneChangingSpeed;
    public float gravityScale;
    public int lane;
    public float y0;
    [SerializeField] float swipeDownVelocity;
    
    [Header("Player States")]
    public bool isGrounded;
    public bool isJumping;
    public bool isDead;
    public bool isPressingJump;
    public bool isFalling;
    public bool isChangingLane;
    private Vector2 direction;
    private Vector2 velocity;
    private Vector2 lastVelocity;
    
    [Header("References")]
    [SerializeField] Transform foot;
    [SerializeField] LayerMask mask;
    [SerializeField] Animator animator;
    [SerializeField] Transform startLane;
    RaycastHit2D hit;
    RaycastHit2D ground;
    GameObject currentSurface;
    
    [SerializeField] SplineFollower splineFollower;




    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void OnDeath()
    {
        VibrationManager.instance.VibrationWithDelay(40, 0.1f);
        isDead = true;
        animator.SetBool("IsDead", true);
    }

    void Start()
    {
        y0 = transform.localPosition.y;
        lane = 1;
        transform.position = new Vector3(transform.position.x, startLane.position.y, transform.position.z);
    }
    
    public void SwipeDown()
    {
        if (isGrounded || isDead) return;
        //rb.velocity = new Vector2(rb.velocity.x, swipeDownVelocity);
        
    }
    
    public void Jump()
    {
        print("e");
        if (isJumping) return;
        isJumping = true;
        animator.SetBool("Jumping", true);
        StartCoroutine(JumpRoutine());
    }

    void Update()
    {
        if (isDead) return;
        //if (Input.GetKeyDown(KeyCode.DownArrow)) SwipeDown();
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.W)) LaneUp(); 
        if (Input.GetKeyDown(KeyCode.S))
        {
            //if(!isGrounded)SwipeDown();
            LaneDown();
        }

        
     
    }


    private void FixedUpdate()
    {
        /*if (isDead) return;
        velocity = rb.velocity;
        lastVelocity = velocity;
        if (isGrounded)
        {
            
            if (isPressingJump)
            {
                animator.SetBool("Jumping", true);
                velocity.y = jumpVelocity;
            }
            else
            {
                velocity = Vector2.ClampMagnitude(velocity + direction * acceleration * Time.fixedDeltaTime, maxSpeed);
            }
        }

        isPressingJump = false;
        Vector2 snapForce = SnapToFloor();
        rb.velocity = velocity + snapForce;*/
    }
    
    public void LaneUp()
    {
        //if (isDead || isPressingJump || isChangingLane || isFalling || !isGrounded) return;
        if (lane == 0) return;
        lane--;
        Vector2 offset = new Vector2(0, GameManaging.instance.lanePos[lane]);
        //StartCoroutine(LaneUpRoutine());
        //splineFollower.offsetModifier.keys[0] = new OffsetModifier.OffsetKey(offset, 0, 1, splineFollower.offsetModifier);

        transform.localPosition = new Vector3(transform.localPosition.x, offset.y, transform.localPosition.z);
    }

    public void LaneDown()
    {
       // if (isDead || isPressingJump || isChangingLane || isFalling || !isGrounded) return;
        if (lane == 2) return;
        lane++;
        Vector2 offset = new Vector2(0, GameManaging.instance.lanePos[lane]);
        transform.localPosition = new Vector3(transform.localPosition.x, offset.y, transform.localPosition.z);

        //splineFollower.offsetModifier.keys[0] = new OffsetModifier.OffsetKey(offset, 0, 1, splineFollower.offsetModifier);
        //StartCoroutine(LaneDownRoutine());
    }

    IEnumerator LaneUpRoutine()
    {
        isChangingLane = true;
        Vector2 pos1 = currentSurface.GetComponent<Street>().laneBegin[lane].position;
        Vector2 pos2 = currentSurface.GetComponent<Street>().laneEnd[lane].position;
        Vector2 finalPos = Vector2.Lerp(pos1, pos2, (transform.position.x - pos1.x) / (pos2.x - pos1.x));
        var distance = (finalPos.y - transform.position.y);
      
        while (distance > 0.01f)
        {
            Debug.DrawLine(pos1, pos2, Color.white);
            Debug.DrawRay(transform.position, Vector2.up * distance, Color.red);
            finalPos = Vector2.Lerp(pos1, pos2, (transform.position.x - pos1.x) / (pos2.x - pos1.x));
            distance = (finalPos.y - transform.position.y);
            yield return new WaitForFixedUpdate();
        }
        isChangingLane = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere((Vector2)transform.position, 0.2f);
    }

    IEnumerator LaneDownRoutine()
    {
        isChangingLane = true;
        Vector2 pos1 = currentSurface.GetComponent<Street>().laneBegin[lane].position;
        Vector2 pos2 = currentSurface.GetComponent<Street>().laneEnd[lane].position;
        Vector2 finalPos = Vector2.Lerp(pos1, pos2, (transform.position.x - pos1.x) / (pos2.x - pos1.x));
        var distance = (finalPos.y - transform.position.y);

        while (distance < -0.01f)
        {
            Debug.DrawLine(pos1, pos2, Color.white);
            Debug.DrawRay(transform.position, Vector2.up * distance, Color.red);
            finalPos = Vector2.Lerp(pos1, pos2, (transform.position.x - pos1.x) / (pos2.x - pos1.x));
            distance = (finalPos.y - transform.position.y);
            yield return new WaitForFixedUpdate();
        }
        isChangingLane = false;
    }
    
    IEnumerator JumpRoutine()
    {
        float y = jumpVelocity;
        float y0 = transform.localPosition.y;
        while (true)
        {
            Vector3 nextPos = transform.localPosition + Vector3.up * y * Time.fixedDeltaTime;
            if (y < 0 && nextPos.y < y0) break;
            transform.localPosition = nextPos;
            y -= gravityScale * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
                
        }
        transform.localPosition = new Vector3(transform.localPosition.x, y0, transform.localPosition.z);
        isJumping = false;
        animator.SetBool("Jumping",false);

    }

    /* private void OnCollisionEnter2D(Collision2D collision)
     {
         if (isDead) return;
         if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
         {
         }
     }*/

}
