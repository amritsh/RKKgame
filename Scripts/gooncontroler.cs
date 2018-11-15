using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gooncontroler : MonoBehaviour {
    private bool alive = false;
    public float[] launchheight;
    public float velocity = 70f;
    public float speedFactor = 1.0f;
    public int type = 0;

    public int scoreptperkill = 2;
    public int goldperkill = 2;
    public float animationspeed = 0.1f;
    public float dyinganimationspeed = 0.1f;

    public Vector2[] positions;
    public Sprite[] images;
    public Sprite[] dyingimages;
    public Image scoregain;
    public int animationtime = 20;

    private GameStatsController statsController;
    private GameController gameController;
    private bombController bombcontroller;
    public float[] scalefactors;
    public float scalefactor;

    private float index = 0;

    public GameObject[] Playerlayers;
	// Use this for initialization
    void Start () {
        statsController = GameObject.Find("Scripts").GetComponent<GameStatsController>();
        gameController = GameObject.Find("Scripts").GetComponent<GameController>();
        bombcontroller = GameObject.Find("Scripts").GetComponent<bombController>();
    }
	
    public void handeInitialization(){
        Playerlayers[0] = GameObject.Find("ForegroundLayer");
        Playerlayers[1] = GameObject.Find("ForegroundLayer");
        Playerlayers[2] = GameObject.Find("PlayerLayer2");
        Playerlayers[3] = GameObject.Find("PlayerLayer1");
        scoregain = GameObject.Find("coinsgrant").GetComponent<Image>();
        scoregain.enabled = false;
        alive = true;
        SoundEffectsHelper.Instance.MakeHorseSound();
        int indexer = Random.Range(0, 4);
        transform.position = new Vector3(Screen.width, positions[indexer].x, positions[indexer].y);
        transform.parent = Playerlayers[indexer].transform;
        scalefactor = scalefactors[indexer];
        speedFactor *= scalefactor;
        transform.localScale = new Vector3(scalefactor, scalefactor);
        StartCoroutine(animaterun());
    }
	// Update is called once per frame
	void Update () {
        
	}

    public IEnumerator playDyingAnimation(){
        for (int i = 0; i < dyingimages.Length; i++)
        {
            gameObject.GetComponent<Image>().sprite = dyingimages[i];
            yield return new WaitForSeconds(animationspeed);
        }
    }

    public IEnumerator playCoinanimation()
    {
        for (int i = 0; i < animationtime; i++)
        {
            var tempcolor = scoregain.color;
            tempcolor.a = 1 - (float)i / animationtime;
            scoregain.color = tempcolor;
            scoregain.gameObject.GetComponent<Transform>().position -= new Vector3(0f, -10f, 0f);
            yield return new WaitForSeconds(animationspeed);
        }
        scoregain.enabled = false;
    }

    void makedyingsound(){
        SoundEffectsHelper.Instance.MakeEnemyShotSound();
    }
    private void handleScreenExit(){
        statsController.updateGoonsReached(1);
        alive = false;
        Destroy(gameObject);
    }

    public IEnumerator animaterun()
    {
        while (alive)
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (!alive) break;
                gameObject.GetComponent<Image>().sprite = images[i];
                yield return new WaitForSeconds(animationspeed);
                if (i == images.Length) i = 0;
            }
        }
    }

    public void kill(){
        alive = false;
        statsController.updateScore(scoreptperkill);
        if (type == 2)
            statsController.updateGold(goldperkill);
        if (type == 3)
            bombcontroller.updatebombCount(1);
        SoundEffectsHelper.Instance.StopHorseSound();
        Invoke("makedyingsound", 0.5f);
        StartCoroutine(playDyingAnimation());
        scoregain.enabled = true;
        StartCoroutine(playCoinanimation());
        scoregain.gameObject.GetComponent<Transform>().position = transform.position;
        transform.localScale *= 2.5f;
        transform.position += new Vector3(-400, -75, 0) * scalefactor;
        Destroy(gameObject,2.0f);
    }

	private void FixedUpdate()
	{
        if (alive && !gameController.gameOver)
        {
            this.gameObject.transform.position -= new Vector3(velocity * speedFactor * Time.deltaTime, 0);
        }
        if (this.gameObject.transform.position.x < -100f)
            handleScreenExit();            
	}
}
