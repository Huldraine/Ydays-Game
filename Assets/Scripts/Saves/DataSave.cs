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
        Debug.Log("Données récupéré et sauvegardé");
        
        string dataJson = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(GetFilePath(), dataJson);
        
        Debug.Log("Sauvegarde en cours " + saveData + " " + filePath);
    }
    private Data waitLoadData;
    public void LoadFromJson()
    {
        
        
        if (!File.Exists(GetFilePath())) 
        { 
            Debug.LogError("Aucune sauvegarde trouvé");
            return;
        }
        Debug.Log("Sauvegarde trouvé !");
        
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
        var pause = FindObjectOfType<PauseMenu>();
        
        pause.Resume();
        
        if (playerGo == null)
        {
            Debug.LogError("Joueur introuvable dans la scène chargée (tag 'Player')");
            return;
        }
        
        // Appliquer les données
        player.position = waitLoadData.playerPosition;
        life.currentHealth = waitLoadData.playerHealth;
        
        Debug.Log("Chargement en cours " + waitLoadData + " " + filePath);
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