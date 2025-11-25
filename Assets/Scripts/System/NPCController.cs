using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Settings")]
    // 💡 변경 1: 하나의 ID가 아니라, 순서대로 출력할 ID 목록을 입력받음
    public int[] dialogueIDs;

    public string nextObjective = "다음 지역으로 이동하세요."; // 대화가 '모두 끝난 후' 갱신될 목표

    private bool isPlayerNear = false;
    private bool isChatting = false; // 현재 대화 중인지 확인하는 변수
    private int currentLineIndex = 0; // 현재 몇 번째 대사를 보여주는지

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            isChatting = false; // 대화 상태 초기화
            currentLineIndex = 0; // 순서 초기화
            PlatformerSceneManager.Instance.CloseDialogue();
        }
    }

    void Update()
    {
        // 플레이어가 근처에 있고 Z키를 누르면
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Z))
        {
            // 1. 대화가 아직 시작되지 않았다면 -> 첫 대사 시작
            if (!isChatting)
            {
                StartConversation();
            }
            // 2. 이미 대화 중이라면 -> 다음 대사로 넘기기
            else
            {
                AdvanceConversation();
            }
        }
    }

    void StartConversation()
    {
        isChatting = true;
        currentLineIndex = 0;

        // 첫 번째 대사 출력
        if (dialogueIDs.Length > 0)
        {
            PlatformerSceneManager.Instance.ShowDialogue(dialogueIDs[0]);
        }
    }

    void AdvanceConversation()
    {
        currentLineIndex++; // 다음 순서로 이동

        // 💡 아직 보여줄 대사가 남아있다면
        if (currentLineIndex < dialogueIDs.Length)
        {
            int nextID = dialogueIDs[currentLineIndex];
            PlatformerSceneManager.Instance.ShowDialogue(nextID);
        }
        // 💡 모든 대사가 끝났다면 -> 대화창 닫기 & 목표 갱신
        else
        {
            EndConversation();
        }
    }

    void EndConversation()
    {
        isChatting = false;
        currentLineIndex = 0;
        PlatformerSceneManager.Instance.CloseDialogue();

        // 대화가 끝난 시점에 목표 갱신
        if (!string.IsNullOrEmpty(nextObjective))
        {
            PlatformerSceneManager.Instance.UpdateObjective(nextObjective);
        }
    }
}