using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public delegate void EnemyDeath();
    public event EnemyDeath OnEnemyDeath;

    Transform player;
    public float moveSpeed = 5f;
    public float minDistance = 1f; // Distância mínima para evitar mudança de direção
    public int maxHealth = 50;
    private int currentHealth;
    bool isFollowing = true; // Flag para controlar se o inimigo está seguindo o jogador

    public int damageToPlayer = 5; // Dano que o inimigo causa ao jogador
    public GameObject deathEffectPrefab; // Prefab da animação de morte

    public GameObject projectilePrefab; // Prefab do projétil
    public float shootInterval = 2f; // Intervalo entre disparos
    private float lastShootTime; // Tempo do último disparo

    void Start()
    {
        player = FindObjectOfType<Jogador>().transform; // Encontra o jogador
        currentHealth = maxHealth; // Inicializa a saúde atual
        lastShootTime = Time.time;
    }

    void Update()
    {
        if (isFollowing && player != null)
        {
            // Calcula a direção do jogador em relação ao inimigo a cada quadro
            Vector2 direction = player.position - transform.position;
            direction.Normalize();

            // Verifica a distância entre o inimigo e o jogador
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > minDistance)
            {
                // Move o inimigo na direção do jogador
                transform.position += moveSpeed * Time.deltaTime * (Vector3)direction;
            }

            // Virar o sprite do inimigo com base na direção do movimento
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Direita
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Esquerda
            }

            // Disparar projétil
            if (Time.time >= lastShootTime + shootInterval)
            {
                ShootProjectile();
                lastShootTime = Time.time;
            }
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se colidiu com o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aplica dano ao jogador
            Jogador jogador = collision.gameObject.GetComponent<Jogador>();
            if (jogador != null)
            {
                jogador.TakeDamage(damageToPlayer);
            }

            // Define que o inimigo não está mais seguindo o jogador
            isFollowing = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica se deixou de colidir com o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Define que o inimigo está seguindo o jogador novamente
            isFollowing = true;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Morrer();
    }

    public void Morrer()
    {
        // Instanciar a animação de morte
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Notificar o spawner que o inimigo morreu
        OnEnemyDeath?.Invoke();

        // Destruir o inimigo
        Destroy(gameObject);
    }
}
