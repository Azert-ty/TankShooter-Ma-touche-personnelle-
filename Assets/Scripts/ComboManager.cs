using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private int comboCount = 0;
    private float comboTimer = 0f;

    [Header("Réglages du combo")]
    public float comboDuration = 3f; // Durée avant que le combo expire
    public float comboMultiplier = 0.1f; // +10% de score par coup consécutif

    private void Update()
    {
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                comboCount = 0;
            }
        }
    }

    public void RegisterHit()
    {
        comboCount++;
        comboTimer = comboDuration;
    }

    public int ApplyComboMultiplier(int baseScore)
    {
        float bonus = 1 + comboCount * comboMultiplier;
        return Mathf.RoundToInt(baseScore * bonus);
    }
}
