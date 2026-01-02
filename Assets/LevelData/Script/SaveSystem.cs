using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static GameData data;
    private static string savePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

    public static void Load()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                data = JsonUtility.FromJson<GameData>(json);
                
                Debug.Log($"<color=green>[LOAD SUCCESS]</color> Đọc từ: {savePath}\nData: {json}");
                
                if (data == null)
                {
                    Debug.LogError("[LOAD ERROR] JsonUtility trả về null. Tạo data mới.");
                    data = new GameData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[LOAD EXCEPTION] Lỗi đọc file: {e.Message}");
                data = new GameData();
            }
        }
        else
        {
            Debug.LogWarning($"[LOAD] Chưa thấy file save tại: {savePath}. Tạo mới.");
            data = new GameData();
        }
    }

    public static void Save()
    {
        if (data == null) data = new GameData();

        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            
            Debug.Log($"<color=yellow>[SAVE SUCCESS]</color> Đã lưu vào: {savePath}\nContent: {json}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SAVE ERROR] Không thể ghi file: {e.Message}");
        }
    }
}