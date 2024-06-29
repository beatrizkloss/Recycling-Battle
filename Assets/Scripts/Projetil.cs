using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float speed = 10f; // Velocidade do projétil
    public int damage = 10; // Dano que o projétil causa ao jogador
    private Transform player;
    private Vector2 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            target = new Vector2(player.position.x, player.position.y);
        }
        else
        {
            Destroy(gameObject); // Destroi o projétil se não encontrar o jogador
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                Destroy(gameObject); // Destroi o projétil ao alcançar o destino
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Jogador jogador = collision.GetComponent<Jogador>();
            if (jogador != null)
            {
                jogador.TakeDamage(damage);
                Debug.Log("Jogador atingido pelo projétil!"); // Mensagem de debug para verificar a colisão
            }
            Destroy(gameObject); // Destroi o projétil ao colidir com o jogador
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // Destroi o projétil ao colidir com um obstáculo
        }
    }
}

