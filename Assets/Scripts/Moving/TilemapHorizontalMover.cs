using UnityEngine;

public class TilemapHorizontalMover : MonoBehaviour
{
    [Header("Settings")]
    public float moveRange = 3f;  // 좌우 이동 범위 (예: 3이면 왼쪽 3, 오른쪽 3)
    public float speed = 2f;      // 이동 속도

    private Vector3 startPos;
    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Vector3 currentTarget;

    void Start()
    {
        // 시작 위치 기준 좌우 목표 지점 계산
        startPos = transform.position;
        leftPoint = startPos + Vector3.left * moveRange;   // 왼쪽 끝
        rightPoint = startPos + Vector3.right * moveRange; // 오른쪽 끝

        // 처음에는 오른쪽으로 이동 시작
        currentTarget = rightPoint;
    }

    void FixedUpdate() // 물리 이동은 FixedUpdate 권장
    {
        // 현재 위치에서 목표 지점까지 일정 속도로 이동
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.fixedDeltaTime);

        // 목표 지점에 거의 도달했으면 방향 전환
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            // 오른쪽 끝에 왔으면 -> 왼쪽으로 목표 변경
            if (currentTarget == rightPoint)
            {
                currentTarget = leftPoint;
            }
            // 왼쪽 끝에 왔으면 -> 오른쪽으로 목표 변경
            else
            {
                currentTarget = rightPoint;
            }
        }
    }

    // =================================================================
    // 💡 플레이어 탑승 처리 (필수)
    // =================================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 발판 위에 닿으면 자식으로 설정 (같이 이동)
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어가 발판에서 떨어지면 부모 해제
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}