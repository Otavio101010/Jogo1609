using UnityEngine;

public class BossIACondicional : MonoBehaviour
{
    public Transform player;
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance < 3f)
            {
                ExecuteAttack(0); // Investida se estiver perto
            }
            else if (distance < 6f)
            {
                ExecuteAttack(1); // Pulo se estiver a média distância
            }
            else
            {
                ExecuteAttack(2); // Projétil se estiver longe
            }

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
