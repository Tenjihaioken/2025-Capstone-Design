using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Settings")]
    public int dialogueID = 100; // JSON에 적힌 ID와 일치시켜야 함
    public string nextObjective = "다음 지역으로 이동하세요."; // 대화 후 갱신될 목표

    private bool isPlayerNear = false;

    // 트리거 감지 (2D Collider isTrigger 체크 필수)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            // (선택) "Z키를 눌러 대화" 같은 안내 문구 표시 가능
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            PlatformerSceneManager.Instance.CloseDialogue(); // 멀어지면 대화창 닫기
        }
    }

    void Update()
    {
        // 플레이어가 근처에 있고 Z키를 누르면 대화 시작
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Z))
        {
            // 1. 대화 출력
            PlatformerSceneManager.Instance.ShowDialogue(dialogueID);

            // 2. 목표 갱신 (옵션)
            PlatformerSceneManager.Instance.UpdateObjective(nextObjective);
        }
    }
}