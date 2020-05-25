using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public delegate void CurrencyChanged();
public delegate void ScoreChanged();

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject blur;
    [SerializeField]
    private GameObject statsPanel;
    [SerializeField]
    private Text SizeText;
    [SerializeField]
    private Text statText;
    [SerializeField]
    private GameObject inGameMenu;
    [SerializeField]
    private GameObject OptionsMenu;
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text GameOverScoreText;

    [SerializeField]
    private GameObject ExpandButton;
    private bool expanded = false;
    
    private float score;
    public event ScoreChanged scoreChanged;
    public event CurrencyChanged Changed;

    public TowerBtn ClickedBtn { get;  set; }

    public ObjectPool Pool { get; set; }

    private int currency;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private Text sellText;

    [SerializeField]
    private Text upgradeText;

    [SerializeField]
    private Text currencyText;

    private int wave = 0;

    private int lives;

    [SerializeField]
    private Text livesText;

    [SerializeField]
    private Text waveText;

    [SerializeField]
    private GameObject waveBtn;

    List<Monster> activeMonsters = new List<Monster>();

    private bool gameOver = false;

    [SerializeField]
    private GameObject gameOverMenu;

    private Tower selectedTower;
    private Monster selectedMonster;

    private int health;

    public bool WaveActive
    {
        get { return activeMonsters.Count > 0; }
    }

    public int Currency
    {
        get { return currency; }
        set
        {
            this.currency = value;
            this.currencyText.text = value.ToString() + "<color=cyan>$</color>";

            OnCurrencyChanged();
        }
    }

    public int Lives { get => lives; set
        {
            lives = value;
            if (lives <= 0)
            {
                lives = 0;
                GameOver();
            }
            livesText.text = lives.ToString();
        }
    }

    public float Score
    {
        get => score; set
        {
            this.score = value;
            this.ScoreText.text = "Score: " + value.ToString();
            OnScoreChanged();
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        Score = 0;
        Lives = 5;
        Currency = 500;
        health = 15;
    }

    private void Update()
    {
        HandleEscape();
        if (selectedMonster != null)
        {
            UpdateMonsterStatsTooltip();
        }
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price)
        {
            DeSelectMonster();
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
    }

    public void BuyTower()
    {
        if(Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
            DeSelectMonster();
        }
    }

    public void StartWave()
    {
        wave++;

        waveText.text = string.Format("Wave: <color=aqua>{0}</color>", wave);

        waveBtn.SetActive(false);

        if (!expanded)
        {
            ExpandButton.SetActive(false);
        }

        StartCoroutine(SpawnWave());

    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();
        if (wave < 10)
        {
                health += 5;
        } else
        {
            health += wave*(wave/10);
        }

        int monsterType = 1;
        List<int> waveMonsters = new List<int>();
        int batCount = 1, orcCount = 1;
        float batOrc;
        for (int i = 0; i < wave; i++)
        {
            //monster type 0-orc 1-bat
            batOrc = batCount / (float)wave;
            if (batOrc < 0.4f && batCount < orcCount)
            {
                monsterType = Random.Range(0, 2);
            }
            else monsterType = 0;

            //monster type
            if (monsterType == 0)
                waveMonsters.Add(Random.Range(0, 3));
            else waveMonsters.Add(Random.Range(3, 5));

            //monster count
            if (monsterType == 0) orcCount++;
            else batCount++;
        }
        foreach (int monsterIndex in waveMonsters)
        {
            float monsterHealth = 0;
            int monsterKillGold;
            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "Orc1";
                    monsterHealth = health * 1.5f;
                    break;
                case 1:
                    type = "Orc2";
                    monsterHealth = health * 2;
                    break;
                case 2:
                    type = "Orc3";
                    monsterHealth = health * 3;
                    break;
                case 3:
                    type = "Bat1Icon";
                    monsterHealth = health;
                    break;
                case 4:
                    type = "Bat2Icon";
                    monsterHealth = health * 0.5f;
                    break;
            }
            monsterKillGold = 2 + Mathf.RoundToInt(health / 30);
            if (monsterKillGold > 10) monsterKillGold = 10;
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monsterHealth = Mathf.RoundToInt(monsterHealth);
            monster.Spawn(monsterHealth, monsterKillGold);
            activeMonsters.Add(monster);
            yield return new WaitForSeconds(1f);
        }
        
    }

    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
            if (!expanded)
            {
                ExpandButton.SetActive(true);
            }
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
        }
        blur.SetActive(true);
        inGameMenu.SetActive(false);
        Time.timeScale = 0;
        GameOverScoreText.text = Score.ToString();
        HighScoreTable.Instance.AddHighscoreEntry(Mathf.RoundToInt(Score), PlayerPrefs.GetString("Name"));
        gameOverMenu.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Lives = 10;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        selectedTower.Select();

        sellText.text = "+ " + (selectedTower.Price / 2).ToString() + "$";

        upgradePanel.SetActive(true);
    }

    public void DeSelectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
        upgradePanel.SetActive(false);
    }

    public void SellTower()
    {
        if(selectedTower != null)
        {
            Currency += selectedTower.Price / 2;

            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;

            Destroy(selectedTower.transform.parent.gameObject);

            DeSelectTower();
        }
    }

    public void OnCurrencyChanged()
    {
        if(Changed != null)
        {
            Changed();
        }
    }

    public void OnScoreChanged()
    {
        if(scoreChanged != null)
        {
            scoreChanged();
        }
    }

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void SetTooltipText(string txt)
    {
        statText.text = txt;
        SizeText.text = txt;
    }

    public void UpdateUpgradeTooptip()
    {
        if(selectedTower != null)
        {
            sellText.text = "+" + (selectedTower.Price / 2).ToString() + "$"; 
            SetTooltipText(selectedTower.GetStats());

            if(selectedTower.NextUpgrade != null)
            {
                upgradeText.text = selectedTower.NextUpgrade.Price.ToString() + "$";
            }else
            {
                upgradeText.text = string.Empty;
            }
        }
    }

    public void SelectMonster(Monster monster)
    {
        if (selectedMonster == null)
        {
            selectedMonster = monster;
            UpdateMonsterStatsTooltip();
            ShowStats();
        }
    }

    public void DeSelectMonster()
    {
        if(selectedMonster != null)
        {
            selectedMonster = null;
            ShowStats();
        }
    }
    
    public void UpdateMonsterStatsTooltip()
    {
        if (selectedMonster.IsActive) { SetTooltipText(selectedMonster.GetMonsterInfo()); }
        else { DeSelectMonster(); }
    }

    public void ShowSelectedTowerStats()
    {
        ShowStats();
        UpdateUpgradeTooptip();
    }

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

    public void ShowInGameMenu()
    {
        if (OptionsMenu.activeSelf)
        {
            OptionsGoBack();
        } else {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
            if (!inGameMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    public void ShowOptions()
    {
        inGameMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void OptionsGoBack()
    {
        inGameMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void ExpandMap()
    {
        ExpandButton.SetActive(false);
        expanded = true;
        LevelManager.Instance.NextLevel();
    }
}
