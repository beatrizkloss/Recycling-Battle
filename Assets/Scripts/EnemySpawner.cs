using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // Lista de prefabs de diferentes inimigos
    public Transform player; // Referência ao jogador
    public float spawnRadius = 10f; // Raio em torno do jogador onde os inimigos podem spawnar
    public float spawnInterval = 3f; // Intervalo de tempo entre os spawns
    public int maxEnemies = 10; // Número máximo de inimigos simultâneos
    private float lastSpawnTime; // Tempo do último spawn

    private int currentEnemyCount = 0; // Contagem de inimigos ativos

    void Start()
    {
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        // Verifica se é hora de spawnar um novo inimigo e se não atingiu o limite de inimigos ativos
        if (Time.time >= lastSpawnTime + spawnInterval && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Count > 0 && player != null)
        {
            // Define uma posição aleatória em um círculo ao redor do jogador
            Vector2 spawnPosition = player.position + (Vector3)Random.insideUnitCircle.normalized * spawnRadius;

            // Seleciona aleatoriamente um prefab de inimigo da lista
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // Instancia o inimigo na posição gerada
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Incrementa a contagem de inimigos ativos
            currentEnemyCount++;

            // Adiciona um listener para quando o inimigo for destruído, decrementa a contagem
            spawnedEnemy.GetComponent<Inimigo>().OnEnemyDeath += DecrementEnemyCount;
        }
    }

    void DecrementEnemyCount()
    {
        currentEnemyCount--;
    }
}
