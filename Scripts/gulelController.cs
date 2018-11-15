using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gulelController : MonoBehaviour {
    public float gulelCoolDownTime = 1.0f;
    public float COOLDOWN_THRESHOLD_GULEL = 20f;
    public float gulelspeed = 50f;
    public float handanimationspeed = 0.15f;


    public Button gulelBtn;
    public Image gulel;
    public Image progressbar;
    public Image hand;
    public Sprite[] rotation;
    public Sprite[] gulelhandimages;
    public Sprite[] btnstates;
    public int gulelcount = 0;
    public Text gulelcounttext;
    private int index = 0;
	// Use this for initialization
	void Start () {
        gulelBtn.onClick.AddListener(fireGulel);
        gulel.enabled = false;
        hand.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (gulelCoolDownTime > 0)
            gulelCoolDownTime -= Time.deltaTime;
	}

    public void fireGulel()
    {
        if (CanGulel())
        {
            updategulelCount(-1);
            SoundEffectsHelper.Instance.MakeBomerangSound();
            gulelCoolDownTime = COOLDOWN_THRESHOLD_GULEL;
            gulel.enabled = true;
            gulel.gameObject.GetComponent<Transform>().position = GameObject.Find("Gun").GetComponent<Transform>().position + new Vector3 (300f,0f,0f);
            StartCoroutine(animateHand());
            StartCoroutine(RadialProgress(COOLDOWN_THRESHOLD_GULEL));
        }
    }

    public void updategulelCount(int incr)
    {
        if (gulelcount + incr >= 0 && gulelcount + incr <= 3)
            gulelcount = gulelcount + incr;
        if (gulelcount == 0)
            gulelBtn.gameObject.GetComponent<Image>().sprite = btnstates[0];
        else
            gulelBtn.gameObject.GetComponent<Image>().sprite = btnstates[1];
        gulelcounttext.text = "X " + gulelcount.ToString();
    }

    IEnumerator animateHand(){
        hand.enabled = true;
        for (int i = 0; i < gulelhandimages.Length; i++)
        {
            hand.sprite = gulelhandimages[i];
            yield return new WaitForSeconds(handanimationspeed);
        }
        hand.enabled = false;
        GameObject[] goons = GameObject.FindGameObjectsWithTag("Goons");
        StartCoroutine(handlegulelmovement(goons, 0.03f));
    }

    IEnumerator handlegulelmovement(GameObject[] goons, float delay)
    {
        foreach (GameObject goon in goons)
        {
            while (goon != null && goon.GetComponent<gooncontroler>() !=null && Vector3.Distance(goon.gameObject.GetComponent<Transform>().position-new Vector3(200f*goon.GetComponent<gooncontroler>().scalefactor,0f,0f), gulel.gameObject.GetComponent<RectTransform>().position) > 180f * goon.GetComponent<gooncontroler>().scalefactor)
            {
                movetowardsgoon(goon);
                yield return new WaitForSeconds(delay);

            }
            while (goon != null && goon.GetComponent<bomerController>() != null && goon.GetComponent<bomerController>().alive && Vector3.Distance(goon.gameObject.GetComponent<Transform>().position - new Vector3(50f, 0f, 0f), gulel.gameObject.GetComponent<RectTransform>().position) > 180f)
            {
                movetowardsgoon(goon);
                yield return new WaitForSeconds(delay);

            }
            if (goon)
            {
                if (goon.GetComponent<gooncontroler>())
                    goon.GetComponent<gooncontroler>().kill();
                else
                    goon.GetComponent<bomerController>().kill();
            }
        }
        gulel.enabled = false;
        SoundEffectsHelper.Instance.StopBomerangSound();
    }

    IEnumerator RadialProgress(float time)
    {
        float rate = 1 / time;
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            //progressbar.fillAmount = i;
            gulelBtn.gameObject.GetComponent<Image>().fillAmount = i;
            yield return 0;
        }
    }

    public void movetowardsgoon(GameObject goon)
    {
        gulel.gameObject.GetComponent<Image>().sprite = rotation[index];
        index++;
        if (index == rotation.Length) index = 0;
        Vector3 direction = goon.gameObject.GetComponent<Transform>().position - gulel.gameObject.GetComponent<Transform>().position;
        if(goon.GetComponent<gooncontroler>())
            direction.x -= 140f * goon.gameObject.GetComponent<gooncontroler>().scalefactor;
        else direction.x -= 50f;
        direction = direction.normalized;
        gulel.gameObject.GetComponent<Transform>().position += direction * gulelspeed;
        //gulel.gameObject.GetComponent<Transform>().localScale *= (1.7f-(float)(gulel.gameObject.GetComponent<Transform>().position.y) / (float)Screen.height);
    }

    public bool CanGulel()
    {
        //return gulelCoolDownTime < 0 ;
        return (gulelCoolDownTime < 0 && gulelcount>0);
    }
}
