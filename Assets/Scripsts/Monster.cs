using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Element elementType;

    [SerializeField]
    private ParticleSystem Bleed;

    [SerializeField]
    private ParticleSystem DebuffBleed;

    private Stack<Node> path;

    public Point GridPosition { get; set; }

    private Vector3 destination;

    public bool IsActive { get; set; }

    private Animator myAnimator;

    public float  invulnerability { get; set; }

    public int KillGold { get; set; }

    private List<Debuff> debuffs = new List<Debuff>();
    private List<Debuff> debuffsToRemove = new List<Debuff>();
    private List<Debuff> debuffsToAdd = new List<Debuff>();

    private string monstersInfo;

    public bool Alive
    {
        get { return Health.CurrentValue > 0; }
    }

    public Element ElementType { get => elementType; }
    internal Stat Health { get => health; }
    public float Speed { get => speed; set => speed = value; }
    public float MaxSpeed { get; set; }

    [SerializeField]
    private Stat health;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        MaxSpeed = speed;
        Health.Initialize();
    }

    private void Update()
    {
        Move();
        HandleDebuffs();
    }

    public void Spawn(float health, int killGold)
    {
        speed = MaxSpeed;
        IsActive = false;
        transform.position = LevelManager.Instance.StartPortal.transform.position;

        this.Health.Bar.Resset();
        invulnerability = 1;
        this.Health.MaxVal = health;
        this.KillGold = killGold;
        this.Health.CurrentValue = this.Health.MaxVal;

        StartCoroutine(Scale(new Vector3(0.1f,0.1f), new Vector3(1,1), false));
        GetComponent<SpriteRenderer>().sortingOrder = 17;
        SetPath(LevelManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float progress = 0;
        while(progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;

        IsActive = true;

        if (remove)
        {
            Release();
        }
    }

    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPostion);
                    GridPosition = path.Peek().GridPostion;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }

    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.path = newPath;

            Animate(GridPosition, path.Peek().GridPostion);

            GridPosition = path.Peek().GridPostion;

            destination = path.Pop().WorldPosition;
        }
    }

    private void Animate(Point currentPos, Point newPos)
    {
        Vector3 theScale = transform.localScale;
        if (currentPos.X > newPos.X)
        {
            //mmoving left
            theScale.x = -1;
        }
        else
        {
            //moving rigth
            theScale.x = 1;
        }
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EndPortal")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));

            GameManager.Instance.Lives--;
        }
    }

    public void Release()
    {
        debuffs.Clear();
        IsActive = false;
        GridPosition = LevelManager.Instance.StartSpawn;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this);

    }

    public void TakeDamage(float damage, Element dmgSource, bool isDebuff)
    {
        if (IsActive)
        {
            if (dmgSource == ElementType)
            {
                damage = damage - invulnerability;
                //invulnerability++;
            }
            Health.CurrentValue -= damage;

            if (isDebuff)
            {
                if(DebuffBleed != null)
                {
                    DebuffBleed.Play();
                }
            }
            else
            {
                if(Bleed != null)
                {
                    Bleed.Play();
                }
            }

            if(Health.CurrentValue <= 0)
            {
                GameManager.Instance.Currency += KillGold;
                GameManager.Instance.Score += health.MaxVal;

                SoundManager.Instance.PlaySFX("Die2");

                myAnimator.SetTrigger("Die");

                IsActive = false;

                GetComponent<SpriteRenderer>().sortingOrder--;
            }
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        if(!debuffs.Exists(x => x.GetType() == debuff.GetType()))
        {
            debuffsToAdd.Add(debuff);
        }
        
    }

    public void RemoveDebuff(Debuff debuff)
    {
        debuffsToRemove.Add(debuff);
    }

    private void HandleDebuffs()
    {
        if(debuffsToAdd.Count > 0)
        {
            debuffs.AddRange(debuffsToAdd);
            debuffsToAdd.Clear();
        }

        foreach (Debuff debuff in debuffsToRemove)
        {
            debuffs.Remove(debuff);
            
        }

        debuffsToRemove.Clear();

        foreach(Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }

    public string GetMonsterInfo()
    {
        return string.Format("Name: {3} \nImune To: {2} Debuff \nMax HP: {0} \nCurrent HP: {1}", Health.Bar.MaxValue, Health.CurrentValue, ElementType.ToString(), transform.name);
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.SelectMonster(this);
        }

    }


}
