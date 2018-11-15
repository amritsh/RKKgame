using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameendScript : MonoBehaviour
{
    public Image mask;
    public Image endscreenbg;
    public Text yourscoretext;
    public Text gameovertext;
    public Text enternametext;
    public Text[] usernames;
    public Text[] scores;
    public Image[] icons;
    public InputField nameenter;
    public Button okbtn;
    public Button restartbtn;
    public Image leaderboardbg;
    private GameStatsController statsController;
    dreamloLeaderBoard dl;
    public float waittime = 1.8f;
    string dreamloWebserviceURL = "http://dreamlo.com/lb/";

    public string privateCode = "QIZikKktakaoUrzrH7ishwLkxGUNYz7kqLW5IWqxLKNw";
    public string publicCode = "5b47db44191a8a0bcc2694f3";

    string highScores = "";

    public struct Score
    {
        public string playerName;
        public int score;
        public int seconds;
        public string shortText;
        public string dateString;
    }

    // Use this for initialization
    void Start()
    {
        this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        statsController = GameObject.Find("Scripts").GetComponent<GameStatsController>();
        mask.enabled = false;
        endscreenbg.enabled = false;
        yourscoretext.enabled = false;
        gameovertext.enabled = false;
        enternametext.enabled = false;
        nameenter.gameObject.SetActive(false);
        okbtn.gameObject.SetActive(false);
        restartbtn.gameObject.SetActive(false);
        restartbtn.onClick.AddListener(restart);
        okbtn.onClick.AddListener(loadleaderboard);
        leaderboardbg.enabled = false;
        foreach (Image I in icons)
            I.enabled = false;
        foreach (Text te in usernames)
            te.enabled = false;
        foreach (Text t in scores)
            t.enabled = false;
    }

    public void dogameoveraction(){
        mask.enabled = true;
        endscreenbg.enabled = true;
        yourscoretext.enabled = true;
        yourscoretext.text = "SCORE : " + statsController.score.ToString();
        gameovertext.enabled = true;
        enternametext.enabled = true;
        nameenter.gameObject.SetActive(true);
        okbtn.gameObject.SetActive(true);
        restartbtn.gameObject.SetActive(true);
    }

    public void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void loadleaderboard(){
        dl.AddScore(nameenter.text, statsController.score);
        leaderboardbg.enabled = true;
        StartCoroutine(GetText());
        StartCoroutine(insertintoLeaderBoard());
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(dreamloWebserviceURL + publicCode + "/pipe"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                //Debug.Log(www.downloadHandler.text);
                highScores = www.downloadHandler.text;
            }
        }
    }

    public string[] ToStringArray()
    {
        Debug.Log("call to method");
        if (this.highScores == null) return null;
        if (this.highScores == "") return null;

        string[] rows = this.highScores.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log("rows0 : " + rows[0]);
        return rows;
    }

    public List<Score> ToListHighToLow()
    {
        Score[] scoreList = this.ToScoreArray();

        if (scoreList == null) return new List<Score>();

        List<Score> genericList = new List<Score>(scoreList);

        genericList.Sort((x, y) => y.score.CompareTo(x.score));

        return genericList;
    }

    public Score[] ToScoreArray()
    {
        string[] rows = ToStringArray();
        if (rows == null) return null;

        int rowcount = rows.Length;

        if (rowcount <= 0) return null;

        Score[] scoreList = new Score[rowcount];

        for (int i = 0; i < rowcount; i++)
        {
            string[] values = rows[i].Split(new char[] { '|' }, System.StringSplitOptions.None);

            Score current = new Score();
            current.playerName = values[0];
            current.score = 0;
            current.seconds = 0;
            current.shortText = "";
            current.dateString = "";
            if (values.Length > 1) current.score = CheckInt(values[1]);
            if (values.Length > 2) current.seconds = CheckInt(values[2]);
            if (values.Length > 3) current.shortText = values[3];
            if (values.Length > 4) current.dateString = values[4];
            scoreList[i] = current;
        }

        return scoreList;
    }


    string Clean(string s)
    {
        s = s.Replace("/", "");
        s = s.Replace("|", "");
        return s;

    }

    int CheckInt(string s)
    {
        int x = 0;

        int.TryParse(s, out x);
        return x;
    }

    IEnumerator insertintoLeaderBoard(){
        yield return new WaitForSeconds(waittime);
        List<Score> scoreList = ToListHighToLow();
        int i = 0;
        foreach (Score currentScore in scoreList)
        {
            if (i == 5) break;
            if (i < icons.Length)
                icons[i].enabled = true;
            usernames[i].enabled = true;
            usernames[i].text = (i+1).ToString() + ": " + currentScore.playerName;
            scores[i].enabled = true;
            scores[i].text = currentScore.score.ToString();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
