using UnityEngine;

public class GameSaveController : MonoBehaviour
{
    // 싱글톤 (어디서든 접근 가능하게)
    public static GameSaveController Instance;

    private LocalSaveManager saveManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 같은 오브젝트에 있는 LocalSaveManager를 가져옴
            saveManager = GetComponent<LocalSaveManager>();
        }
        else Destroy(gameObject);
    }

    // =========================================================
    // 💾 저장 기능: GameManager의 변수를 직접 가져와서 저장
    // =========================================================
    public void SaveCurrentGame()
    {
        if (saveManager == null || GameManager.Instance == null) return;

        // 1. 저장할 데이터 객체 생성
        GameData dataToSave = new GameData();

        // 2. GameManager의 변수들을 하나씩 꺼내서 담기 (직접 접근)
        dataToSave.score = GameManager.Instance.currentScore;
        dataToSave.playerHealth = GameManager.Instance.playerHealth;
        dataToSave.currentLevel = 1; // 혹은 GameManager.Instance.currentLevel (변수가 있다면)
        dataToSave.playTime = GameManager.Instance.currentPlayTime;

        // (필요하다면 플레이어 위치 등도 여기서 참조해서 저장)
        // GameObject player = GameObject.FindWithTag("Player");
        // if (player != null) { ... }

        // 3. 현재 슬롯 번호 가져오기
        int currentSlot = GlobalTransferData.currentSlotIndex;

        // 4. 파일로 저장
        saveManager.SaveGame(dataToSave, currentSlot);

        Debug.Log($"[GameSaveController] 슬롯 {currentSlot}에 저장 완료 (점수: {dataToSave.score})");
    }

    // =========================================================
    // 📂 로드 기능: 파일에서 읽은 데이터를 GameManager 변수에 덮어쓰기
    // =========================================================
    public void LoadCurrentSlot()
    {
        int currentSlot = GlobalTransferData.currentSlotIndex;

        // 파일이 있는지 확인
        if (saveManager.ExistSaveData(currentSlot))
        {
            // 1. 파일에서 데이터 읽기
            GameData loadedData = saveManager.LoadGame(currentSlot);

            if (loadedData != null && GameManager.Instance != null)
            {
                // 2. GameManager의 변수에 직접 덮어쓰기 (Import)
                GameManager.Instance.currentScore = loadedData.score;
                GameManager.Instance.playerHealth = loadedData.playerHealth;
                GameManager.Instance.currentPlayTime = loadedData.playTime;
                // GameManager.Instance.currentLevel = loadedData.currentLevel;

                Debug.Log($"[GameSaveController] 슬롯 {currentSlot} 로드 완료. 게임 상태가 갱신되었습니다.");

                // ⚠️ 주의: 변수만 바꿨으므로 UI는 아직 그대로일 수 있습니다.
                // UI 갱신을 위해 씬을 다시 로드하거나, GameManager의 이벤트를 억지로 발동시켜야 할 수도 있습니다.
                // 가장 깔끔한 방법은 로드 후 씬을 다시 시작하는 것입니다.
            }
        }
        else
        {
            Debug.LogWarning($"슬롯 {currentSlot}에 저장된 데이터가 없습니다.");
        }
    }
}