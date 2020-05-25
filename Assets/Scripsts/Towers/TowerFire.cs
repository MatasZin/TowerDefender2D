using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFire : Tower
{
    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamge;

    public float TickTime { get => tickTime; set => tickTime = value; }
    public float TickDamge { get => tickDamge; set => tickDamge = value; }

    private void Start()
    {
        ElementType = Element.FIRE;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(10,2,.5f,5,0,1),
            new TowerUpgrade(20,3,.5f,5,0,1),
            new TowerUpgrade(50,5,0,0,-0.1f,1),
            new TowerUpgrade(100,10,0,10,-0.2f,2)
        };
    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamge, TickTime, DebuffDuration,  Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null) //If the next is avaliable
        {
            return string.Format("<color=#FF4500ff>{0}</color>{1} \nTick time: {2}sec <color=#00FFFFff>{4}sec</color>\nTick damage: {3}% <color=#00FFFFff>+{5}%</color>", 
                "<size=20><b>Fire</b></size> ", base.GetStats(), TickTime, TickDamge, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        //Returns the current upgrade
        return string.Format("<color=#FF4500ff>{0}</color>{1} \nTick time: {2}\nTick damage: {3}", "<size=20><b>Fire</b></size> ", base.GetStats(), TickTime, TickDamge);
    }


    public override void Upgrade()
    {
        this.tickTime += NextUpgrade.TickTime;
        this.TickDamge += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}
