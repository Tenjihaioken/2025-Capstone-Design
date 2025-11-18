using UnityEngine;
using System.IO; // 파일 입출력용

public class LocalSaveManager : MonoBehaviour
{
    // 저장할 파일 경로 (운영체제에 따라 안전한 경로를 자동 지정)
    string path;

    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    public void SaveGame(GameData data)
    {
        // 1. 데이터를 JSON 문자열로 변환
        string json = JsonUtility.ToJson(data, true);

        // 2. 파일로 쓰기
        File.WriteAllText(path, json);
        Debug.Log("저장 완료: " + path);
    }

    public GameData LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.Log("저장된 파일이 없습니다. 새 데이터를 반환합니다.");
            return new GameData(); // 기본값 반환
        }

        // 1. 파일 읽기
        string json = File.ReadAllText(path);

        // 2. JSON을 데이터 객체로 변환
        GameData data = JsonUtility.FromJson<GameData>(json);
        Debug.Log("로드 완료");
        return data;
    }
}