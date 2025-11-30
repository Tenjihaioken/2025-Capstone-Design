using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText, scoreTextBG;
    public GameObject restartMessage, knifeSelector, gunSelector, endSection;
    int currentScore = 0;
    static GameManager myslf;
    public bool gameOver = false;
    int enemyCount;

    // 🔊 적 무기 사운드
    public AudioSource audioSource;
    public AudioClip enemyRifleClip;
    public AudioClip enemyShotgunClip;
    public AudioClip enemyKnifeClip;
    // 🔊 적 죽는 소리
    public AudioClip enemyGoreClip;      // 피 튀기는 소리
    public AudioClip enemyDeathMoanClip; // 죽는 신음

    public AudioClip weaponSwapClip;  // 🔊 무기 변경 효과음

    void Awake()
    {
        myslf = this;
        SelectWeapon(PlayerWeaponType.KNIFE);
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public static void AddScore(int pointsAdded)
    {
        myslf.currentScore += pointsAdded;
        myslf.scoreText.text = myslf.currentScore.ToString();
        myslf.scoreTextBG.text = myslf.currentScore.ToString();
        myslf.scoreText.transform.localScale = Vector3.one * 2.5f;
        iTween.Stop(myslf.scoreText.gameObject);
        iTween.ScaleTo(myslf.scoreText.gameObject, iTween.Hash(
            "scale", Vector3.one, "time", 0.25f, "delay", 0.1f, "easetype", iTween.EaseType.spring));
    }

    public static void RegisterPlayerDeath()
    {
        myslf.restartMessage.SetActive(true);
        myslf.restartMessage.transform.localScale = Vector3.one * 2.0f;
        iTween.Stop(myslf.restartMessage.gameObject);
        iTween.ScaleTo(myslf.restartMessage, iTween.Hash(
            "scale", Vector3.one, "time", 0.5f, "delay", 0.1f, "easetype", iTween.EaseType.spring));
        myslf.gameOver = true;
    }

    public static void SelectWeapon(PlayerWeaponType weaponType)
    {
        switch (weaponType)
        {
            case PlayerWeaponType.KNIFE:
                myslf.knifeSelector.SetActive(true);
                myslf.gunSelector.SetActive(false);
                break;
            case PlayerWeaponType.PISTOL:
                myslf.knifeSelector.SetActive(false);
                myslf.gunSelector.SetActive(true);
                break;
        }

        // 🔊 무기 변경 소리 재생
        PlayWeaponSwapSound(2.0f);
    }

    public static void AddToEnemyCount()
    {
        myslf.enemyCount++;
    }

    public static void RemoveEnemy()
    {
        myslf.enemyCount--;
        if (myslf.enemyCount <= 0)
        {
            myslf.endSection.SetActive(true);
        }
    }

    // 🔫 적 총기 소리 재생
    public enum EnemyWeaponType { RIFLE, SHOTGUN }

    public static void PlayEnemyGunShot(Vector3 position, EnemyWeaponType weaponType, float volume = 1.0f)
    {
        AudioClip clipToPlay = null;
        switch (weaponType)
        {
            case EnemyWeaponType.RIFLE:
                clipToPlay = myslf.enemyRifleClip;
                break;
            case EnemyWeaponType.SHOTGUN:
                clipToPlay = myslf.enemyShotgunClip;
                break;
        }

        if (clipToPlay != null && myslf.audioSource != null)
        {
            myslf.audioSource.spatialBlend = 0f; // 0 = 2D 사운드 (위치 무시)
            myslf.audioSource.PlayOneShot(clipToPlay, volume);
        }
    }
    // 🔪 적 칼 공격 사운드 재생
    public static void PlayEnemyKnifeSwing(float volume = 1.0f)
    {
        if (myslf.enemyKnifeClip != null && myslf.audioSource != null)
        {
            myslf.audioSource.spatialBlend = 0f; // 2D 사운드
            myslf.audioSource.PlayOneShot(myslf.enemyKnifeClip, volume);
        }
    }
    //적 사망 사운드
    public static void PlayEnemyDeathSounds(float goreVolume = 1.0f, float moanVolume = 0.8f)
    {
        if (myslf.audioSource != null)
        {
            if (myslf.enemyGoreClip != null)
                myslf.audioSource.PlayOneShot(myslf.enemyGoreClip, goreVolume);

            if (myslf.enemyDeathMoanClip != null)
                myslf.audioSource.PlayOneShot(myslf.enemyDeathMoanClip, moanVolume);
        }
    }
    //무기 변경 사운드
    public static void PlayWeaponSwapSound(float volume = 1.0f)
    {
        if (myslf.audioSource != null && myslf.weaponSwapClip != null)
        {
            myslf.audioSource.spatialBlend = 0f; // 2D 사운드
            myslf.audioSource.PlayOneShot(myslf.weaponSwapClip, volume);
        }
    }
}


