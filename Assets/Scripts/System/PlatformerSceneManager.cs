using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용
using System.Collections.Generic;
using System.IO;

// JSON 데이터를 받기 위한 클래스 구조 정의
[System.Serializable]
public class DialogueData
{
    public int id;
    public string name;
    public string text;
}

[System.Serializable]
public class DialogueList
{
    public DialogueData[] dialogues;
}

public class PlatformerSceneManager : MonoBehaviour
{
    public static PlatformerSceneManager Instance;

    [Header("1. JSON Dialogue System")]
    public TextAsset dialogueJsonFile; // 인스펙터에 JSON 파일 할당
    public GameObject dialoguePanel;   // 대화창 UI 패널
    public TextMeshProUGUI nameText;   // 화자 이름 텍스트
    public TextMeshProUGUI contentText;// 대화 내용 텍스트
    private Dictionary<int, DialogueData> dialogueDictionary; // 빠른 검색을 위한 딕셔너리

    [Header("2. Objective System")]
    public TextMeshProUGUI objectiveText; // 목표 표시 텍스트

    [Header("3. Minimap System")]
    public RawImage minimapUI; // 우상단 미니맵 UI

    void Awake()
    {
        Instance = this;
        LoadDialogueData(); // 게임 시작 시 JSON 데이터 로드
    }

    void Start()
    {
        // 초기 설정
        if (dialoguePanel != null) dialoguePanel.SetActive(false); // 대화창 숨김
        if (minimapUI != null) minimapUI.gameObject.SetActive(true); // 미니맵 켜기

        // 초기 목표 설정 (예시)
        UpdateObjective("현재 목표: 정보상에게 임무를 수령하세요.");
    }

    // ==========================================
    // 1. 대화 시스템 (JSON)
    // ==========================================
    void LoadDialogueData()
    {
        if (dialogueJsonFile == null)
        {
            Debug.LogError("JSON 파일이 할당되지 않았습니다!");
            return;
        }

        // JSON 파싱
        DialogueList dataList = JsonUtility.FromJson<DialogueList>(dialogueJsonFile.text);
        dialogueDictionary = new Dictionary<int, DialogueData>();

        foreach (var data in dataList.dialogues)
        {
            dialogueDictionary.Add(data.id, data);
        }
        Debug.Log($"대화 데이터 로드 완료: {dialogueDictionary.Count}개");
    }

    // ID를 통해 대화 출력 (NPC가 이 함수를 호출)
    public void ShowDialogue(int id)
    {
        if (dialogueDictionary.ContainsKey(id))
        {
            dialoguePanel.SetActive(true); // 패널 켜기
            DialogueData data = dialogueDictionary[id];

            nameText.text = data.name;
            contentText.text = data.text;

            // 대화 중에는 플레이어 이동을 막거나 시간을 멈출 수 있음
            // Time.timeScale = 0; 
        }
        else
        {
            Debug.LogWarning($"ID {id}에 해당하는 대화가 없습니다.");
        }
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        // Time.timeScale = 1; // 시간 재개
    }

    // ==========================================
    // 2. 목표 표시 시스템
    // ==========================================
    public void UpdateObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "- " + newObjective;

            // (선택) 목표 갱신 시 깜빡이는 효과 등을 줄 수 있음
        }
    }
}