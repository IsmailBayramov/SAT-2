using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб вашего Enemy
    public int timeBetweenSpawn; // Изначальное время между спавнами enemy
    public int enemyAmount; // Количество врагов, которое вы хотите заспавнить
    private GameObject[] targets;

    private void Start()
    {
        StartCoroutine(spawnEnemyX());
        StartCoroutine(spawnEnemyZ());
    }
    private IEnumerator spawnEnemyX()
    {
        int countSpawn = 0; 
        while (countSpawn < enemyAmount)
        {
            countSpawn++;
            int xPos = Random.Range(-115, 115); 

            Instantiate(enemyPrefab, new Vector3(xPos, 1.5f, 115), Quaternion.identity);
            Instantiate(enemyPrefab, new Vector3(xPos, 1.5f, -115), Quaternion.identity);

            if (countSpawn % 10 == 0 && countSpawn <= 100)
            {
                timeBetweenSpawn -= 1;
            }

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    private IEnumerator spawnEnemyZ()
    {
        int countSpawn = 0; // Переменная, которая будет подсчитывать, сколько врагов мы заспавнили
        while (countSpawn < enemyAmount)
        {
            countSpawn++;
            int zPos = Random.Range(-115, 115); // Берем рандомный Z

            // Спавним объект enemyPrefab в указанной позиции без разворотов по осям
            Instantiate(enemyPrefab, new Vector3(115, 1.5f, zPos), Quaternion.identity);
            Instantiate(enemyPrefab, new Vector3(-115, 1.5f, zPos), Quaternion.identity);

            //Если заспавнили 10 врагов и их меньше 100, уменьшаем время между спавном на секунду
            if (countSpawn % 10 == 0 && countSpawn <= 100)
            {
                timeBetweenSpawn -= 1; // 
            }

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        if (targets.Length == 0)
            //SceneManager.LoadScene("Win");
            Debug.Log("You won!");
    }
}
