using UnityEngine;

public class DrawGroundLine : MonoBehaviour
{
    // Altura da linha em rela��o ao ch�o
    public float lineHeight = 0.1f;

    // Cor da linha
    public Color lineColor = Color.red;

    // Tamanho da linha
    public float lineWidth = 2.0f;

    // Dist�ncia m�xima para desenhar a linha
    public float maxDistance = 10.0f;

    private void OnDrawGizmos()
    {
        // Define a cor do gizmo
        Gizmos.color = lineColor;

        // Raycast para baixo a partir da posi��o do personagem
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance))
        {
            // Desenha a linha do ponto de colis�o para baixo
            Vector3 startPoint = transform.position;
            Vector3 endPoint = hit.point + Vector3.up * lineHeight;

            // Desenha a linha
            Gizmos.DrawLine(startPoint, endPoint);

            // Desenha uma esfera no ponto de colis�o
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            // Se n�o houver colis�o, desenha uma linha at� a altura m�xima definida
            Vector3 startPoint = transform.position;
            Vector3 endPoint = transform.position - Vector3.up * lineHeight;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}