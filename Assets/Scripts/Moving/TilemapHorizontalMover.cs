using UnityEngine;

public class TilemapHorizontalMover : MonoBehaviour
{
    [Header("Settings")]
    public float moveRange = 3f;  // 좌우 이동 범위
    public float speed = 2f;      // 이동 속도

    private Vector3 startPos;
    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Vector3 currentTarget;
    private Rigidbody2D rb; // 💡 Rigidbody 참조 추가

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // 💡 필수: Rigidbody 가져오기
        startPos = transform.position;
        leftPoint = startPos + Vector3.left * moveRange;
        rightPoint = startPos + Vector3.right * moveRange;
        currentTarget = rightPoint;
    }

    void FixedUpdate() // 물리 이동은 FixedUpdate
    {
        // 💡 중요: MovePosition 사용 (Transform 직접 수정 X)
        Vector2 nextPos = Vector2.MoveTowards(rb.position, currentTarget, speed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);

        // 목표 지점 도달 확인
        if (Vector2.Distance(rb.position, currentTarget) < 0.01f)
        {
            if (currentTarget == rightPoint) currentTarget = leftPoint;
            else currentTarget = rightPoint;
        }
    }

    // 플레이어 탑승 처리 (기존 유지)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}