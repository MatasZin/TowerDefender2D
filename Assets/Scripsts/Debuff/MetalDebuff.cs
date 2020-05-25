using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDebuff : Debuff
{
    private float timeSinceTick;
    private float tickTime;
    
    private MetalSplash splashPrefab;

    private int splashDamage;

    public MetalDebuff(int splashDamage, float tickTime, MetalSplash splashPrefab, float duration, Monster target) : base(target, duration)
    {
        this.splashDamage = splashDamage;
        this.tickTime = tickTime;
        this.splashPrefab = splashPrefab;
    }

    public override void Update()
    {
        base.Update();
        if(target != null)
        {
            timeSinceTick += Time.deltaTime;
            if(timeSinceTick >= tickTime)
            {
                Splash();
                timeSinceTick = 0;
            }
        }
    }

    private void Splash()
    {
        MetalSplash tmp = GameObject.Instantiate(splashPrefab, target.transform.position, Quaternion.identity);
        tmp.Damage = splashDamage;
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }
}
