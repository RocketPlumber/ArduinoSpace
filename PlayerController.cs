using UnityEngine;
using System.Collections;
using System.IO.Ports;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, yMin, yMax;
}


public class PlayerController : MonoBehaviour {

    private AudioSource audioSource;
    private Rigidbody rb;   // I gotta do this because Unity5 won't let me directly access
                            // the Rigidbody shorthand, so this is a workaround so I don't
                            // have to type GetComponent<> every time
    public float speed;    // input speed is a value from 0 to 1, so need to multiply it by some speed to make it decent 
    public Boundary boundary;
    public float tilt;

    public GameObject shot;
    public Transform ShotSpawn;

    public float fireRate;

    private float nextFire;

    public SerialPort sp = new SerialPort("COM7", 9600);

    void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        sp.Open();
        sp.ReadTimeout = 1;
        //Debug.Log("STARTED");
    }
    void FixedUpdate () {

        int moveHorizontal = 0;
        int moveVertical = 0;

        try {
            string value = sp.ReadLine();
            string[] movedirs = value.Split(',');
            //Debug.Log(movedirs[0]);
            if (movedirs[0] == "-1") {
                moveHorizontal = -1;
            }
            else if (movedirs[0] == "1") {
                moveHorizontal = 1;
            }

            if (movedirs[1] == "-1") {
                moveVertical = -1;
            }
            else if (movedirs[1] == "1") {
                moveVertical = 1;
            }
        }
        catch (System.Exception) { }

        //float moveHorizontal = Input.GetAxis("Horizontal");   // Eventually, will have to replace these two lines
        //float moveVertical = Input.GetAxis("Vertical");     // with the code for the Arduino read 
        Debug.Log(moveHorizontal);
        Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
        rb.velocity = movement * speed;
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
            0.0f
            );
        //rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    void Update() {
        if (Input.GetButton("Fire1") && Time.time > nextFire) {
            audioSource.Play();
            nextFire = Time.time + fireRate;
            Instantiate(shot, ShotSpawn.position, ShotSpawn.rotation);
        }
    }

}
