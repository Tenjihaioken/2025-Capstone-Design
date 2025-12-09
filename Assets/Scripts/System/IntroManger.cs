using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject mainMenuPanel;
    public GameObject slotPanel;
    public GameObject settingsPanel;
    public GameObject quitPopup;

    [Header("Slot UI")]
    public Button[] slotButtons; // 슬롯 버튼 3개 연결
    public TextMeshProUGUI[] slotTexts; // 슬롯 버튼 내부 텍스트 3개 연결

    [Header("Settings UI")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private bool isTitleScreen = true; // 현재 타이틀 화면인지?
    private LocalSaveManager saveManager;

    void Start()
    {
        // 1. 초기 UI 상태 설정
        titlePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        slotPanel.SetActive(false);
        settingsPanel.SetActive(false);
        quitPopup.SetActive(false);

        // 2. 저장 매니저 가져오기 (같은 오브젝트나 GameManager에 있다고 가정)
        saveManager = GetComponent<LocalSaveManager>();
        if (saveManager == null) saveManager = FindFirstObjectByType<LocalSaveManager>();

        // 3. 슬라이더 초기값 설정
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    void Update()
    {
        // 1. 타이틀 화면에서 아무 키나 누르면 메인 메뉴로
        if (isTitleScreen && Input.anyKeyDown)
        {
            isTitleScreen = false;
            titlePanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    // ================= 메인 메뉴 버튼 기능 =================

    public void OnClickNewGame()
    {
        // 새 게임: 데이터 초기화 후 1스테이지로 이동
        // (여기서는 편의상 슬롯 0번에 새 게임을 덮어쓰거나, 그냥 시작하도록 함)
        // 정식으로는 "빈 슬롯 선택 -> 이름 입력 -> 시작" 과정이 필요함

        GameData newData = new GameData();
        newData.currentLevel = 1; // 필요하다면 초기화

        GlobalTransferData.dataToLoad = newData;
        GlobalTransferData.currentSlotIndex = 0;

        // 2. 씬 이동
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene("2D_Stage2");
        else
            SceneManager.LoadScene("2D_Stage2");
    }

    public void OnClickContinue()
    {
        mainMenuPanel.SetActive(false);
        slotPanel.SetActive(true);
        UpdateSlotUI(); // 슬롯 정보 갱신
    }

    public void OnClickSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnClickQuit()
    {
        quitPopup.SetActive(true);
    }

    // ================= 이어하기 (슬롯) 기능 =================

    void UpdateSlotUI()
    {
        for (int i = 0; i < 3; i++)
        {
            int slotIndex = i; // 클로저 문제 방지용 로컬 변수

            // 데이터가 있는지 확인
            if (saveManager.ExistSaveData(slotIndex))
            {
                GameData data = saveManager.LoadGame(slotIndex);
                slotTexts[i].text = $"Slot {i + 1}\n레벨: {data.currentLevel} | 점수: {data.score}";
                slotButtons[i].interactable = true;

                // 버튼 클릭 이벤트 연결 (람다식 활용)
                slotButtons[i].onClick.RemoveAllListeners();
                slotButtons[i].onClick.AddListener(() => LoadGame(slotIndex));
            }
            else
            {
                slotTexts[i].text = $"Slot {i + 1}\n[비어있음]";
                slotButtons[i].interactable = false; // 데이터 없으면 클릭 불가
            }
        }
    }

    void LoadGame(int slotIndex)
    {
        GameData data = saveManager.LoadGame(slotIndex);
        GlobalTransferData.dataToLoad = data;
        GlobalTransferData.currentSlotIndex = slotIndex; // 선택한 슬롯 번호 기억

        string sceneName = "Stage" + data.currentLevel;
        SceneTransition.Instance.LoadScene(sceneName);
    }

    public void CloseSlotPanel()
    {
        slotPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ================= 환경설정 기능 =================

    public void OnBGMSliderChanged(float value)
    {
        SystemManager.Instance.SetVolume(value); // 또는 SetBGMVolume
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ================= 종료 팝업 기능 =================

    public void OnClickYesQuit()
    {
        SystemManager.Instance.ExitGame();
    }

    public void OnClickNoQuit()
    {
        quitPopup.SetActive(false);
    }

}