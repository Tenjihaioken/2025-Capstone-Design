using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수

public class FallDetector : MonoBehaviour
{
    // 닿았을 때 실행되는 함수 (Is Trigger 체크 필수)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 닿은 물체가 "Player" 태그를 달고 있는지 확인
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 낙사! 재시작합니다.");
            RestartScene();
        }
    }

    void RestartScene()
    {
        // 현재 활성화된 씬의 이름을 가져와서 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();

        // 만약 페이드 효과(SceneTransition)를 사용 중이라면 아래 주석을 해제하고 쓰세요.
        // SceneTransition.Instance.LoadScene(currentScene.name);

        // 일반 재시작 (SceneTransition이 없을 경우)
        SceneManager.LoadScene(currentScene.name);
    }
}