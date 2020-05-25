using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{

    [SerializeField]
    public Point GridPosition { get; set; }

    public Vector2 WorldPosition { get; set; }

    private Color32 fullColor = new Color32(255, 118, 118, 255);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    public bool IsEmpty { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsCorner { get; private set; }

    private SpriteRenderer spriteRenderer;

    public bool Debugging { get; set; }

    private Tower myTower;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent, bool isCorner, int tileType)
    {
        
        if(tileType == 0 || tileType == 2)
        {
            IsWalkable = false;
            IsEmpty = true;
        } else
        {
            IsWalkable = true;
            IsEmpty = false;
        }
        this.IsCorner = isCorner;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        this.WorldPosition = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            if (IsEmpty && !Debugging)
            {
                ColorTile(emptyColor);
            } if(!IsEmpty && !Debugging)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
            
        }else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if(myTower!= null)
            {
                GameManager.Instance.DeSelectMonster();
                GameManager.Instance.SelectTower(myTower);
            } else
            {
                GameManager.Instance.DeSelectMonster();
                GameManager.Instance.DeSelectTower();
            }
        }
        
    }

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }

    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        //tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y.ToString();

        tower.transform.SetParent(transform);



        this.IsEmpty = false;
        this.IsWalkable = false;
        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();

        myTower.Price = GameManager.Instance.ClickedBtn.Price;
        ColorTile(Color.white);

        GameManager.Instance.BuyTower();
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
