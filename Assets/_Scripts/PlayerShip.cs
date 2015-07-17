using UnityEngine;
using System.Collections;

public class PlayerShip : BaseGameObject
{

    public float shipScreenOffset = 0.172f;

    public float maxVelocity = 15.0f;
    public float acclerationAmount = 0.1f;
    public float declerationAmount = 0.1f;
    public Animator shipAnimator;
    public GameObject defaultProjectile;
    public int maxHealth = 100;
    private int currentHealth;

    private float shipVelocity = 0f;
    private float currAccleration = 0f;
    private float currStep = 0;


    // Use this for initialization
    new void Start()
    {
        base.Start();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (!gameData.IsGamePaused())
        {

            float newVelocityX = 0.0f;

            if (gameData.GetAIEnabled())
            {
            }

            if (((gameData.IsLeftPaddledown() && !gameData.IsRightPaddledown()) || (gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown())) && currStep < 1f)
            {
                currStep = currStep + acclerationAmount;
            }

            if (!gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown() && currStep > 0)
            {
                currStep = currStep - declerationAmount;
            }

            //Lerp acceleration amount
            currAccleration = Mathf.Lerp(0f, maxVelocity, currStep);
            //Debug.Log(currAccleration);

            //accerate left
            if (gameData.IsLeftPaddledown() && !gameData.IsRightPaddledown())
            {
                shipAnimator.SetBool("moveLeft", true);
                shipAnimator.SetBool("moveRight", false);
                if (shipVelocity > 0f)
                {
                    shipVelocity = 0f;
                    currAccleration = 0f;
                    currStep = 0f;
                }
                newVelocityX = (currAccleration * -1);
                if (newVelocityX < (maxVelocity * -1))
                {
                    newVelocityX = maxVelocity * -1;
                }
            }

            //accerate right
            if (gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown())
            {
                shipAnimator.SetBool("moveLeft", false);
                shipAnimator.SetBool("moveRight", true);
                if (shipVelocity < 0f)
                {
                    shipVelocity = 0f;
                    currAccleration = 0f;
                    currStep = 0f;
                }
                newVelocityX = currAccleration;
                if (newVelocityX > maxVelocity)
                {
                    newVelocityX = maxVelocity;
                }
            }

            //decelerate
            if (!gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown() && shipVelocity != 0.0f)
            {
                shipAnimator.SetBool("moveLeft", false);
                shipAnimator.SetBool("moveRight", false);

                //currAccleration = 0f;
                if (shipVelocity > 0.0f)
                {
                    newVelocityX = currAccleration;
                    if (newVelocityX < 0f)
                    {
                        newVelocityX = 0;
                        currAccleration = 0f;
                        currStep = 0;
                    }
                }
                else if (shipVelocity < 0.0f)
                {
                    newVelocityX = (currAccleration * -1);
                    if (newVelocityX > 0f)
                    {
                        newVelocityX = 0;
                        currAccleration = 0f;
                        currStep = 0f;
                    }
                }
            }
            if (!gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown() && shipVelocity == 0.0f)
            {
                shipAnimator.SetBool("moveLeft", false);
                shipAnimator.SetBool("moveRight", false);
                currAccleration = 0f;
                currStep = 0f;
            }
            //stop at wall
            Vector2 maxBounds = gameData.GetMaxScreenBounds();
            Vector2 minBounds = gameData.GetMinScreenBounds();
            if ((this.transform.position.x >= maxBounds.x && newVelocityX > 0) || (this.transform.position.x <= minBounds.x && newVelocityX < 0))
            {
                newVelocityX = 0.0f;
                Vector2 paddlePos = this.transform.position;
                paddlePos.x = Mathf.Clamp(paddlePos.x, minBounds.x, maxBounds.x);
                this.transform.position = paddlePos;
                currAccleration = 0f;
            }

            //set paddle velocity
            shipVelocity = newVelocityX;
            this.rigidbody2D.velocity = new Vector2(shipVelocity, 0.0f);
        }
        if (finishDestruction)
        {
            DestroyGameObject();
        }

    }


    new public void DestroyGameObject()
    {
        gameData.LoseOneLife();
        gameData.SetPlayerReady(false);
        base.DestroyGameObject();

    }

    void OnTriggerEnter2D(Collider2D collidingObject)
    {
        if (collidingObject.name.IndexOf("Enemy") >= 0 && collidingObject.name.IndexOf("Projectile") >= 0)
        {
            Projectile projectile = collidingObject.gameObject.GetComponent<Projectile>() as Projectile;
            currentHealth = currentHealth - projectile.GetDestructionAmount();
            Debug.Log("player hit: " + projectile.GetDestructionAmount());
            if (destructionSound != null)
            {
                AudioSource.PlayClipAtPoint(destructionSound, this.transform.position, gameData.GetSFXVolume());
            }

            shipAnimator.SetTrigger("Destroyed");

        }
    }

    public void FireProjectile()
    {
        if (!gameData.IsGamePaused() && gameData.IsPlayerReady()  && !destroyed)
        {
            if (firingSound != null)
            {
                AudioSource.PlayClipAtPoint(firingSound, this.transform.position, gameData.GetSFXVolume());
            }
            Instantiate(defaultProjectile, transform.position, transform.rotation);
        }
    }

}
