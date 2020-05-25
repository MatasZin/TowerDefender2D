using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { ROCK, METAL, LARGEROCK, FIRE}

public abstract class Tower : MonoBehaviour
{
    private Monster target;
    public int Level { get; protected set; }
    public TowerUpgrade[] Upgrades { get; protected set; }
    public Element ElementType { get; protected set; }

    private SpriteRenderer mySpriteRenderer;

    private Queue<Monster> monsters = new Queue<Monster>();

    [SerializeField]
    private string projectileType;

    [SerializeField]
    private float projectileSpeed;

    private bool canAttack = true;

    private float attackTimer;

    public int Price { get; set; }

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float debuffDuration;

    [SerializeField]
    private float proc;

    public float ProjectileSpeed { get => projectileSpeed; }
    public Monster Target { get => target; }
    public int Damage { get => damage; }
    public float DebuffDuration { get => debuffDuration; set => debuffDuration = value; }
    public float Proc { get => proc; set => proc = value; }


    // Start is called before the first frame update
    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        Level = 1;
    }

    // Update is called once per frame
    void Update()
    {

        Attack();
        if (!GameManager.Instance.WaveActive) DropTargets();
    }

    public void Select()
    {
        //mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        GameManager.Instance.UpdateUpgradeTooptip();
    }

    private void Attack()
    {
        if (target != null)
        {
            if (!target.IsActive || !target.Alive)
            {
                target = null;
            }
        }
        if(target == null && monsters.Count > 0)
        {
            if (monsters.Peek().IsActive)
            {
                target = monsters.Dequeue();
            }
            else {
                monsters.Dequeue();
            }
        }
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= AttackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (Target != null && Target.IsActive)
        {
            if (canAttack)
            {
                Shoot();

                canAttack = false;
            }
        }
    }

    private void DropTargets()
    {
        target = null;
        monsters.Clear();
    }

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.ProcChance;
        this.DebuffDuration += NextUpgrade.DebuffDuration;
        Level++;
        GameManager.Instance.UpdateUpgradeTooptip();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            monsters.Enqueue(collision.GetComponent<Monster>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Monster" && Target != null)
        {
            target = null;
        }
    }

    public virtual string GetStats()
    {
        if(NextUpgrade != null)
        {
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00FFFFff>+{4}</color>\nProc Chance {2}% <color=#00FFFFff>+{5}%</color> \nDebuff: {3}sec <color=#00FFFFff>+{6}sec</color>",
                Level, damage, proc, debuffDuration,NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }
        return string.Format("\nLevel: {0} \nDamage: {1} \nProc Chance: {2}% \nDebuff: {3}sec", Level, damage, proc, DebuffDuration);
    }

    public TowerUpgrade NextUpgrade
    {
        get
        {
            if(Upgrades.Length > Level - 1)
            {
                return Upgrades[Level - 1];
            }
            return null;
        }
    }

    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

    public abstract Debuff GetDebuff();
}
