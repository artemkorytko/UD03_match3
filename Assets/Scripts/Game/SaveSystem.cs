using UnityEngine;
using Zenject;

namespace Game
{
    public class SaveSystem
    {
        private const string SAVE_KEY = "GameData";

        public GameData Data { get; private set; }

        public SaveSystem()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                LoadData();
            }
            else
            {
                Data = new GameData();
                PlayerPrefs.SetString(JsonUtility.ToJson(Data), SAVE_KEY);
                PlayerPrefs.Save();
            }
        }

        public void LoadData()
        {
            var text = PlayerPrefs.GetString(SAVE_KEY);
            Data = JsonUtility.FromJson<GameData>(text);
        }

        public void SaveData()
        {
            var data = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(data, SAVE_KEY);
            PlayerPrefs.Save();
        }
    }
}