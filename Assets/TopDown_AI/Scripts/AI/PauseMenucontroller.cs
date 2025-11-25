using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;    // 일시정지 메뉴 패널
    public GameObject settingsPanel; // 환경설정 패널

    [Header("Settings")]
    public string mainMenuSceneName = "Main"; // 메인 메뉴 씬 이름 (정확히 적어야 함)
    private bool isPaused = false;

    void Update()
    {
        // ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 환경설정 창이 켜져있다면 그것만 끔
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            // 이미 일시정지 상태라면 해제 (Resume)
            else if (isPaused)
            {
                ResumeGame();
            }
            // 게임 중이라면 일시정지 (Pause)
            else
            {
                PauseGame();
            }
        }
    }

    // 1. 일시정지 기능 (ESC 누를 때 호출)
    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // 메뉴창 켜기
        Time.timeScale = 0f;        // 💡 게임 시간 정지 (물리, 애니메이션 멈춤)
    }

    // 2. 일시정지 해제 (버튼 연결용)
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // 메뉴창 끄기
        Time.timeScale = 1f;         // 💡 게임 시간 정상화
    }

    // 3. 스테이지 재시작 (버튼 연결용)
    public void RestartStage()
    {
        Time.timeScale = 1f; // 씬 이동 전에는 반드시 시간을 다시 흐르게 해야 함!
        // 현재 활성화된 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 4. 환경설정 열기 (버튼 연결용)
    public void OpenSettings()
    {
        settingsPanel.SetActive(true); // 설정창 켜기
        pausePanel.SetActive(false);   // 일시정지 메뉴는 잠시 숨김 (선택 사항)
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);    // 다시 일시정지 메뉴 보이기
    }

    // 5. 메인 메뉴로 이동 (버튼 연결용)
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // 시간 정상화 필수
        SceneManager.LoadScene(mainMenuSceneName);
    }
}