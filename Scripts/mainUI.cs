using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(loadmainscreen());
	}

    IEnumerator loadmainscreen(){
        yield return new WaitForSeconds(5.0f);
        Application.LoadLevel("VJS");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel("VJS");
        }
	}
}
