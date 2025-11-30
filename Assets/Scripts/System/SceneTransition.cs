using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    public Image fadeImage; // 검은색 패널 연결
    public float fadeDuration = 1.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동해도 파괴 안 됨
        }
        else Destroy(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        // 1. Fade Out (투명 -> 검정)
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        // 2. 씬 로딩
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.5f);
        // 3. Fade In (검정 -> 투명)
        while (t > 0)
        {
            t -= Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0); // 완전히 투명하게
    }
}