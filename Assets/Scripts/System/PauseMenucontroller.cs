using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;    // 현재 이 패널 (PausePanel)
    public GameObject settingsPanel; // 설정 창 패널 (SettingPanel)

    private bool isPaused = false;

    void Update()
    {
        // ESC 키를 누르면 일시정지 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // 1. 게임 일시정지 (ESC 키용)
    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // 💡 중요: 게임 시간 정지
    }

    // 2. [Resume] 게임 재개
    public void ResumeGame()
    {
        isPaused = false;

        // 설정창이 열려있다면 같이 닫기
        if (settingsPanel != null) settingsPanel.SetActive(false);

        pausePanel.SetActive(false);
        Time.timeScale = 1f; // 💡 중요: 게임 시간 정상화
    }

    // 3. [Restart] 현재 씬 재시작
    public void RestartStage()
    {
        Time.timeScale = 1f; // 씬 이동 전에 반드시 시간을 되돌려야 함!

        // 현재 활성화된 씬 이름 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        // SceneTransition을 통해 페이드 효과와 함께 재시작
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(currentSceneName);
        else
            SceneManager.LoadScene(currentSceneName); // 없을 경우 대비
    }

    // 4. [Settings] 설정 창 열기
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            // 설정창은 켜고, 일시정지 메뉴는 유지할지 끌지 선택
            // 여기서는 설정창을 띄우는 방식 (Overlay)
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("SettingPanel이 연결되지 않았습니다.");
        }
    }

    // (옵션) 설정 창 닫기 버튼용 함수
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // 5. [Title] 인트로 씬으로 이동
    public void GoToTitle()
    {
        Time.timeScale = 1f; // 이동 전 시간 정상화

        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene("IntroScene");
        else
            SceneManager.LoadScene("IntroScene");
    }
}