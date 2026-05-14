using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSave : MonoBehaviour
{
    public static DataSave Instance { get; private set; }

    private Transform player;
    private Health life;
    
    private string filePath;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private string GetFilePath()
    {
        if (string.IsNullOrEmpty(filePath))
            filePath = Application.persistentDataPath + "/data.json"; // Chemin du fichier JSON de sauvegarde;

        return filePath;
    }

    
    public void SaveToJson()
    {
        Data saveData = new Data();
        
        // Récupération des données du joueurs à sauvegarder
        saveData.levelName = SceneManager.GetActiveScene().name;
        saveData.playerPosition = player.position;
        saveData.playerHealth = life.currentHealth;
        
        string dataJson = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(GetFilePath(), dataJson);
    }
    
    private Data waitLoadData;
    public void LoadFromJson()
    {
        if (!File.Exists(GetFilePath())) return; 
        
        waitLoadData =  JsonUtility.FromJson<Data>(File.ReadAllText(GetFilePath()));
        
        // On charge la scène
        SceneManager.LoadScene(waitLoadData.levelName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-Bind des refs dans la nouvelle scène
        GameObject playerGo = GameObject.FindWithTag("Player");
        if (playerGo == null) return;
        player = playerGo.transform;
        if (player.GetComponent<Health>() == null) return;
        life = player.GetComponent<Health>();

        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        //if (pauseMenu != null)
        //    pauseMenu.ForceResume();

        if (waitLoadData == null) return;

        // Appliquer les données
        player.position = waitLoadData.playerPosition;
        life.currentHealth = waitLoadData.playerHealth;

        waitLoadData = null;
    }
}

[System.Serializable]
// Class qui stock les informations à sauvegarder 
public class Data 
{
    public string levelName;
    public Vector3 playerPosition;
    public float playerHealth;
}