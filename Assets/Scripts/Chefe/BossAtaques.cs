using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAtaques : MonoBehaviour
{
    public Transform player;
    
    [Header("Patrulha")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float patrolSpeed = 2f;
    private bool movingRight = true;
    private bool patrolling = true;

    [Header("Prefabs")]
    public GameObject swordPrefab;
    public GameObject aoePrefab;

    [Header("Atributos")]
    public float dashForce = 10f;
    public float aoeRadius = 3f;
    public float swordSpeed = 8f;
    public float swordReturnSpeed = 12f;

    private Rigidbody2D rb;
    private GameObject spawnedSword;
    private bool swordReturning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ComecarPatrulha()
    {
        patrolling = true;
    }

    private void Patrulhar()
    {
        // Movimento entre dois pontos
        if (movingRight)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                     rightPoint.position,
                                                     patrolSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, rightPoint.position) < 0.1f)
                movingRight = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                     leftPoint.position,
                                                     patrolSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, leftPoint.position) < 0.1f)
                movingRight = true;
        }
    }

    // ------------------- ATAQUES ------------------- //

    // 1. Investida Diagonal vinda do céu
    public void Attack_DiagonalDive()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = Vector2.zero; // reseta movimento antes
        rb.AddForce(new Vector2(direction.x, -1f).normalized * dashForce, ForceMode2D.Impulse);

        Debug.Log("Boss faz investida diagonal do céu!");
    }

    // 2. Explosão em área no ar
    public void Attack_AoE()
    {
        // instanciar efeito visual de explosão
        GameObject aoe = Instantiate(aoePrefab, transform.position, Quaternion.identity);
        Destroy(aoe, 1.5f); // limpa depois de 1.5s

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Player atingido pelo ataque em área!");
                // Aqui chamaria o TakeDamage do player
            }
        }
    }

    // 3. Arremesso da espada para frente e puxada de volta
    public void Attack_ThrowSword()
    {
        if (spawnedSword == null) // só cria se não tiver uma já lançada
        {
            Vector2 direction = (player.position - transform.position).normalized;
            spawnedSword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
            Rigidbody2D swordRb = spawnedSword.GetComponent<Rigidbody2D>();
            swordRb.velocity = direction * swordSpeed;

            Debug.Log("Boss lança a espada para frente!");
            swordReturning = false;
        }
    }

    void Update()
    {
        if (spawnedSword != null)
        {
            Rigidbody2D swordRb = spawnedSword.GetComponent<Rigidbody2D>();

            if (!swordReturning)
            {
                // Se a espada já andou um certo tempo, começa a voltar
                if (Vector2.Distance(transform.position, spawnedSword.transform.position) > 5f)
                {
                    swordReturning = true;
                }
            }
            else
            {
                // Espada volta em direção ao boss
                Vector2 dirBack = (transform.position - spawnedSword.transform.position).normalized;
                swordRb.velocity = dirBack * swordReturnSpeed;

                // Se chegou perto, destrói
                if (Vector2.Distance(transform.position, spawnedSword.transform.position) < 0.5f)
                {
                    Destroy(spawnedSword);
                    swordReturning = false;
                    Debug.Log("Espada retornou ao boss!");
                }
            }
        }else if (patrolling)
        {
            Patrulhar();
        }
    }

    // Gizmo para visualizar área do ataque no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
