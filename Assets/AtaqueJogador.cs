using UnityEngine;

public class AtaqueJogador : MonoBehaviour
{
    [SerializeField] int danoJogador = 10;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Inimigo"))
        {
            SistemaDeVida sistemaDeVida = other.gameObject.GetComponent<SistemaDeVida>();
            sistemaDeVida.AplicarDano(danoJogador);
        }
    }
}