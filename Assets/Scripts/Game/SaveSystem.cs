using System;
using Game;
using Zenject;
using UnityEngine;

public class SaveSystem : IInitializable
{
  private const string SAVE_KEY = "SAVE_DATA";

  private SaveData _data;
  private BoardConfig _boardConfig;
  
  public SaveData Data => _data;
  public bool LoadSuccessful { get; private set; }

  public SaveSystem(BoardConfig boardConfig)
  {
    _boardConfig = boardConfig;
  }
  
  public void Initialize()
  {
    if (PlayerPrefs.HasKey(SAVE_KEY))
    {
      LoadData();
      LoadSuccessful = true;
    }
    else
    {
      _data = new SaveData(_boardConfig.SizeX * _boardConfig.SizeY);
      LoadSuccessful = false;
    }
  }

  private void LoadData()
  {
    string data = PlayerPrefs.GetString(SAVE_KEY, string.Empty);
    _data = JsonUtility.FromJson<SaveData>(data);
  }

  public void SaveData()
  {
    string json = JsonUtility.ToJson(_data);
    PlayerPrefs.SetString(SAVE_KEY, json);
  }
  
}

[Serializable]
public class SaveData
{
  public int Score;
  public string[] ElementsKey;

  public SaveData(int keyCount)
  {
    Score = 0;
    ElementsKey = new string[keyCount];
  }
  
  public void UpdateScore(int value)
  {
    Score = value;
  }

  public void UpdateElement(int index,Element element)
  {
    ElementsKey[index] = element.ConfigItem.Key;
  }
}
