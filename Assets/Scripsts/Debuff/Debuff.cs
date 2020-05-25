using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    protected Monster target;

    private float duration;

    private float elapsed;

    public Debuff(Monster target, float duration)
    {
        elapsed = 0;
        this.duration = duration;
        this.target = target;
    }

    public virtual void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= duration)
        {
            Remove();
        }
    }

    public virtual void Remove()
    {
        if (target != null)
        {
            target.RemoveDebuff(this);
        }
    }
}
