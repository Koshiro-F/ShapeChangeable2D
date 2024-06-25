// JsonControllerGenerics.cs
using System.IO;
using UnityEngine;

namespace MyJsonControllerGenerics
{
    public class JsonController<T> where T : new()
    {
        public string filePath { get; private set; }
        public string folderPath { get; private set; }

        // コンストラクタ
        public JsonController(string jsonFileName)
        {
            folderPath = Path.Combine(Application.persistentDataPath, "JSON");
            filePath = Path.Combine(Application.persistentDataPath, "JSON", jsonFileName);
        }


        // JSONファイルを初期化
        public void InitializeJsonFile()
        {
            if (!JsonFileExists())
            {
                T empty = new T();
                string json = JsonUtility.ToJson(empty, true);
                File.WriteAllText(filePath, json);
            }
            Debug.Log(filePath);
        }

        // JSONファイルからデータを読み込む
        public T LoadJsonData()
        {
            string jsonString = File.ReadAllText(filePath);
            T data = JsonUtility.FromJson<T>(jsonString);
            return data;
        }

        // JSONファイルを上書きする
        public void UpdateJsonFile(T data)
        {
            var JSON = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, JSON);
        }

        // JSONファイルの存在確認。 フォルダがなければ作成
        private bool JsonFileExists()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                return false;
            }
            else if (!File.Exists(filePath))
            {
                return false;
            }
            return true;
        }
    }
}
