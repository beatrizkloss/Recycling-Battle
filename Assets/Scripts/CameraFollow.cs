using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Referência ao transform do jogador
    public Vector3 offset;    // Deslocamento entre a câmera e o jogador
    public float smoothSpeed = 0.125f;  // Velocidade de suavização

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}

