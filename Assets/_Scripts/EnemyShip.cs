using UnityEngine;
using System.Collections;

public class EnemyShip : BaseGameObject
{

    public GameObject defaultProjectile;
    public int maxHealth = 200;
    public int scoreValue = 1000;
    private float firingYOffset = -0.25f;
    private Animator shipAnimator;
    public bool fire = false;
    public bool destroy = false;

    private int currentHealth;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        shipAnimator = GetComponent<Animator>();
        shipAnimator.SetTrigger("Arrival");
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (!gameData.IsGamePaused())
        {
            if (destroy)
            {
                gameData.AddPlayerScore(scoreValue);
                Destroy(gameObject);
                return;
            }
            int randVal = Random.Range(0, 300);
            if (randVal == 50)
            {
                FireProjectile();
                Debug.Log("fire");
            }
            if (fire)
            {
                DoProjectileFiring();
                fire = false;
            }
        }
    }

    public void FireProjectile()
    {
        if (gameData.IsPlayerReady())
        {
            shipAnimator.SetTrigger("FireWeapons");
            //Instantiate(defaultProjectile, transform.position, Quaternion.identity);
        }
    }

    public void DoProjectileFiring()
    {
        if (firingSound != null)
        {
            AudioSource.PlayClipAtPoint(firingSound, this.transform.position, gameData.GetSFXVolume());
        }
        Instantiate(defaultProjectile, new Vector3(transform.position.x, transform.position.y + firingYOffset, transform.position.z), Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D collidingObject)
    {
        if (collidingObject.name.IndexOf("Projectile") >= 0 && !destroy)
        {
            Projectile projectile = collidingObject.gameObject.GetComponent<Projectile>() as Projectile;
            currentHealth = currentHealth - projectile.GetDestructionAmount();
            Debug.Log("enemy hit: " + projectile.GetDestructionAmount());

            if (currentHealth > 0)
            {
                shipAnimator.SetTrigger("TookDamage");
            }
            else
            {
                if (destructionSound != null)
                {
                    AudioSource.PlayClipAtPoint(destructionSound, this.transform.position, gameData.GetSFXVolume());
                }
                shipAnimator.SetTrigger("Destroyed");
            }
        }
    }
}
