using UnityEngine;

public class BossIAFSM : MonoBehaviour
{
    public Transform player;
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;

    private enum BossState { Idle, ChooseAttack, Attack, Recover }
    private BossState currentState = BossState.Idle;

    private int chosenAttack = -1;

    void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                IdleState();
                break;

            case BossState.ChooseAttack:
                ChooseAttackState();
                break;

            case BossState.Attack:
                AttackState();
                break;

            case BossState.Recover:
                RecoverState();
                break;
        }
    }

    void IdleState()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            currentState = BossState.ChooseAttack;
        }
    }

    void ChooseAttackState()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < 3f)
            chosenAttack = 0; // Investida
        else if (distance < 6f)
            chosenAttack = 1; // Pulo
        else
            chosenAttack = 2; // Projétil

        currentState = BossState.Attack;
    }

    void AttackState()
    {
        switch (chosenAttack)
        {
            case 0:
                Debug.Log("FSM: Ataque 1 → Investida!");
                break;
            case 1:
                Debug.Log("FSM: Ataque 2 → Pulo!");
                break;
            case 2:
                Debug.Log("FSM: Ataque 3 → Projétil!");
                break;
        }

        currentState = BossState.Recover;
    }

    void RecoverState()
    {
        cooldownTimer = attackCooldown;
        currentState = BossState.Idle;
    }
}
