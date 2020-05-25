using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : Singleton<HighScoreTable>
{
    [SerializeField]
    private Transform entryContainer;
    [SerializeField]
    private Transform entryTemplate;
    
    private List<Transform> highScoreEntryTransformList;

    private void Awake()
    {
        if (entryContainer != null && entryTemplate != null)
        {
            string jsonString = PlayerPrefs.GetString("highscoreTable");
            HigScores HighScores = JsonUtility.FromJson<HigScores>(jsonString);
            if (HighScores != null && HighScores.highScoreEntriesList.Count > 0)
            {
                HighScores.highScoreEntriesList.Sort();
                if (HighScores.highScoreEntriesList.Count > 10)
                {
                    HighScores.highScoreEntriesList.RemoveRange(10, HighScores.highScoreEntriesList.Count - 10);
                }
                highScoreEntryTransformList = new List<Transform>();
                foreach (HighScoreEntry highScoreEntry in HighScores.highScoreEntriesList)
                {
                    CreateHighscoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
                }
            }
        }
    }

    private void CreateHighscoreEntryTransform(HighScoreEntry highscore, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, 100 - (20 * transformList.Count));
        entryRectTransform.gameObject.SetActive(true);

        Image Trophy = entryTransform.Find("Trophy").GetComponent<Image>();
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1: rankString = "1St"; Trophy.color = new Color32(254, 225, 1, 255); Trophy.enabled = true; break;
            case 2: rankString = "2Nd"; Trophy.color = new Color32(215, 215, 215, 255); Trophy.enabled = true; break;
            case 3: rankString = "3Rd"; Trophy.color = new Color32(130, 74, 2, 255); Trophy.enabled = true; break;
            default: rankString = rank.ToString() + "Th"; break;
        }
        entryTransform.Find("Place").GetComponent<Text>().text = rankString;

        int score = highscore.score;
        entryTransform.Find("Score").GetComponent<Text>().text = score.ToString();

        string name = highscore.name;
        entryTransform.Find("Name").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, string name)
    {
        HighScoreEntry newScore = new HighScoreEntry { score = score, name = name };
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HigScores HighScores = JsonUtility.FromJson<HigScores>(jsonString);
        if(HighScores == null || HighScores.highScoreEntriesList == null)
        {
            HighScores = new HigScores { highScoreEntriesList = new List<HighScoreEntry>()};
        }
        HighScores.highScoreEntriesList.Add(newScore);
       
        string json = JsonUtility.ToJson(HighScores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }


    private class HigScores
    {
        public List<HighScoreEntry> highScoreEntriesList;
    }

    [System.Serializable]
    private class HighScoreEntry : IComparable<HighScoreEntry>
    {
        public int score;
        public string name;

        public int CompareTo(HighScoreEntry other)
        {
            if (this.score > other.score) return -1;
            else if (this.score == other.score) return 0;
            else return 1;
        }
        
    }
}
