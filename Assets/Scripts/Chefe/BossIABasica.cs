using UnityEngine;

public class BossIABasica : MonoBehaviour
{
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;

    BossAtaques bossAtaques;

    void Start()
    {
        bossAtaques = GetComponent<BossAtaques>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            int randomAttack = Random.Range(0, 3); // 0, 1 ou 2
            ExecuteAttack(randomAttack);
            cooldownTimer = attackCooldown;
        }
    }

    void ExecuteAttack(int attackIndex)
    {
        switch (attackIndex)
        {
            case 0:
                bossAtaques.ComecarPatrulha();
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
