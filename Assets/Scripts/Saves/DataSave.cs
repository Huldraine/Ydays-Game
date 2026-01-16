using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSave : MonoBehaviour
{
    [Header("Données du joueur")]
    [SerializeField] private Transform player; 
    [SerializeField] private Health life;
    
    public void Awake()
    {
        string filePath = Application.persistentDataPath + "/data.json"; // Chemin du fichier JSON de sauvegarde;
    }
    
    string filePath =  Application.persistentDataPath + "/data.json";     
    
    public void SaveToJson()
    {
        string filePath = Application.persistentDataPath + "/data.json"; 
        life.TakeDamage(0);
        
        Data saveData = new Data();
        
        // Récupération des données du joueurs à sauvegarder
        saveData.levelName = SceneManager.GetActiveScene().name;
        saveData.playerPosition = player.position.ToString();
        saveData.playerHealth = life.currentHealth;
        Debug.Log("Données récupéré et sauvegardé");
        
        string dataJson = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, dataJson);
        
        Debug.Log("Sauvegarde en cours " + saveData + " " + filePath);
    }

    public void LoadFromJson()
    {
        if (!File.Exists(filePath)) 
        { 
            Debug.LogError("Aucune sauvegarde trouvé");
            
        }
        
    }
}

[System.Serializable]
// Class qui stock les informations à sauvegarder 
public class Data 
{
    public string levelName;
    public string playerPosition;
    public float playerHealth;
}