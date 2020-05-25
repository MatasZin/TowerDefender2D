using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMetal : Tower
{
    [SerializeField]
    private float tickTime;
    [SerializeField]
    private MetalSplash splashPrefab;
    [SerializeField]
    private int splashDamage;

    public int SplashDamage { get => splashDamage; set => splashDamage = value; }
    public float TickTime { get => tickTime; set => tickTime = value; }

    private void Start()
    {
        ElementType = Element.METAL;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(6,2,0.5f,5,0,1),
            new TowerUpgrade(15,3,0.5f,5,0,4),
            new TowerUpgrade(40,5,0.5f,10,-0.1f,5),
            new TowerUpgrade(100,5,0.5f,10,-0.2f,10)
        };
    }
    public override Debuff GetDebuff()
    {
        return new MetalDebuff(SplashDamage, TickTime, splashPrefab, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#696969ff>{0}</color>{1} \nTick time: {2}sec <color=#00FFFFff>{4}sec</color>\nSplash damage: {3} <color=#00FFFFff>+{5}</color>",
                "<size=20><b>Metal</b></size>", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=#696969ff>{0}</color>{1} \nTick time: {2}\nSplash damage: {3}", "<size=20><b>Metal</b></size>", base.GetStats(), TickTime, SplashDamage);
    }

    public override void Upgrade()
    {
        this.splashDamage += NextUpgrade.SpecialDamage;
        this.tickTime += NextUpgrade.TickTime;
        base.Upgrade();
    }
}
