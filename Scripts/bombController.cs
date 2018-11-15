using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bombController : MonoBehaviour {
    public float bombCoolDownTime = 1.0f;
    public float COOLDOWN_THRESHOLD_BOMB = 20f;


    public Button bombBtn;
    public Image explosion;
    public Image explosionHand;

    private float handanimationspeed = 0.05f;
    private float bombanimationspeed = 0.05f;

    public Sprite[] explosionhandimages;
    public Sprite[] explosionimages;
    public Sprite[] btnstates;
    public int bombcount = 0;
    public Text bombcounttext;

	// Use this for initialization
	void Start () {
        bombBtn.onClick.AddListener(fireGBomb);
        explosion.enabled = false;
        explosionHand.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (bombCoolDownTime > 0)
            bombCoolDownTime -= Time.deltaTime;
	}

    public void fireGBomb()
    {
        if (CanBomb())
        {
            updatebombCount(-1);
            bombCoolDownTime = COOLDOWN_THRESHOLD_BOMB;
            explosionHand.enabled = true;
            StartCoroutine(animatebombhand());
            //StartCoroutine(animatebomb());
            StartCoroutine(RadialProgress(COOLDOWN_THRESHOLD_BOMB));
        }
    }

    public void updatebombCount(int incr){
        if (bombcount + incr >= 0 && bombcount + incr <= 3)
            bombcount = bombcount + incr;
        if (bombcount == 0)
            bombBtn.gameObject.GetComponent<Image>().sprite = btnstates[0];
        else
            bombBtn.gameObject.GetComponent<Image>().sprite = btnstates[1];
        bombcounttext.text = "X " + bombcount.ToString();
    }

    IEnumerator RadialProgress(float time)
    {
        float rate = 1 / time;
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            //progressbar.fillAmount = i;
            bombBtn.gameObject.GetComponent<Image>().fillAmount = i;
            yield return 0;
        }
    }

    IEnumerator animatebombhand()
    {
        for (int i = 0; i < explosionhandimages.Length; i++)
        {
            explosionHand.sprite = explosionhandimages[i];
            yield return new WaitForSeconds(handanimationspeed);
        }
        explosionHand.enabled = false;
        explosion.enabled = true;
        StartCoroutine(animatebomb());
        SoundEffectsHelper.Instance.MakeExplosionSound();
        GameObject[] goons = GameObject.FindGameObjectsWithTag("Goons");
        foreach (GameObject goon in goons)
        {
            if(goon.GetComponent<gooncontroler>())
                goon.GetComponent<gooncontroler>().kill();
            else
                goon.GetComponent<bomerController>().kill();
        }
    }

    IEnumerator animatebomb()
    {
        for (int i = 0; i < explosionimages.Length; i++)
        {
            explosion.sprite = explosionimages[i];
            yield return new WaitForSeconds(bombanimationspeed);
        }
        explosion.enabled = false;
    }

    public bool CanBomb()
    {
        return (bombCoolDownTime < 0 && bombcount>0);
    }
}
