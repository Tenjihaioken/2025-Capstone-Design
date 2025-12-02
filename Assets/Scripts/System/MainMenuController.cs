using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button continueButton;
    public Button newGameButton;

    [Header("Settings")]
    public string saveFileName = "savefile.json";

    // 저장된 데이터를 임시로 담아둘 변수 (GameManager 의존성 제거)
    private GameData loadedData;
    private string saveFilePath;

    void Start()
    {
        // 경로 설정
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);

        // 1. 저장 파일 존재 여부 확인
        if (File.Exists(saveFilePath))
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    // [이어하기] 버튼 클릭 시
    public void OnClickContinue()
    {
        // 1. 파일에서 직접 데이터 읽어오기 (독립적 기능)
        string json = File.ReadAllText(saveFilePath);
        loadedData = JsonUtility.FromJson<GameData>(json);

        if (loadedData != null)
        {
            Debug.Log($"이어하기 로드 완료: 레벨 {loadedData.currentLevel}, 점수 {loadedData.score}");

            // 2. 저장된 레벨로 이동
            // (주의: 씬 이름 규칙이 "Stage1", "Stage2"라고 가정)
            string sceneToLoad = "Stage" + loadedData.currentLevel;

            // 3. 씬 이동 (페이드 효과 사용)
            SceneTransition.Instance.LoadScene(sceneToLoad);

            // 💡 중요: 로드된 데이터(loadedData)를 게임 씬으로 넘겨주는 처리가 필요합니다.
            // 방법 A: 정적 변수에 저장해두고 게임 씬에서 가져가기 (간단한 방법)
            GlobalTransferData.dataToLoad = loadedData;
        }
        else
        {
            Debug.LogError("데이터 로드 실패");
        }
    }

    // [새 게임] 버튼 클릭 시
    public void OnClickNewGame()
    {
        // 1. 새로운 데이터 생성 (GameManager 초기화 대신 여기서 직접 생성)
        GameData newData = new GameData();
        newData.score = 0;
        newData.playerHealth = 100f;
        newData.currentLevel = 1;
        newData.playTime = 0f;

        Debug.Log("새 게임 데이터 생성 완료");

        // 2. 1스테이지로 이동
        SceneTransition.Instance.LoadScene("Stage1"); // 첫 스테이지 이름

        // 💡 중요: 새 데이터를 게임 씬으로 넘겨줌

    }
}

// 씬 간 데이터 전달을 위한 간단한 정적 클래스 (파일 아래에 추가하거나 별도 파일로 만드세요)
