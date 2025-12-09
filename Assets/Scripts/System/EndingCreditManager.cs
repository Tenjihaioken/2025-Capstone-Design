using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCreditManager : MonoBehaviour
{
    [Header("Scroll Text")]
    public RectTransform scrollContainer;
    public float scrollSpeed = 30f;
    public float endY = 1500f;

    [Header("Audio")]
    public AudioSource creditAudio;

    [Header("Scene")]
    public string mainSceneName = "IntroScene"; // 메인화면 씬 이름

    [Header("Fade")]
    public Animator fadeAnimator;

    private bool isRunning = false;
    private bool isEnding = false;

    private void Start()
    {
        // 시작시 페이드인 실행
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("FadeIn");

        StartCoroutine(RunCredit());
    }


    private void Update()
    {
        // ✅ 아무 키나 / 아무 클릭 시 메인으로 이동
        if (isRunning && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            SkipCredit();
        }
    }

    private IEnumerator RunCredit()
    {
        isRunning = true;

        // FadeIn 끝날 때까지 1초 정도 기다림 (애니메이션 시간에 맞추기)
        yield return new WaitForSeconds(1f);

        // 음악 재생
        if (creditAudio != null)
            creditAudio.Play();

        // 크레딧 스크롤
        while (scrollContainer != null && scrollContainer.anchoredPosition.y < endY)
        {
            scrollContainer.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void SkipCredit()
    {
        if (isEnding) return;
        StopAllCoroutines();
        EndCredit();
    }

    private void EndCredit()
    {
        isEnding = true;

        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("FadeOut");

        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(mainSceneName);
    }
}
