using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class HighscoreEntry
{
    public string playerName;
    public float time; 
}

[System.Serializable]
public class Highscores
{
    public List<HighscoreEntry> highscoreEntryList;
}




public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance; 

    private Highscores highscores;
    private const string SAVE_KEY = "highscoreTable";
    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighscores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadHighscores()
    {
        string jsonString = PlayerPrefs.GetString(SAVE_KEY);

        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.Log("Nenhum ficheiro de highscore encontrado. A criar um novo.");
            highscores = new Highscores { highscoreEntryList = new List<HighscoreEntry>() };
        }
        else
        {
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
            Debug.Log("Highscores carregados com sucesso!");
        }
    }

    private void SaveHighscores()
    {
        string json = JsonUtility.ToJson(highscores);

        // Guarda a string JSON no PlayerPrefs
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save(); 
        Debug.Log("Highscores guardados!");
    }

    public void AddHighscoreEntry(string playerName, float time)
    {
        //  Cria a nova entrada
        HighscoreEntry newEntry = new HighscoreEntry { playerName = playerName, time = time };

        // Adiciona à lista
        highscores.highscoreEntryList.Add(newEntry);

        // Ordena a lista (o tempo mais baixo fica em primeiro)
        highscores.highscoreEntryList.Sort((x, y) => x.time.CompareTo(y.time));

        // Limita a lista a um número máximo de entradas, ex: 10
        while (highscores.highscoreEntryList.Count > 10)
        {
            highscores.highscoreEntryList.RemoveAt(10);
        }

        // Guarda as alterações
        SaveHighscores();
    }

    public List<HighscoreEntry> GetHighscoreList()
    {
        return highscores.highscoreEntryList;
    }
}
