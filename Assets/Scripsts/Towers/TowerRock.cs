using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRock : Tower
{
    [SerializeField]
    private float slowingFactror;

    public float SlowingFactror { get => slowingFactror; set => slowingFactror = value; }

    private void Start()
    {
        ElementType = Element.ROCK;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(4,3,0.2f,5,5),
            new TowerUpgrade(15,3,0.2f,5,5),
            new TowerUpgrade(50,4,0.3f,0,5),
            new TowerUpgrade(100,10,0.3f,10,10)
        };
    }

    public override Debuff GetDebuff()
    {
        return new RockDebuff(SlowingFactror, DebuffDuration,Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return string.Format("<color=#A0522Dff>{0}</color>{1} \nSlowing factor: {2}% <color=#00FFFFff>+{3}%</color>", 
                "<size=20><b>Rock</b></size>", base.GetStats(), SlowingFactror, NextUpgrade.SlowingFactor);
        }

        //Returns the current upgrade
        return string.Format("<color=#A0522Dff>{0}</color>{1} \nSlowing factor: {2}%", "<size=20><b>Rock</b></size>", base.GetStats(), SlowingFactror);
    }

    public override void Upgrade()
    {
        this.slowingFactror += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}
