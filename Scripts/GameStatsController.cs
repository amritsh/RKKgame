using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStatsController : MonoBehaviour {
    public float FrequencyIncreaseFactor = 1.3f;
    public float MaxSpeed = 8.0f;
    public float FastestSpawn = 1.1f;
    public float SpeedIncreaseFactor = 2.32f;
    public float RandomnessIncreaseFactor = 8f;


    public Text ScoreText;
    public Text GoldText;
    public GameObject[] stars;

    private int goonsReached = 0;
    public int score = 0;
    private int gold = 0;
    private GameController gameController;
    private bomerController bomerController;
    public int highscore = 0;

    public void updateScore(int scr){
        score += scr;
        if (score % 10 == 0)
            bomerController.handeInitialization();
        ScoreText.text = score.ToString();
        if (gameController.speedFactor < MaxSpeed)
            gameController.speedFactor += SpeedIncreaseFactor*(Mathf.Log(score)-Mathf.Log(score-1));
        if (gameController.COOLDOWN_THRESHOLD_SPAWN > FastestSpawn)
            gameController.COOLDOWN_THRESHOLD_SPAWN -= FrequencyIncreaseFactor*(Mathf.Log(score) - Mathf.Log(score - 1));
    }
    public void updateGold(int gld)
    {
        gold += gld;
        GoldText.text = gold.ToString();
    }
    public void updateGoonsReached(int gr)
    {
        Destroy(stars[goonsReached]);
        goonsReached += gr;
        SoundEffectsHelper.Instance.MaketauntSound(goonsReached);
        if(goonsReached>=5){
            updateHighScore();
            gameController.dogameoveraction();
        }
    }

    public void updateHighScore(){
        if(PlayerPrefs.HasKey("highscore"))
            highscore = PlayerPrefs.GetInt("highscore", highscore);
        if(score > highscore){
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
            
    }
    public int getScore(){
        Debug.Log("wtf:" + score);
        return score;
    }
    public int getGold(){
        return gold;
    }
    public int getHighScore(){
        Debug.Log("wtf:" + highscore);
        return highscore;
    }
    public int getGoonsReached(){        
        return goonsReached;
    }

    // Use this for initialization
    void Start()
    {
        bomerController = GameObject.Find("bomerman").GetComponent<bomerController>();
        gameController = GameObject.Find("Scripts").GetComponent<GameController>();
    }

	private void Update()
	{
		
	}
}
