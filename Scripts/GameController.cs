using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Transform[] goonsprefab;
    public float spawnCoolDownTime = 1.0f;
    public float COOLDOWN_THRESHOLD_SPAWN = 8f;
    public float speedFactor = 1.0f;
    public float FAST_SPAWN_ODDS = 0.1f;
    public float FAST_SPAWN_SPEED = 10f;
    public float MULTI_SPAWN_ODD = 0.2f;

    private GameStatsController statsController;
    private gulelController gulelcontroller;
    private shootController shootcontroller;
    private GameendScript endscript;
    public Image warning;

    public bool gameOver = false;

    int totalScore = 0;
    string playerName = "";
    //dreamloLeaderBoard dl;
    enum gameState
    {
        waiting,
        enterscore,
        leaderboard
    };

    gameState gs;


    public void spawngoon(){
        float spawnsodd = Random.Range(0.0f, 1.0f);
        int spawns = spawnsodd < MULTI_SPAWN_ODD ? 2 : 1;
        for (int i = 0; i < spawns; i++)
        {
            float fastspawn = Random.Range(0.0f, 1.0f);
            if (fastspawn < FAST_SPAWN_ODDS)
            {
                warning.enabled = true;
                var goonTransform = Instantiate(goonsprefab[3]) as Transform;
                goonTransform.gameObject.GetComponent<gooncontroler>().type = 3;
                SoundEffectsHelper.Instance.MakeSambaSound();
                StartCoroutine(handlerandomspawn(goonTransform, 1f));
            }
            else
            {
                int index = Random.Range(0, goonsprefab.Length-1);
                var goonTransform = Instantiate(goonsprefab[index]) as Transform;
                goonTransform.gameObject.GetComponent<gooncontroler>().type = index;
                goonTransform.gameObject.GetComponent<gooncontroler>().speedFactor = (1+ Random.Range(-0.1f,0.1f)) * speedFactor;
                goonTransform.gameObject.GetComponent<gooncontroler>().handeInitialization();
            }
        }
    }

    IEnumerator handlerandomspawn(Transform goonTransform, float delay){
        yield return new WaitForSeconds(delay);
        warning.enabled = false;
        goonTransform.gameObject.GetComponent<gooncontroler>().speedFactor = FAST_SPAWN_SPEED;
        goonTransform.gameObject.GetComponent<gooncontroler>().handeInitialization();
    }

	// Use this for initialization
	void Start () {
        statsController = GameObject.Find("Scripts").GetComponent<GameStatsController>();
        gulelcontroller = GameObject.Find("Scripts").GetComponent<gulelController>();
        shootcontroller = GameObject.Find("Scripts").GetComponent<shootController>();
        endscript = GameObject.Find("Scripts").GetComponent<GameendScript>();
        startUi();
        // get the reference here...
        //this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        this.gs = gameState.waiting;
        //dogameoveraction();
	}

    void startUi(){
        GameObject.Find("Shoot").SetActive(true);
        GameObject.Find("gulelBtn").SetActive(true);
        GameObject.Find("bombBtn").SetActive(true);
        warning.enabled = false;
    }

    public void endUI()
    {
        GameObject.Find("Shoot").SetActive(false);
        GameObject.Find("gulelBtn").SetActive(false);
        GameObject.Find("bombBtn").SetActive(false);
    }

    public void dogameoveraction(){
        gameOver = true;
        SoundEffectsHelper.Instance.StopBomerangSound();
        SoundEffectsHelper.Instance.StopHorseSound();
        this.totalScore = statsController.score;
        endUI();
        endscript.dogameoveraction();
        //this.gs = gameState.enterscore;
    }
	
	// Update is called once per frame
	void Update () {
        if (!gameOver)
        {
            if (spawnCoolDownTime > 0)
                spawnCoolDownTime -= Time.deltaTime;
            else
            {
                spawnCoolDownTime = COOLDOWN_THRESHOLD_SPAWN;
                spawngoon();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                gulelcontroller.fireGulel();
            }
            if (Input.GetKey(KeyCode.LeftCommand))
            {
                shootcontroller.fireBullet();
            }
        }
	}

    //void OnGUI()
    //{
    //    if (this.gs != gameState.waiting)
    //    {
    //        GUILayoutOption[] width200 = new GUILayoutOption[] { GUILayout.Width(400) };

    //        float width = 600;  // Make this wider to add more columns
    //        float height = 400;

    //        Rect r = new Rect(-400, -400, width, height);
    //        GUILayout.BeginArea(r, new GUIStyle("box"));

    //        GUILayout.BeginVertical();

    //        if (this.gs == gameState.enterscore)
    //        {
    //            GUILayout.Label("Total Score: " + this.totalScore.ToString());
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label("Your Name: ");
    //            this.playerName = GUILayout.TextField(this.playerName, width200);

    //            if (GUILayout.Button("Save Score"))
    //            {
    //                // add the score...
    //                if (dl.publicCode == "") Debug.LogError("You forgot to set the publicCode variable");
    //                if (dl.privateCode == "") Debug.LogError("You forgot to set the privateCode variable");

    //                dl.AddScore(this.playerName, totalScore);

    //                this.gs = gameState.leaderboard;
    //            }
    //            GUILayout.EndHorizontal();
    //        }

    //        if (this.gs == gameState.leaderboard)
    //        {
    //            GUILayout.Label("High Scores:");
    //            List<dreamloLeaderBoard.Score> scoreList = dl.ToListHighToLow();

    //            if (scoreList == null)
    //            {
    //                GUILayout.Label("(loading...)");
    //            }
    //            else
    //            {
    //                int maxToDisplay = 20;
    //                int count = 0;
    //                foreach (dreamloLeaderBoard.Score currentScore in scoreList)
    //                {
    //                    count++;
    //                    GUILayout.BeginHorizontal();
    //                    GUILayout.Label(currentScore.playerName, width200);
    //                    GUILayout.Label(currentScore.score.ToString(), width200);
    //                    GUILayout.EndHorizontal();

    //                    if (count >= maxToDisplay) break;
    //                }
    //            }
    //        }
    //        GUILayout.EndArea();
    //        r.y = r.y + r.height + 20;
    //    }
    //}

}
