using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour, IDamageable
{
    public int maxHealth = 5;
    public float currentHealth;
    public List<Image> healthImages;

    public UnityEvent onDeath = new UnityEvent();

    public Sprite spritePlein;
    public Sprite spriteVide;

    public Sprite[] animationFrames;
    public float frameDuration = 0.08f;

    private HashSet<int> animatingIndices = new HashSet<int>();
    private bool hasDied;

    void Start()
    {
        currentHealth = maxHealth;
        hasDied = false;
        MettreAJourLesMasques();
    }

    public void TakeDamage(int amount)
    {
        TakeDamage((float)amount);
    }

    public void TakeDamage(float damage)
    {
        if (hasDied)
            return;

        float oldHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        for (int i = Mathf.FloorToInt(currentHealth); i < Mathf.FloorToInt(oldHealth); i++)
        {
            if (animationFrames != null && animationFrames.Length > 0 && i < healthImages.Count && i >= 0)
            {
                StartCoroutine(AnimateHeartLoss(i));
            }
        }

        if (oldHealth > 0f && currentHealth <= 0f)
        {
            hasDied = true;
            onDeath?.Invoke();
        }

        MettreAJourLesMasques();
    }

    public void RestoreFullHealth()
    {
        hasDied = false;
        currentHealth = maxHealth;
        MettreAJourLesMasques();
    }

    void MettreAJourLesMasques()
    {
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (animatingIndices.Contains(i)) continue;

            if (i < currentHealth)
                healthImages[i].sprite = spritePlein;
            else
                healthImages[i].sprite = spriteVide;
        }
    }

    IEnumerator AnimateHeartLoss(int index)
    {
        animatingIndices.Add(index);
        Image targetImage = healthImages[index];

        foreach (Sprite frame in animationFrames)
        {
            targetImage.sprite = frame;
            yield return new WaitForSeconds(frameDuration);
        }

        animatingIndices.Remove(index);
        targetImage.sprite = spriteVide;
    }
}
