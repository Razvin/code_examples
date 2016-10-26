using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Ball : MonoBehaviour
{
    protected string id;
    protected float launchDelay;
    protected float speed;
    protected bool active;
    protected Rigidbody2D rigid;
    protected Vector2 pos2d;
    protected PlayerController owner;
    protected Color myColor;

    protected virtual void Awake()
    {
        //the sub class start should run before the base start in order for all of this to work
        rigid = GetComponent<Rigidbody2D>();
        pos2d = new Vector2(transform.position.x, transform.position.y);
        active = false;
        gameObject.GetComponent<SpriteRenderer>().color = myColor;
        GetComponent<Light>().color = GetComponent<SpriteRenderer>().color;
    }

    protected virtual void Update()
    {
        pos2d = new Vector2(transform.position.x, transform.position.y);
    }

    public virtual void Launch(Vector2 target)
    {
        pos2d = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = target - pos2d;
        this.transform.position = target;
        pos2d = new Vector2(transform.position.x, transform.position.y);
        direction = direction.normalized;
        rigid.velocity = direction * speed;
        transform.parent = null;
        active = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        MonoBehaviour[] behaviors = col.gameObject.GetComponents<MonoBehaviour>();
        //ignores the collision if the other object was the owner of the ball
        if (col.gameObject.tag.Equals("Player") && col.gameObject.GetComponent<PlayerController>().playerID == owner.playerID)
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        }
        else
        {
            foreach (MonoBehaviour mb in behaviors)
            {
                if (mb is Ball)
                {
                    //ignores the collision if either ball is inactive or the other ball belongs to the player
                    if (((Ball)mb).Owner.playerID == owner.playerID || !((Ball)mb).Active || !active)
                    {
                        Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
                    }
                    else
                    {
                        ((Ball)mb).DestroyCommand();
                        DestroyCommand();
                    }
                }

                //checks to see if collision is damageable and if the ball has a method to deal damage
                if (OnDealDamage != null && mb is IDamageable)
                {
                    OnDealDamage(mb as IDamageable);
                }
            }

            //adds a lingering effect to enemy player if the ball has one to inflict
            if (OnInflictEffect != null && col.gameObject.tag.Equals("Player"))
            {
                OnInflictEffect(col.gameObject);
            }
            //if the collision is not ignored then the ball should be destroyed
            DestroyCommand();
        }
    }

    public string ID
    {
        get
        {
            return id;
        }
    }

    public float LaunchDelay
    {
        get
        {
            return launchDelay;
        }
    }

    public PlayerController Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    public bool Active
    {
        get
        {
            return active;
        }
    }

    public Color MyColor
    {
        get
        {
            return myColor;
        }
    }

    public virtual void DestroyCommand()
    {
        Destroy(gameObject);
    }

    //delegate pattern for dealing damage
    protected delegate void DealDamage(IDamageable damageable);
    //called when ball can inflict damage
    protected DealDamage OnDealDamage;
    //delegate pattern for inflicting effects
    protected delegate void InflictEffect(GameObject affected);
    //called when ball can inflict a lingering effect
    protected InflictEffect OnInflictEffect;
}

