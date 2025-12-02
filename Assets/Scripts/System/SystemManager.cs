using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    // 싱글톤 패턴
    public static SystemManager Instance;

    [Header("UI References")]
    public GameObject pauseMenuUI; // 일시정지 시 띄울 UI 패널

    [Header("Settings")]
    public bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시작 시 저장된 볼륨 설정 불러오기 (예시)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        AudioListener.volume = savedVolume;
    }

    void Update()
    {
        // ESC 키를 누르면 일시정지 토글
        // (메인 메뉴 씬이 아닐 때만 작동하도록 조건 추가 가능)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // 1. 일시정지 기능
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // 게임 시간 정지

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true); // UI 켜기

        Debug.Log("게임 일시정지");
    }

    // 2. 게임 재개 기능
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 게임 시간 정상화

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false); // UI 끄기

        Debug.Log("게임 재개");
    }

    // 3. 볼륨 조절 기능 (슬라이더와 연결)
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume); // 설정 저장
        PlayerPrefs.Save();
    }

    // 4. 해상도 설정 기능 (드롭다운과 연결)
    public void SetResolution(int width, int height, bool isFullScreen)
    {
        Screen.SetResolution(width, height, isFullScreen);
    }

    // 5. 게임 종료 기능 (버튼과 연결)
    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    // 메인 메뉴로 돌아가기
    public void GoToMainMenu()
    {
        ResumeGame(); // 시간은 다시 흐르게 해두고 이동
        SceneManager.LoadScene("MainMenu"); // 메인 메뉴 씬 이름 입력
    }

    [Header("Audio Sources")]
    public AudioSource bgmSource; // 배경음악 오디오 소스 연결
    public AudioSource[] sfxSources; // 효과음 오디오 소스들 (선택)

    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null) bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // 모든 효과음 소스 볼륨 조절 (혹은 AudioMixer 사용 권장)
        // 여기서는 간단히 전역 설정 값만 저장한다고 가정
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

}