using UnityEngine;
using Zenject;

namespace Game
{
    public class SaveDataSystem
    {

        private const string DATA_KEY = "GameData";

        public GameData Data { get; private set; }
    

        public void Initialize()
        {
            if (PlayerPrefs.HasKey(DATA_KEY))
            {
                LoadData();
            }
            else
            {
                Data = new GameData();
            }
        }

        public void LoadData()
        {
            string jsonData = PlayerPrefs.GetString(DATA_KEY);
            Data = JsonUtility.FromJson<GameData>(jsonData);
        }

        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(DATA_KEY,jsonData);
        }
    }
}