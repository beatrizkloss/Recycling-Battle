using UnityEngine;

public class Jogador : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    public int attackDamage = 10; // Dano do ataque
    public float attackRange = 1f; // Alcance do ataque
    public LayerMask enemyLayers; // Camada dos inimigos
    public int maxHealth = 100;
    public GameObject deathEffectPrefab; 
    public GameObject gameOverUI; 
    
    private int currentHealth;

    private readonly float attackCooldown = 0.5f; // Tempo de espera entre ataques
    private float lastAttackTime = 0f; // Último tempo de ataque

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Input de movimento
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Atualizar parâmetros do animador
        if (animator != null && movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);

            // Definir o estado de correndo
            bool isMoving = movement.magnitude > 0;
            animator.SetBool("IsMoving", isMoving);

            // Virar o sprite do jogador com base na direção do movimento
            if (movement.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Direita
            }
            else if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Esquerda
            }
        }
        else
        {
            // Certificar-se de que a animação de corrida está desativada quando não está se movendo
            if (animator != null)
                animator.SetBool("IsMoving", false);
        }

        // Input de ataque
        if (Time.time >= lastAttackTime + attackCooldown) // Verifica se o cooldown do ataque já passou
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Z))
            {
                lastAttackTime = Time.time; // Atualiza o tempo do último ataque

                Vector2 attackDirection = Vector2.zero;

                if (movement.x > 0)
                {
                    animator.SetTrigger("AtaqueDireita");
                    attackDirection = Vector2.right;
                }
                else if (movement.x < 0)
                {
                    animator.SetTrigger("AtaqueDireita"); // Usa a mesma animação para esquerda
                    attackDirection = Vector2.left;
                }
                else if (movement.y > 0)
                {
                    animator.SetTrigger("AtaqueCostas");
                    attackDirection = Vector2.up;
                }
                else if (movement.y < 0)
                {
                    animator.SetTrigger("AtaqueFrente");
                    attackDirection = Vector2.down;
                }
                else // Ataque parado
                {
                    animator.SetTrigger("AtaqueFrente");
                    attackDirection = Vector2.down;
                }

                AttackEnemy(attackDirection);
            }
        }
    }

    void FixedUpdate()
    {
        // Movimento
        rb.MovePosition(rb.position + movement.normalized * (5f * Time.fixedDeltaTime)); // Ajustar moveSpeed conforme necessário
    }

    void AttackEnemy(Vector2 direction)
    {
        // Lógica de ataque ao inimigo
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Inimigo inimigo = enemy.GetComponent<Inimigo>();
            if (inimigo != null)
            {
                // Aplica dano ao inimigo
                inimigo.TakeDamage(attackDamage);
                Debug.Log("Inimigo recebeu dano!"); // Mensagem de debug para verificar o dano aplicado
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (transform == null)
            return;

        // Desenha o alcance do ataque no editor
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se colidiu com o inimigo
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            TakeDamage(5); // Aplica dano ao jogador ao colidir com o inimigo
            Debug.Log("Jogador recebeu dano!"); // Mensagem de debug para verificar o dano aplicado
        }
    }

    public void TakeDamage(int damageAmount)
{
    currentHealth -= damageAmount;
    Debug.Log("Jogador recebeu dano! Saúde atual: " + currentHealth); // Mensagem de debug para verificar a aplicação do dano

    if (currentHealth <= 0)
    {
        Die();
    }
}

    void Die()
    {
       //Chamar morte --> dava para fazer de outra forma
        Morrer();
    }

   public void Morrer()
    {
        // Instanciar a animação de morte
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Mostrar tela de Game Over
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Destruir o jogador
        Destroy(gameObject);
    }
}
