using UnityEngine;
using UnityEngine.UI;

public class SpriteHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; 
    public Image healthBarImage;      
    public Sprite[] healthSprites;    

    void Update()
    {
        if (playerHealth == null || healthBarImage == null || healthSprites.Length == 0)
            return;

        float healthPercent = (float)playerHealth.CurrentHealth / playerHealth.maxHealth;
        int index = Mathf.CeilToInt(healthPercent * (healthSprites.Length - 1));
        index = Mathf.Clamp(index, 0, healthSprites.Length - 1);

        healthBarImage.sprite = healthSprites[index];
    }
}
