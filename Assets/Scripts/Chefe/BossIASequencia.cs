using UnityEngine;

public class BossIASequencia : MonoBehaviour
{
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;
    private int currentAttack = 0;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            ExecuteAttack(currentAttack);
            currentAttack = (currentAttack + 1) % 3; // avança para o próximo (0→1→2→0...)
            cooldownTimer = attackCooldown;
        }
    }

    void ExecuteAttack(int attackIndex)
    {
        switch (attackIndex)
        {
            case 0:
                Debug.Log("Ataque 1: Investida!");
                break;
            case 1:
                Debug.Log("Ataque 2: Pulo!");
                break;
            case 2:
                Debug.Log("Ataque 3: Projétil!");
                break;
        }
    }
}
