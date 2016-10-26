using UnityEngine;
using System.Collections;
using System;

public class IceBall : Ball {
    private float damage;
    // Use this for initialization
    protected override void Awake () {
        id = "IceBall";
        myColor = Color.cyan;
        launchDelay = 0.4f;
        speed = 7;
        damage = 10;
        OnDealDamage += SubDealDamage;
        OnInflictEffect += SubInflictEffect;
        base.Awake();
    }

    protected void SubDealDamage(IDamageable damageable)
    {
        damageable.TakeDamage(damage, owner);
    }

    protected void SubInflictEffect(GameObject affected)
    {
        //adds freeze effect
        FreezeEffect prevEffect = affected.GetComponent<FreezeEffect>();
        if (prevEffect != null)
        {
            prevEffect.Refresh();
        }
        else {
            affected.AddComponent<FreezeEffect>();
            affected.GetComponent<FreezeEffect>().StartEffect();
        }
    }
}
