using System;

[Serializable]
public class GameData
{
    public float playerHealth;
    public int score;
    public int currentLevel;

    // 💡 추가된 부분: 플레이 시간 (초 단위)
    public float playTime;

    // 위치 데이터
    public float posX, posY, posZ;

    public GameData()
    {
        playerHealth = 100f;
        score = 0;
        currentLevel = 1;
        playTime = 0f; // 0초로 초기화
        posX = 0; posY = 0; posZ = 0;
    }
}