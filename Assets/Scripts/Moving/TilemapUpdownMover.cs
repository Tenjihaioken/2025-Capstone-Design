using UnityEngine;

public class TilemapMover : MonoBehaviour
{
    [Header("Settings")]
    public float moveDistance = 3f; // 위아래로 움직일 거리
    public float speed = 2f;        // 움직이는 속도
    public bool startMovingUp = true; // 처음에 위로 갈지 여부

    private Vector3 startPos;

    void Start()
    {
        // 시작 위치 기억
        startPos = transform.position;
    }

    void Update()
    {
        // PingPong 함수를 이용한 왕복 이동 계산
        // Mathf.PingPong(시간 * 속도, 거리) -> 0에서 거리까지 갔다가 0으로 돌아옴
        float newY = Mathf.PingPong(Time.time * speed, moveDistance);

        // 위아래 방향 적용
        if (startMovingUp)
        {
            transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
        }
        else
        {
            transform.position = new Vector3(startPos.x, startPos.y - newY, startPos.z);
        }
    }

    // 플레이어가 탔을 때 같이 움직이게 하기 (필수)
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