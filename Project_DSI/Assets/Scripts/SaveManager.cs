using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
	public static SaveManager instance;

	public Save currentSave;
	[System.Serializable]
	public class Save
	{
		public int level;
		public Save(int _levelIndex)
		{
			level = _levelIndex;
		}
	}
    // Start is called before the first frame update
    void Awake()
    {
		#region Singleton
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		#endregion

		BinaryFormatter _formatter = new BinaryFormatter();
		if (File.Exists(Application.persistentDataPath + "/Saves.bp"))
		{
			LoadGame();
		}
		else
		{
			InitializeSave();
		}
    }
	

	public void InitializeSave()
	{
		BinaryFormatter _formatter = new BinaryFormatter();
		FileStream _file = File.Create(Application.persistentDataPath + "/Saves.bp");

		currentSave = new Save(ScoreManager.instance.levelIndex);

		_formatter.Serialize(_file, currentSave);

		_file.Close();
		print(Application.persistentDataPath + "/Saves.bp");
	}

	public void LoadGame()
	{
		BinaryFormatter _formatter = new BinaryFormatter();
		FileStream _file = File.Open(Application.persistentDataPath + "/Saves.bp", FileMode.Open);
		currentSave = (Save)_formatter.Deserialize(_file);
		ScoreManager.instance.levelIndex = currentSave.level;
		_file.Close();
	}

	public void SaveGame()
	{
		BinaryFormatter _formatter = new BinaryFormatter();
		FileStream _file = File.Open(Application.persistentDataPath + "/Saves.bp", FileMode.Open);
		_formatter.Serialize(_file, currentSave);
		_file.Close();
	}
}
