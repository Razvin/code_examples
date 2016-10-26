using UnityEngine;
using System.Collections;
using System;

public class Mine : Ball
{
    bool grown;

    protected override void Awake()
    {
        id = "Mine";
        myColor = new Color(180 / 255f, 64 / 255f, 16 / 255f);
        launchDelay = 1.5f;
        speed = 6;
        grown = false;
        OnDealDamage += SubDealDamage;
        base.Awake();
    }

    protected void SubDealDamage(IDamageable damageable)
    {
        DestroyCommand();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (active)
        {
            if (owner.Controller.GetButtonUp(Controller.Buttons.Fire) && !grown
                && owner.GetComponentInChildren<LaunchManager>().GetSelectedBallId().Equals(id))
            {
                rigid.velocity = new Vector2(0f, 0f);
                this.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f, transform.localScale.z);
                grown = true;
            }

            if (owner.Controller.GetButtonDown(Controller.Buttons.Fire) && grown
                && owner.GetComponentInChildren<LaunchManager>().GetSelectedBallId().Equals(id))
            {
                DestroyCommand();
            }
        }
    }

    public override void DestroyCommand()
    {
        GameObject explosion = Instantiate(Resources.Load("MineExplosion"), transform.position, transform.rotation) as GameObject;
        explosion.GetComponent<Explosion>().owner = owner;
        if (grown)
        {
            explosion.GetComponent<Explosion>().explosionRadius = 1.5f;
            explosion.GetComponent<Explosion>().damage = 20;
        }
        base.DestroyCommand();
    }
}
