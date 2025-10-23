using System.Collections;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Configurações")]
    public float moveSpeed = 2f;
    public int maxHealth = 2;
    public float knockbackForce = 5f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    [Header("Detecção de ambiente")]
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkDistance = 0.1f;
    public LayerMask groundLayer;

    private int currentHealth;
    private bool vivo = true;
    private bool isKnockBacked = false;
    private bool movingRight = true;
    private bool atacando = false;
    private float lastAttackTime = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!vivo || isKnockBacked || atacando) return;

        Move();
        CheckEnvironment();
    }

    // ================= Movimento =================
    void Move()
    {
        float direction = movingRight ? 1 : -1;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        spriteRenderer.flipX = direction < 0;
        anim.SetFloat("Velocidade", Mathf.Abs(rb.velocity.x));
    }

    void CheckEnvironment()
    {
        // Checa chão à frente
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);
        // Checa parede à frente
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, movingRight ? Vector2.right : Vector2.left, checkDistance, groundLayer);

        if (!groundHit.collider || wallHit.collider)
        {
            movingRight = !movingRight;
        }
    }

    // ================= Ataque =================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!vivo) return;

        if (collision.gameObject.CompareTag("Player") && Time.time - lastAttackTime > attackCooldown)
        {
            var playerHealth = collision.gameObject.GetComponent<SistemaDeVida>();
            if (playerHealth != null)
            {
                playerHealth.AplicarDano(attackDamage);
                anim.SetTrigger("Atacar");
                lastAttackTime = Time.time;
                StartCoroutine(AttackCooldown());
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        atacando = true;
        yield return new WaitForSeconds(attackCooldown);
        atacando = false;
    }

    // ================= Sistema de Vida =================
    public void LevarDano(int dano)
    {
        if (!vivo) return;

        currentHealth -= dano;
        AnimacaoDeDano();
        EfeitoDeRecuo();
        EfeitoDePiscar();

        if (currentHealth <= 0)
            Morrer();
    }

    private void Morrer()
    {
        vivo = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        col.enabled = false;

        anim.SetBool("Vivo", vivo);

        EfeitoDePiscar(); // efeito opcional ao morrer
        Destroy(gameObject, 3f);
    }

    // ================= Efeitos =================
    public void EfeitoDeRecuo()
    {
        isKnockBacked = true;

        float knockbackDirection = movingRight ? -1 : 1;
        Vector2 force = new(knockbackDirection * knockbackForce, 0);

        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockback());
    }

    IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        isKnockBacked = false;
    }

    public void EfeitoDePiscar()
    {
        StartCoroutine(Piscar());
    }

    IEnumerator Piscar()
    {
        Color corOriginal = spriteRenderer.color;
        Color corTransparente = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = corTransparente;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = corOriginal;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AnimacaoDeDano()
    {
        anim.SetTrigger("Machucado");
        StartCoroutine(ResetMachucado());
    }

    IEnumerator ResetMachucado()
    {
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("Machucado");
    }
}
