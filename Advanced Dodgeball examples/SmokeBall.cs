using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

public class SmokeBall : Ball {

    protected override void Awake()
    {
        id = "SmokeBall";
        myColor = Color.white;
        launchDelay = 0.4f;
        speed = 6;
        base.Awake();
    }

    public override void DestroyCommand()
    {
        GameObject smoke = Instantiate(Resources.Load("Smoke"), transform.position, transform.rotation) as GameObject;
        smoke.GetComponent<Smoke>().target = owner.gameObject;
        base.DestroyCommand();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        if (active)
        {
            if (owner.Controller.GetButtonUp(Controller.Buttons.Fire)
                && owner.GetComponentInChildren<LaunchManager>().GetSelectedBallId().Equals(id))
            {
                DestroyCommand();
            }
        }
    }
}
