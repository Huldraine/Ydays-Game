using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{
    // la Vie maximale du joueur
    public int maxHealth = 5;
    // Vie actuelle du joueur
    public float currentHealth;
    // images des coeurs
    public List<Image> healthImages;

    // permet de changer pour les coeurs vides ou pleins
    public Sprite spritePlein;
    public Sprite spriteVide;

    void Start()
    {
        // Definir la vie au maximum au debut
        currentHealth = maxHealth;
        MettreAJourLesMasques();
    }

    // Implémentation de l’interface IDamageable (en int)
    public void TakeDamage(int amount)
    {
        TakeDamage((float)amount);
    }

    // Version float utilisée ailleurs dans ton code (hazardDamage, etc.)
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        MettreAJourLesMasques();
    }

    void MettreAJourLesMasques()
    {
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < currentHealth)
                healthImages[i].sprite = spritePlein;
            else
                healthImages[i].sprite = spriteVide;
        }
    }
}
