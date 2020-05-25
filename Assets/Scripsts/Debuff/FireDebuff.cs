using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuff
{
    private float tickTime;
    private float timeSinceTick;
    private float tickDamage;
    
    public FireDebuff(float tickDamage, float tickTime, float duration, Monster target) : base(target, duration)
    {
        timeSinceTick = 0;
        this.tickDamage = tickDamage*target.Health.MaxVal/100;
        this.tickTime = tickTime;
    }

    public override void Update()
    {
        if(target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick > tickTime)
            {
                timeSinceTick = 0;
                target.TakeDamage(tickDamage, Element.FIRE, true);
            }
        }
        base.Update();
    }
}
