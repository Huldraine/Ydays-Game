using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSave : MonoBehaviour
{
    [Header("Données du joueur")]
    [SerializeField] private Transform player; 
    [SerializeField] private Health life;
    
    private string filePath;

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
        
        // Evènement qui s'éxécute à la fin du chargement de la scène
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // On charge la scène
        SceneManager.LoadScene(waitLoadData.levelName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stoper l'évènement du chargement de la scène 
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        // Re-Bind des refs dans la nouvelle scène
        GameObject playerGo = GameObject.FindWithTag("Player");
        player = playerGo.transform;
        life = player.GetComponent<Health>();
        
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null)
            pauseMenu.ForceResume();


        if (playerGo == null) return;
        
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