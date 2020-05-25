using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLargeRock : Tower
{
    private void Start()
    {
        ElementType = Element.LARGEROCK;
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(8,2,0.5f,5),
            new TowerUpgrade(20,3,0.5f,5),
            new TowerUpgrade(50,10,0.5f,5),
            new TowerUpgrade(100,20,0.5f,10)
        };
    }
    public override Debuff GetDebuff()
    {
        return new LargeRockDebuf(DebuffDuration,Target);
    }

    public override string GetStats()
    {
        return string.Format("<color=#F4A460ff><size=20><b>{0}</b></size></color> {1}", "Large Rock", base.GetStats());
    }
}
