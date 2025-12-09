using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer mainMixer; // 1단계에서 만든 믹서를 여기에 연결

    [Header("UI Sliders (Option)")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Awake()
    {
        // 싱글톤 패턴 (씬 이동해도 파괴되지 않음)
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
        // 게임 시작 시 저장된 볼륨 불러오기 (기본값 0.75)
        float bgmVal = PlayerPrefs.GetFloat("BGMVolume", 0.75f);
        float sfxVal = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        // 슬라이더 위치 동기화 (슬라이더가 있다면)
        if (bgmSlider != null) bgmSlider.value = bgmVal;
        if (sfxSlider != null) sfxSlider.value = sfxVal;

        // 실제 소리 적용
        SetBGMVolume(bgmVal);
        SetSFXVolume(sfxVal);
    }

    // BGM 볼륨 조절 (슬라이더에서 호출)
    public void SetBGMVolume(float value)
    {
        // 슬라이더 값(0~1)을 데시벨(-80~0)로 변환 (로그 스케일)
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;

        mainMixer.SetFloat("BGM", volume); // 믹서의 파라미터 이름 "BGM"
        PlayerPrefs.SetFloat("BGMVolume", value); // 저장
    }

    // SFX 볼륨 조절 (슬라이더에서 호출)
    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;

        mainMixer.SetFloat("SFX", volume); // 믹서의 파라미터 이름 "SFX"
        PlayerPrefs.SetFloat("SFXVolume", value); // 저장
    }
}