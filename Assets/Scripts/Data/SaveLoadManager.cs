using UnityEngine;
using SaveDataVC = SaveDataV1; // 버전 바뀔때마다 다 수정하면 번거로우니까 네임스페이스만 바꾸기
using Newtonsoft.Json;
using System.IO;

public static class SaveLoadManager
{
    public enum SaveMode
    {
        Text, //json
        Encrypted,//dat
    }
    public static SaveMode Mode { get; set; } = SaveMode.Text;
    // 현재 클라이언트의 버젼
    public static int SaveDataVersion { get; } = 1;
    public static SaveDataVC Data { get; set; } = new SaveDataVC();

    private static readonly string SaveDirectory = $"{Application.persistentDataPath}/Save";
    private static readonly string[] SaveFileNames =
    {
        "SaveAuto.json",
        "Save1.json",
        "Save2.json",
        "Save3.json",
    };

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        
        // 버젼마다 개별 클래스로 관리를 하기 때문에 추가. 무슨 클래스인지 타입 구분 가능
        TypeNameHandling = TypeNameHandling.All,

    };

    public static bool Save(int slot = 0)
    {
        return Save(slot, Mode);
    }
    public static bool Load(int slot = 0)
    {
        return Load(slot, Mode);
    }

    static SaveLoadManager()
    {
        if(!Load())
        {
            Debug.LogError("세이브 파일 로드 실패");
        }
    }

    public static bool Save(int slot, SaveMode mode)
    {
        if (Data == null || slot < 0 || slot >= SaveFileNames.Length)
        { 
            return false; 
        }

        // try - catch 는 안쓰는게 좋지만 파일입출력 같은 제어할 수 없는 상황에선 반드시 사용하기
        // 다른 사람이 접근 가능하기 때문에 원치 않은 오류를 발생 시킬 수 있음
        try
        {
            if(!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            string path = GetSaveFilePath(mode, 0);
            var json = JsonConvert.SerializeObject(Data, settings);
            Debug.Log(path);

            if(Mode == SaveMode.Text)
            {
                File.WriteAllText(path, json);
            }
            else
            {
                //File.WriteAllBytes(path, CryptoUtil.Encrypt(json));

            }

            return true;
        }
        catch 
        {
            Debug.LogError("세이브 예외");
            return false;
        }
    }

    public static bool Load(int slot, SaveMode mode)
    {
        if(slot < 0 || (slot >= SaveFileNames.Length))
        {
            return false;
        }

        string path = GetSaveFilePath(mode, 0);

        if (!File.Exists(path))
        {
            return Save();
        }

        try
        {
            string json = string.Empty;
            switch(mode)
            {
                case SaveMode.Text:
                    json = File.ReadAllText(path);
                    break;
                case SaveMode.Encrypted:
                    var encrypted = File.ReadAllBytes(path);
                    //json = CryptoUtil.Decrypt(encrypted);
                    break;

            }

            var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);

            // 버젼업
            while (saveData.Version < SaveDataVersion)
            {
                Debug.Log(saveData.Version);
                saveData = saveData.VersionUp();
                Debug.Log(saveData.Version);

            }

            // 현재 버젼과 클라이언트버젼이 같아질 때만 되게
            Data = saveData as SaveDataVC;
            return true;
        }

        catch
        {
            Debug.LogError("로드 예외");

            return false;

        }

    }
    private static string GetSaveFilePath(int slot)
    {
        return GetSaveFilePath(Mode, slot);
    }
    private static string GetSaveFilePath(SaveMode mode, int slot = 0)
    {
        var ext = mode == SaveMode.Text ? ".json" : ".dat";
        return Path.Combine(SaveDirectory, $"{SaveFileNames[slot]}{ext}");
    }
}
