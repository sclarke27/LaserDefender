using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyShipLarge;
    public GameObject EnemyShipMedium;
    public GameObject EnemyShipSmall;
    public float formationWidth = 10f;
    public float formationHeight = 5f;

    private float formationMovementAmount = 1.5f;
    private bool moveLeft = true;
    private float offsetWidth;
    private float distanceToCamera;
    private Vector3 rightEdge;
    private GameData gameData;

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

        foreach (Transform child in transform)
        {
            GameObject newEnemy;
            switch (child.gameObject.GetComponent<EnemySpawnPosition>().GetEnemyType())
            {
                case EnemyTypes.Large:
                    newEnemy = Instantiate(EnemyShipLarge, child.transform.position, child.transform.rotation) as GameObject;
                    newEnemy.transform.parent = child;
                    break;
                case EnemyTypes.Medium:
                    newEnemy = Instantiate(EnemyShipMedium, child.transform.position, child.transform.rotation) as GameObject;
                    newEnemy.transform.parent = child;
                    break;
                case EnemyTypes.Small:
                    newEnemy = Instantiate(EnemyShipSmall, child.transform.position, child.transform.rotation) as GameObject;
                    newEnemy.transform.parent = child;
                    break;
            }
        }

        offsetWidth = formationWidth / 2;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(formationWidth, formationHeight, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.IsGamePaused())
        {

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
