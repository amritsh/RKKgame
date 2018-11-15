using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveGun : MonoBehaviour {

    public Transform pointer;
    public Vector3 direction;

    public Sprite[] shooting;

    private float animationspeed = 0.1f;
	
	void Update () {
        direction = pointer.position-transform.position;
        float angle = Vector3.SignedAngle(direction, transform.position,Vector3.up);
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
	}	
	
    void Start(){
        pointer = GameObject.Find("crossHair").GetComponent<Transform>();
	}

    public void animategun(){
        StartCoroutine(animationhandler());
    }

    IEnumerator animationhandler()
    {
        for (int i = 0; i < shooting.Length; i++)
        {
            gameObject.GetComponent<Image>().sprite = shooting[i];
            yield return new WaitForSeconds(animationspeed);
        }
        gameObject.GetComponent<Image>().sprite = shooting[0];
    }
}