using UnityEngine;
using System.IO;

public class LocalSaveManager : MonoBehaviour
{
    // 슬롯별 파일 경로를 가져오는 함수
    public string GetSavePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"savefile_{slotIndex}.json");
    }

    // 슬롯 번호를 받아서 저장
    public void SaveGame(GameData data, int slotIndex)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(slotIndex), json);
        Debug.Log($"슬롯 {slotIndex} 저장 완료");
    }

    // 슬롯 번호를 받아서 로드
    public GameData LoadGame(int slotIndex)
    {
        string path = GetSavePath(slotIndex);
        if (!File.Exists(path)) return null; // 파일 없으면 null 반환

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }

    // 해당 슬롯에 파일이 있는지 확인
    public bool ExistSaveData(int slotIndex)
    {
        return File.Exists(GetSavePath(slotIndex));
    }
}