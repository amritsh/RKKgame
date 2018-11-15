using UnityEngine;
using System.Collections;

public class MoveCrosshair : MonoBehaviour {

    public float moveSpeed = 15f;
    public VJHandler jsMovement;

    private Vector3 direction;
    private float xMin, xMax, yMin, yMax;

    void Update()
    {

        direction = jsMovement.InputDirection; //InputDirection can be used as per the need of your project

        if (direction.magnitude != 0)
        {
            transform.position += direction * moveSpeed;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), 0f);//to restric movement of player
            //gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(direction.x, direction.y)*10f;
        }
    }

    void Start()
    {
        //Initialization of boundaries
        xMax = Screen.width - 50; // I used 50 because the size of player is 100*100
        xMin = 50;
        yMax = Screen.height - 50;
        yMin = 50;
    }

}