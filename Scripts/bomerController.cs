using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bomerController : MonoBehaviour {
    public bool alive = false;
    public Vector3 spawnposition;
    public float velocity = 70f;

    public int scoreptperkill = 2;
    public float animationspeed = 0.03f;
    public float dyinganimationspeed = 0.05f;
    public float splashanimationspeed = 0.2f;
    public float spawnduration =3.0f;

    public Sprite[] images;
    public Sprite[] dyingimages;
    public Sprite[] splashimages;

    public Image bomermananimation;
    public Image bomerdyinganimation;
    public Image screensplash;

    private GameStatsController statsController;
    private GameController gameController;
    private gulelController gulelcontroller;

	// Use this for initialization
    void Start () {
        gameObject.GetComponent<Image>().enabled = false;
        bomermananimation.enabled = false;
        bomerdyinganimation.enabled = false;
        screensplash.enabled = false;
        statsController = GameObject.Find("Scripts").GetComponent<GameStatsController>();
        gameController = GameObject.Find("Scripts").GetComponent<GameController>();
        gulelcontroller = GameObject.Find("Scripts").GetComponent<gulelController>();
    }
	
    public void handeInitialization(){
        alive = true;
        gameObject.GetComponent<Image>().enabled = true;
        SoundEffectsHelper.Instance.MakeLaganishanaSound();
        StartCoroutine(animateaction());
    }
	// Update is called once per frame
	void Update () {
        
	}

    public IEnumerator playDyingAnimation(){
        for (int i = 0; i < dyingimages.Length; i++)
        {
            bomerdyinganimation.sprite = dyingimages[i];
            yield return new WaitForSeconds(dyinganimationspeed);
        }
        gulelcontroller.updategulelCount(1);
        bomerdyinganimation.enabled = false;
    }


    public IEnumerator animateaction()
    {
        yield return new WaitForSeconds(spawnduration);
        if(alive){
            bomermananimation.enabled = true;
            gameObject.GetComponent<Image>().enabled = false;
            statsController.updateGoonsReached(1);
            StartCoroutine(playthrowanimation());
        }
    }

    public IEnumerator playthrowanimation()
    {
        for (int i = 0; i < images.Length; i++)
        {
            bomermananimation.sprite = images[i];
            yield return new WaitForSeconds(animationspeed);
        }
        alive = false;
        bomermananimation.enabled = false;
        screensplash.enabled = true;
        Handheld.Vibrate();
        StartCoroutine(playScreenSplashanimation());
    }

    public IEnumerator playScreenSplashanimation()
    {
        for (int i = 0; i < splashimages.Length; i++)
        {
            screensplash.sprite = splashimages[i];
            yield return new WaitForSeconds(splashanimationspeed);
        }
        screensplash.enabled = false;
    }

    public void kill(){
        if (alive)
        {
            alive = false;
            gameObject.GetComponent<Image>().enabled = false;
            statsController.updateScore(scoreptperkill);
            bomerdyinganimation.enabled = true;
            SoundEffectsHelper.Instance.MakeEnemyShotSound();
            StartCoroutine(playDyingAnimation());
        }
    }

	private void FixedUpdate()
	{
                    
	}
}
