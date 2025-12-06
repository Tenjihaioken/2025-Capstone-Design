using UnityEngine;

public class TilemapMover : MonoBehaviour
{
    [Header("Settings")]
    public float moveDistance = 3f; // 위아래로 움직일 거리
    public float speed = 2f;        // 움직이는 속도
    public bool startMovingUp = true; // 처음에 위로 갈지 여부

    private Vector3 startPos;
    private Vector3 endPoint;   // 이동할 끝 지점
    private Vector3 originPoint; // 원래 시작 지점
    private Vector3 currentTarget;

    private Rigidbody2D rb; // 💡 Rigidbody 참조 추가

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // 💡 필수: Rigidbody 가져오기
        startPos = transform.position;
        originPoint = startPos;

        // 시작 설정에 따라 목표 지점 계산
        if (startMovingUp)
        {
            endPoint = startPos + Vector3.up * moveDistance;
        }
        else
        {
            endPoint = startPos + Vector3.down * moveDistance;
        }

        // 첫 목표 설정
        currentTarget = endPoint;
    }

    void FixedUpdate() // 💡 물리 이동은 FixedUpdate에서 처리
    {
        if (rb == null) return;

        // 1. Rigidbody를 이용해 다음 위치 계산 및 이동
        Vector2 nextPos = Vector2.MoveTowards(rb.position, currentTarget, speed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);

        // 2. 목표 지점에 거의 도달했으면 방향 전환
        if (Vector2.Distance(rb.position, currentTarget) < 0.01f)
        {
            if (currentTarget == endPoint)
            {
                currentTarget = originPoint; // 다시 원래 위치로
            }
            else
            {
                currentTarget = endPoint; // 끝 지점으로
            }
        }
    }

    // =================================================================
    // 플레이어 탑승 처리 (기존 유지)
    // =================================================================
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