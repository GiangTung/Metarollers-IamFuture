using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;
    public Animator animator;
    [Header("Movement Settings")]
    [SerializeField] float jumpVelocity;
    [SerializeField] float changingLaneDuration = 0.5f;
    public float gravity;
    [SerializeField] Transform[] lanes;

    [Header("On Hit Settings")]
    [SerializeField] float onHitHeight;
    [SerializeField] float onHitFlyTime;
    [SerializeField] float invulnerableTime;
    [HideInInspector] public int currentLane = 1;
    [Header("Player States")]
    public bool isJumping;
    public bool isFalling;
    public bool isChangingLane;
    public bool isHit;
    public bool isInvulnerable;
    public bool isGrinding;
    [SerializeField] bool isMovingRight;
    [SerializeField] SpriteRenderer[] clothes;
    [SerializeField] SpriteRenderer[] skin;
    [Header("Increase Speed Settings")]
    public int speedIncreaseLimit;
    public float speedIncreased;
    [HideInInspector] public int speedIncreasedTimes;
    private Rigidbody2D rb;
    [Header("Raycast Settings")]
    [SerializeField] LayerMask layer;



    public float height;

    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        instance = null;
    }

    private void OnEnable()
    {
        EventManager.SpeedIncrease += SpeedButton;
    }

    private void OnDisable()
    {
        EventManager.SpeedIncrease -= SpeedButton;
    }

    void Start()
    {
        // footPos = new Vector2(footRay.position.x, footRay.position.y);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(5, 0);
        currentLane = 1;
        UpdateOrderInLayer(currentLane);
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, lanes[currentLane].position.y + height);
        animator.SetFloat("Height", height);

        if (Input.GetKeyDown(KeyCode.F))
            animator.SetTrigger("Air Flip");

        if (Input.GetKeyDown(KeyCode.W))
            CallMoveUpRoutine();

        if (Input.GetKeyDown(KeyCode.S))
            CallMoveDownRoutine();

        if (Input.GetKeyDown(KeyCode.A))
            Jump();

    }

    public void OnHit(int damage)
    {
        isHit = true;
        animator.SetBool("IsHit", true);
        GameController.instance.OnHit(damage);
        StartCoroutine(OnHitRoutine());
    }

    public void CallMoveUpRoutine()
    {
        if (isChangingLane || isGrinding) return;
        if (currentLane == 0) return;
        if (isJumping || isHit) return;
        isChangingLane = true;
        StartCoroutine(MoveUpRoutine());
        UpdateOrderInLayer(currentLane - 1);
    }

    public void CallMoveDownRoutine()
    {
        if (isChangingLane || isGrinding) return;
        if (currentLane == 2) return;
        if (isJumping || isHit) return;
        isChangingLane = true;
        StartCoroutine(MoveDownRoutine());
        UpdateOrderInLayer(currentLane + 1);
    }

    private IEnumerator JumpRoutine()
    {
        float v = jumpVelocity;
        yield return new WaitForSeconds(0.2f);
        while (v > 0)
        {
            height += v * Time.deltaTime;
            v -= Time.deltaTime * gravity * gravity;
            yield return null;
        }
        //transform.position = initialPos;
        Fall();
    }

    private IEnumerator FallRoutine()
    {
        float v = 0;
        isFalling = true;
        animator.SetBool("IsFalling", true);
        while (height > 0)
        {
            v -= gravity * Time.deltaTime * gravity;
            height = Mathf.Max(0, height + v * Time.deltaTime);
            yield return null;
        }
        isJumping = isFalling = false;
        animator.SetBool("Jumping", false);
    }
    public void UpdatePosition()
    {
        transform.position = lanes[currentLane].position + Vector3.up * height;
    }

    public void Fall()
    {
        StartCoroutine(FallRoutine());
    }

    private IEnumerator MoveUpRoutine()
    {
        Vector3 startPos = transform.position;
        float yPos = lanes[currentLane - 1].position.y;
        Vector3 finalPos = new Vector3(startPos.x, yPos, startPos.z);

        for (float t = 0; t < 1; t += Time.deltaTime / changingLaneDuration)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, t);
            yield return null;
        }
        currentLane--;
        isChangingLane = false;
        transform.position = finalPos;
    }

    private IEnumerator MoveDownRoutine()
    {
        Vector3 startPos = transform.position;
        float yPos = lanes[currentLane + 1].position.y;
        Vector3 finalPos = new Vector3(startPos.x, yPos, startPos.z);

        for (float t = 0; t < 1; t += Time.deltaTime / changingLaneDuration)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, t);
            yield return null;
        }
        isChangingLane = false;
        currentLane++;
        transform.position = finalPos;
    }
    public void AirTrick()
    {
        animator.SetTrigger("AirTrick");
    }

    private IEnumerator OnHitRoutine()
    {
        VibrationManager.instance.VibrationWithDelay(40, 0);
        isInvulnerable = true;
        Vector2 initialPos = transform.position;
        for (float t = 0; t < 1; t += Time.deltaTime / onHitFlyTime)
        {
            transform.position = initialPos + Vector2.up * Mathf.Sin(t * Mathf.PI) * onHitHeight;
            yield return null;
        }
        transform.position = initialPos;
        isHit = false;
        animator.SetBool("IsHit", false);
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / invulnerableTime)
        {
            for (int i = 0; i < skin.Length; i++)
                skin[i].enabled = !skin[i].enabled;

            for (int i = 0; i < clothes.Length; i++)
                clothes[i].enabled = !clothes[i].enabled;


            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < skin.Length; i++)
        {
            skin[i].enabled = true;
        }

        for (int i = 0; i < clothes.Length; i++)
        {
            clothes[i].enabled = true;
        }

        isInvulnerable = false;
    }

    public void Jump()
    {
        if (isJumping) return;
        isJumping = true;
        animator.SetBool("Jumping", true);
        animator.SetBool("Grinding", false);
        isGrinding = false;
        StartCoroutine(JumpRoutine());
    }

    public void SpeedButton()
    {
        animator.speed += 0.2f;
    }

    public void TriggerTrick(string s)
    {
        //animator.SetTrigger("_ShootDuck");
        print(s);
        print("Manobra");
        animator.SetTrigger(s);
    }

    private void UpdateOrderInLayer(int lane)
    {
        for (int i = 0; i < clothes.Length; i++)
        {
            clothes[i].sortingOrder = 1 + lane * 10;
        }

        for (int i = 0; i < skin.Length; i++)
        {
            skin[i].sortingOrder = lane * 10;
        }
    }

    public void SetGrindingTrue(float height)
    {
        StopAllCoroutines();
        animator.SetBool("IsFalling", false);
        this.height = height;
        print("Starting Grind");
        animator.SetBool("Jumping", false);
        isGrinding = true;
        isJumping = false;
        animator.SetBool("Grinding", true);
    }

    public void SetGrindingFalse()
    {
        Fall();
        isGrinding = false;
        animator.SetBool("Grinding", false);
    }

    public void SetDeathTrue()
    {
        animator.SetBool("IsDead", true);
        animator.SetTrigger("Dead");
    }
}
