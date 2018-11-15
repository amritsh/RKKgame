using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shootController : MonoBehaviour {

    public float shootCoolDownTime = 1.0f;
    public float COOLDOWN_THRESHOLD_SHOOT = 0.2f;
    public Button shoot;
    private MoveGun movegun;
    public Image impact;
    public Sprite[] impacts;
    public float impactanimationspeed = 0.08f;

    private GameObject Pointer;
	// Use this for initialization
	void Start () {
        Pointer = GameObject.Find("crossHair");
        shoot.onClick.AddListener(fireBullet);
        movegun = GameObject.Find("Gun").GetComponent<MoveGun>();
        impact.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (shootCoolDownTime > 0)
            shootCoolDownTime -= Time.deltaTime;
	}

    public void fireBullet()
    {
        if (CanAttack())
        {
            SoundEffectsHelper.Instance.MakeBulletSound();
            movegun.animategun();
            shootCoolDownTime = COOLDOWN_THRESHOLD_SHOOT;
            GameObject deadgoon = findShot();
            if (deadgoon)
            {
                impact.gameObject.GetComponent<Transform>().position = Pointer.GetComponent<Transform>().position;
                impact.enabled = true;
                StartCoroutine(animateimpact());
                if (deadgoon.GetComponent<gooncontroler>())
                    deadgoon.GetComponent<gooncontroler>().kill();
                else
                    deadgoon.GetComponent<bomerController>().kill();
            }
        }
    }

    IEnumerator animateimpact()
    {
        for (int i = 0; i < impacts.Length; i++)
        {
            impact.sprite = impacts[i];
            yield return new WaitForSeconds(impactanimationspeed);
        }
        impact.enabled = false;
    }

    public GameObject findShot()
    {
        Vector3 Pointerposition = Pointer.GetComponent<Transform>().position;
        foreach (GameObject goon in GameObject.FindGameObjectsWithTag("Goons"))
        {
            if (goon.GetComponent<gooncontroler>())
            {
                if (Vector3.Distance(goon.transform.position - new Vector3(130f * goon.GetComponent<gooncontroler>().scalefactor, 0f, 0f), Pointerposition) < 170f * goon.GetComponent<gooncontroler>().scalefactor)
                    return goon;
            }
            else if (Vector3.Distance(goon.transform.position - new Vector3(50f, 0f, 0f), Pointerposition) < 130f)
                return goon;

        }
        return null;
    }

    public bool CanAttack()
    {
        return shootCoolDownTime < 0;
    }
}
