using UnityEngine;

public class MainSoldier_Move : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;    // 이동 속도 (적절히 조절)
    public float jumpForce = 15f;   // 점프 힘 (ForceMode2D.Impulse 기준, 10~20 사이 추천)

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f; // 범위 약간 넓힘
    public LayerMask whatIsGround;
    private bool isGrounded;

    // 점프 버퍼링 (키 씹힘 방지)
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 1. 땅 감지
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // 2. 물리 이동 (활공 현상 해결의 핵심)
        // Update에서 받은 입력값을 바탕으로 물리 엔진을 통해 이동시킵니다.
        float h = Input.GetAxisRaw("Horizontal");

        // Y축 속도(낙하)는 유지하면서 X축 속도만 변경
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);
    }

    void Update()
    {
        // 1. 입력 감지 및 방향 전환
        float h = Input.GetAxisRaw("Horizontal");
        if (h < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (h > 0) transform.localScale = new Vector3(1, 1, 1);

        // 애니메이션 설정
        animator.SetFloat("Velocity", Mathf.Abs(h));

        // 2. 점프 버퍼링 로직
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // 3. 점프 실행 (버퍼링된 키가 있고 && 땅에 닿았을 때)
        if (jumpBufferCounter > 0 && isGrounded)
        {
            Jump();
            jumpBufferCounter = 0;
        }

        // 4. (선택 사항) 점프 키를 살짝 눌렀을 때 낮은 점프 구현
        // 키를 떼는 순간 상승 중이라면 속도를 줄임
        if ((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space)) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    void Jump()
    {
        // 기존 속도 초기화 (더블 점프나 가속 방지)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        // 💡 중요: ForceMode2D.Impulse 사용 (순간적인 힘)
        // 숫자가 작아도 강력하게 튀어 오릅니다.
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}