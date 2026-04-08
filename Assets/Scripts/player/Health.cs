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
        mettreAJourLesMasques();
    }

    public void takeDamage(int amount)
    {
        takeDamage((float)amount);
    }

    public void takeDamage(float damage)
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
                if (animatingIndices.Contains(i))
                    continue;

                StartCoroutine(animateHeartLoss(i));
            }
        }

        if (oldHealth > 0f && currentHealth <= 0f)
        {
            hasDied = true;
            onDeath?.Invoke();
        }

        mettreAJourLesMasques();
    }

    public void restoreFullHealth()
    {
        hasDied = false;
        StopAllCoroutines();
        animatingIndices.Clear();
        currentHealth = maxHealth;
        mettreAJourLesMasques();
    }

    void mettreAJourLesMasques()
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

    IEnumerator animateHeartLoss(int index)
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

