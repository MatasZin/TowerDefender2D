using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private int price;

    [SerializeField]
    private Text priceText;

    private void Start()
    {
        if(priceText != null)
        priceText.text = Price.ToString() + "<color=cyan>$</color>";

        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }

    public GameObject TowerPrefab { get { return towerPrefab; } }

    public Sprite Sprite { get { return sprite; } }

    public int Price { get { return price; } set => price = value; }

    private void PriceCheck()
    {
        if(price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceText.color = Color.white;
        } else
        {
            GetComponent<Image>().color = Color.grey;
            priceText.color = Color.grey;
        }
    }

    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;
        GameManager.Instance.DeSelectMonster();

        switch (type)
        {
            case "Rock":
                TowerRock rock = towerPrefab.GetComponentInChildren<TowerRock>();
                tooltip = string.Format("<color=#A0522Dff><size=20><b>Rock</b></size></color> \nDamage: {0} \nAttackSpeed: {4} \nProc Chance: {1}%\nDebuff duration: {2}sec\nSlowing factor: {3}%\nHas a chance to slow down the target",
                    rock.Damage, rock.Proc, rock.DebuffDuration, rock.SlowingFactror, 1/(float)rock.AttackCooldown);
                break;
            case "Large Rock":
                TowerLargeRock largeRock = towerPrefab.GetComponentInChildren<TowerLargeRock>();
                tooltip = string.Format("<color=#F4A460ff><size=20><b>Large Rock</b></size></color> \nDamage: {0} \nAttackSpeed: {3} \nProc Chance: {1}%\nDebuff duration: {2}sec\n Has a chance to stunn the target",
                    largeRock.Damage, largeRock.Proc, largeRock.DebuffDuration, 1/(float)largeRock.AttackCooldown);
                break;
            case "Fire":
                TowerFire fire = towerPrefab.GetComponentInChildren<TowerFire>();
                tooltip = string.Format("<color=#FF4500ff><size=20><b>Fire</b></size></color> \nDamage: {0} \nAttackSpeed: {5} \nProc Chance: {1}%\nDebuff duration: {2}sec \nTick time: {3} sec \nHp percantage damage: {4}%\nCan apply a BURN to the target",
                    fire.Damage, fire.Proc, fire.DebuffDuration, fire.TickTime, fire.TickDamge, 1/(float)fire.AttackCooldown);
                break;
            case "Metal":
                TowerMetal metal = towerPrefab.GetComponentInChildren<TowerMetal>();
                tooltip = string.Format("<color=#696969ff><size=20><b>Metal</b></size></color> \nDamage: {0} \nAttackSpeed: {5} \nProc Chance: {1}%\nDebuff duration: {2}sec \nTick time: {3} sec \nSplash damage: {4}\nCan apply dripping spikes",
                    metal.Damage, metal.Proc, metal.DebuffDuration, metal.TickTime, metal.SplashDamage, 1/(float)metal.AttackCooldown);
                break;
        }
        GameManager.Instance.SetTooltipText(tooltip);
        GameManager.Instance.ShowStats();
    }
}
