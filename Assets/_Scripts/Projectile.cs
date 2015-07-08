using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    public enum ProjectileTypes
    {
        player,
        enemy
    }

    public ProjectileTypes projectileType = ProjectileTypes.player;
    public float movementSpeed = 10f;
    public int destructionAmount = 100;

    // Update is called once per frame
    void Update()
    {
        if (projectileType == ProjectileTypes.player)
        {
            transform.position += Vector3.up * movementSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * movementSpeed * Time.deltaTime;
        }
    }

    public int GetDestructionAmount()
    {
        return destructionAmount;
    }

    void OnTriggerEnter2D(Collider2D collidingObject) 
    {

    }
}
