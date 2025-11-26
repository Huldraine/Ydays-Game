using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class vendeur : MonoBehaviour
{
    [Header("UI")]
    public GameObject vendeurPanel;
    public TMP_Text orText;

    public Transform itemsConteneur;
    public GameObject itemTemplate;

    [Header("Données Joueur")]
    public int gold = 1000;

    [System.Serializable]
    public class Item
    {
        public string nomItem;
        public int prixItem;
        public Sprite icone;
        public string description;
    }

    [Header("Articles disponibles")]
    public Item[] items;

    void Start()
    {
        vendeurPanel.SetActive(false);

        // Sécurité : désactiver le template
        if (itemTemplate.activeSelf)
            itemTemplate.SetActive(false);
    }

    public void OpenShop()
    {
        vendeurPanel.SetActive(true);
        orText.text = "Or : " + gold;

        // Nettoyer
        foreach (Transform c in itemsConteneur)
            Destroy(c.gameObject);

        // Générer les items
        foreach (Item it in items)
        {
            GameObject obj = Instantiate(itemTemplate, itemsConteneur);
            obj.SetActive(true);

            // Récupération composants
            TMP_Text nomUI = obj.transform.Find("Texts/NomItem").GetComponent<TMP_Text>();
            TMP_Text prixUI = obj.transform.Find("Texts/PrixItem").GetComponent<TMP_Text>();
            Image iconUI = obj.transform.Find("Icon").GetComponent<Image>();
            Button btn = obj.GetComponent<Button>();

            // Assignation
            nomUI.text = it.nomItem;
            prixUI.text = it.prixItem + " G";
            iconUI.sprite = it.icone;

            // Callback achat
            btn.onClick.AddListener(() => Achat(it));
        }
    }

    public void Achat(Item it)
    {
        if (gold < it.prixItem)
        {
            Debug.Log("Pas assez d'or !");
            return;
        }

        gold -= it.prixItem;
        orText.text = "Or : " + gold;

        Debug.Log("Achat effectué : " + it.nomItem);
    }
}
