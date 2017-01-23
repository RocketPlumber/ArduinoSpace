using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class GameController : MonoBehaviour {

    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float startWait;
    public float spawnWait;
    public float waveGap;
    public int gameNumber;

    // begin new code

    //public PlayerController playerController;
   
    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;

    private bool gameOver;
    private bool restart;
    private int score;
    IEnumerator spawnWaves() {
        yield return new WaitForSeconds(startWait);
        while (true) {
            for (int i = 0; i < hazardCount; i++) {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y, spawnValues.y), spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            hazardCount += 25;
            yield return new WaitForSeconds(waveGap);
            if (gameOver) {
                //restartText.text = "Press 'R' twice to feel the Force again";
                restart = true;
                break;
            }
        }
    }



    // Use this for initialization
    void Start () {
        StartCoroutine(spawnWaves());
        UpdateScore();
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        gameNumber++;
        Debug.Log("Game number: " + gameNumber) ;
        Debug.Log(gameNumber);
        //playerController.sp = new SerialPort("COM3", 9600);
        //playerController.sp.Open();
        //playerController.sp.ReadTimeout = 1;
    }
	
    public void addScore(int newScoreValue) {
        score += newScoreValue;
        UpdateScore();
    }

    public void GameOver() {
        if (score < 50) {
            gameOverText.text = "Weak! But don't give in yet... Press R twice to feel the Force again";
        }
        else if(score < 100){
            gameOverText.text = "Impressive...but you are not a Jedi yet! Press R twice to feel the Force again";
        }
        else {
            gameOverText.text = "Truly, you are the Chosen One! Press R twice to feel the Force again";
        }

        gameOver = true;
    }

    void UpdateScore() {
        scoreText.text = "Score: " + score;
    }

    void Update() {
        //if (restart) {
            if (Input.GetKeyDown(KeyCode.R)) {
                Application.LoadLevel(Application.loadedLevel);
            }
        //}
    }
}
