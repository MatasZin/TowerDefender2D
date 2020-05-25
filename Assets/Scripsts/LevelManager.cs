using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    
    [SerializeField]
    private GameObject Tent;

    [SerializeField]
    private GameObject End;

    [SerializeField]
    private Transform map;

    private Point startSpawn, endSpawn;

    private Point mapSize;

    private Stack<Node> finalPath;

    private Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if(path == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    [SerializeField]
    public Dictionary<Point, TileScript> Tiles { get; set; }

    public Point mapNorth;
    public Point mapSouth;
    public Point mapWest;
    public Point mapEast;

    public Vector3 mapStart;
    public Vector3 mapStart2;
    public Vector3 mapEnd;

    public GameObject StartPortal;
    public GameObject EndPortal;

    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    public Point StartSpawn { get => startSpawn;}

    private void Start()
    {
        setMapSides();
        CreateLevel();
        //Tiles = new Dictionary<Point, TileScript>();
        //setMapTiles(mapStart, mapEnd);

    }

    private void Update()
    {

    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        string[] mapData = ReadLevelText(); ;
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);
        Point min = new Point(mapWest);
        Point max = new Point(mapNorth);
        MakeMap(min, max, mapData);
        mapStart = Tiles[new Point((float)11.5, (float)1.25)].transform.position;
        mapStart2 = Tiles[new Point((float)23, (float)9.5)].transform.position;
        mapEnd = Tiles[new Point(-11.5f, 0.25f)].transform.position;
        SpawnStartEnd();
    }

    private void PlaceTile(Point plase, string tileType, bool isCorner)
    {
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefabs[tileIndex] ).GetComponent<TileScript>();
        newTile.Setup(new Point(plase), new Vector3(TileSize * plase.X, TileSize * plase.Y, 0), map, isCorner, tileIndex);

        //Tiles.Add(new Point(plase), newTile);
    }

    private void MakeMap(Point start, Point end, string[] mapData)
    {
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;
        Point tempWestToSouth = new Point(start);
        for (int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            Point tempWestToNorth = new Point(tempWestToSouth);
            for (int x = 0; x < mapXSize; x++)
            {
                if(x==0 || x==(mapXSize-1) || y==0 || y == (mapYSize - 1))
                {
                    PlaceTile(tempWestToNorth, newTiles[x].ToString(), true);
                } else
                {
                    PlaceTile(tempWestToNorth, newTiles[x].ToString(), false);
                }
                tempWestToNorth.X += (float)0.5;
                tempWestToNorth.Y += (float)0.25;
            }
            tempWestToSouth.X += (float)0.5;
            tempWestToSouth.Y -= (float)0.25;
        }
    }

    private string[] ReadLevelText()
    {
        TextAsset binddata = Resources.Load("Level1") as TextAsset;
        string data = binddata.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-') ;
    }

    private void SpawnStartEnd()
    {
        startSpawn = new Point(mapStart.x, mapStart.y);
        GameObject tmp = Instantiate(Tent, Tiles[StartSpawn].transform.position, Quaternion.identity);
        StartPortal = tmp;
        StartPortal.name = "StartPortal";

        endSpawn = new Point(mapEnd.x, mapEnd.y);
        GameObject tmp2 = Instantiate(End, Tiles[endSpawn].transform.position, Quaternion.identity);
        EndPortal = tmp2;
        EndPortal.name = "EndPortal";
    }

    public void NextLevel()
    {
        Destroy(StartPortal);
        startSpawn = new Point(mapStart2.x, mapStart2.y);
        GameObject tmp = Instantiate(Tent, Tiles[StartSpawn].transform.position, Quaternion.identity);
        StartPortal = tmp;
        StartPortal.name = "StartPortal";
    }

    public void testTiles()
    {
        /*foreach (Point tile in Tiles)
        {

        }*/
    }
    /*
    public void setMapTiles(Point start, Point end)
    {
        float xSize = start.X - end.X;
        float ySize = -start.Y + start.Y;
        
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                Point pos = new Point((start.X - i), start.Y + j);
                TileScript tile = new TileScript();
                tile.Setup(pos, new Vector3(start.X - (TileSize * pos.X), start.Y + (TileSize * pos.Y), 0));
                //Tiles.Add(pos, tile);
            }
        }
    }*/
    

    public void setMapSides()
    {
        mapNorth = new Point(0, (float) 6.5);
        mapSouth = new Point((float) -0.5, (float) -5.75);
        mapEast = new Point(12, (float) 0.5);
        mapWest = new Point((float) -12.5, (float) 0.25);
    }

    public void GeneratePath()
    {
        path = Astar.GetPath(StartSpawn, endSpawn);
    }
    
}
