using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyShipLarge;
    public GameObject EnemyShipMedium;
    public GameObject EnemyShipSmall;
    public float formationWidth = 10f;
    public float formationHeight = 5f;
    public float spawnDelay = 0.5f;

    private float formationMovementAmount = 1.5f;
    private bool moveLeft = true;
    private float offsetWidth;
    private float distanceToCamera;
    private Vector3 rightEdge;
    private GameData gameData;
    private LevelManager levelManager;
    private bool formationSpawned = false;

    public enum EnemyTypes
    {
        Large,
        Medium,
        Small
    }

    // Use this for initialization
    void Start()
    {
        distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        gameData = GameObject.FindObjectOfType<GameData>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();

        //SpawnFullFormation();

        offsetWidth = formationWidth / 2;
    }

    void SpawnFullFormation()
    {
        Transform nextSpawnPoint = NextFreePosition();
        if (nextSpawnPoint != null)
        {
            SpawnEnemy(nextSpawnPoint.gameObject.GetComponent<EnemySpawnPosition>().GetEnemyType(), nextSpawnPoint);
            Invoke("SpawnFullFormation", spawnDelay);
        }
        
    }

    public GameObject SpawnEnemy(EnemyTypes enemyType, Transform targetSpawner)
    {
        GameObject newEnemy;
        GameObject enemyShipObject = EnemyShipLarge;
        switch (enemyType)
        {
            case EnemyTypes.Large:
                enemyShipObject = EnemyShipLarge;
                break;
            case EnemyTypes.Medium:
                enemyShipObject = EnemyShipMedium;
                break;
            case EnemyTypes.Small:
                enemyShipObject = EnemyShipSmall;
                break;
        }
        newEnemy = Instantiate(enemyShipObject, targetSpawner.transform.position, targetSpawner.transform.rotation) as GameObject;
        newEnemy.transform.parent = targetSpawner;

        return newEnemy;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(formationWidth, formationHeight, 0f));
    }

    public bool AllEnemiesDead() 
    {

        foreach (Transform spawnPoint in transform)
        {
            if (spawnPoint.transform.childCount != 0)
            {
                return false;
            }
        }

        return true;
    }

    public Transform NextFreePosition()
    {
        foreach (Transform spawnPoint in transform)
        {
            if (spawnPoint.transform.childCount == 0)
            {
                return spawnPoint;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.IsGamePaused())
        {
            if (!formationSpawned)
            {
                if (gameData.IsPlayerReady())
                {
                    SpawnFullFormation();
                    formationSpawned = true;
                }
            }
            if (AllEnemiesDead() && gameData.IsPlayerReady())
            {
                levelManager.ShowLevelComplete();
            }

            Vector3 newVector = transform.position;
            if (moveLeft)
            {
                newVector += Vector3.left * formationMovementAmount * Time.deltaTime;
                if (newVector.x <= offsetWidth)
                {
                    newVector.x = offsetWidth;
                    moveLeft = false;
                }
            }
            else
            {
                newVector += Vector3.right * formationMovementAmount * Time.deltaTime;
                if (newVector.x >= (rightEdge.x - offsetWidth))
                {
                    newVector.x = (rightEdge.x - offsetWidth);
                    moveLeft = true;
                }
            }

            transform.position = newVector;
        }
    }
}
