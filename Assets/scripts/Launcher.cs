using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public Vector3 angle;
    [SerializeField]
    GameObject ballObject;
    Ball ball;
    Ball ballWait;
    public bool inWaitForBall = false;
    Map map;
    private int numberOfShot = 0;
    private float autofire = 3;
    public bool idle = true;
    public bool defeat = false;
    public bool victory = false;
    public bool pause = false;
    private bool inWaitForDrop = false;
    private GameObject aim;

    // initialization
    private void Start()
    {
        aim = GameObject.FindGameObjectWithTag("aim");
        map = transform.parent.GetComponent<Map>();
        ball = null;
        ballWait = null;
        angle = new Vector3(0f, 0f, 1f);
    }

    // reinitialization
    public void reset()
    {
        idle = false;
        if (ball != null)
            ball.destruct();
        if (ballWait != null)
            ballWait.destruct();
        GameObject tmp = Instantiate(ballObject, transform.localPosition, Quaternion.identity) as GameObject;
        ball = tmp.GetComponent<Ball>();
        ball.setColor(map.getColor());
        ball.transform.SetParent(map.transform);
        ball.transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
        GameObject tmp2 = Instantiate(ballObject, transform.localPosition, Quaternion.identity) as GameObject;
        ballWait = tmp2.GetComponent<Ball>();
        ballWait.setColor(map.getColor());
        ballWait.transform.SetParent(map.transform);
        ballWait.transform.localPosition = new Vector3(4.5f, 1, -7);
        angle = new Vector3(0f, 0f, 1f);
        inWaitForBall = false;
        numberOfShot = 0;
        autofire = 3;
    }

    // Update is called once per frame
    private void Update()
    {
        if (idle == false)
        {
            if (Input.GetButtonDown("Pause"))
            {
                pause = true;
                idle = true;
            }
            // angle of fire
            autofire -= Time.deltaTime;
            if (Input.GetButton("Left") && angle.x > -0.6)
            {
                angle.x -= 0.02f;
                angle.z = (angle.x < 0) ? (1 + angle.x) : (1 - angle.x);
                aim.transform.localEulerAngles = new Vector3(90, 0.7f * Mathf.Rad2Deg * Mathf.Atan(angle.x / angle.z), 0);
            }
            if (Input.GetButton("Right") && angle.x < 0.6)
            {
                angle.x += 0.02f;
                angle.z = (angle.x < 0) ? (1 + angle.x) : (1 - angle.x);
                aim.transform.localEulerAngles = new Vector3(90, 0.7f * Mathf.Rad2Deg * Mathf.Atan(angle.x / angle.z), 0);
            }

            if (!inWaitForDrop && !inWaitForBall && (Input.GetButtonDown("Fire") || autofire < 0)) // waiting for a fire
            {
                inWaitForBall = true;
                ball.Fire(angle);
                numberOfShot++;
            }
            else if (inWaitForBall && ball.isFixed) // updating the map after a fire
            {
                inWaitForBall = false;
                int ret = map.addBall((int)(ball.transform.localPosition.x + 3.5f), 7 - Mathf.RoundToInt(ball.transform.localPosition.z), ball.getColor(), ball);
                if (ret == -1) // if game is not win
                {
                    numberOfShot = map.goDown(numberOfShot);
                    if (numberOfShot != -1) // shot was accepted and game continue
                        inWaitForDrop = true;
                    else // shot was not accepted, game is lost
                    {
                        idle = true;
                        defeat = true;
                    }
                }
                else if (ret == 0) // if game is lost
                {
                    defeat = true;
                    idle = true;
                }
                else // if game is win
                {
                    victory = true;
                    idle = true;
                }
                autofire = 3;
            }
            else if (inWaitForDrop && map.dropping == 0)
            {
                inWaitForDrop = false;
                ballWait.transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
                ball = ballWait;
                GameObject tmp = Instantiate(ballObject, transform.localPosition, Quaternion.identity) as GameObject;
                ballWait = tmp.GetComponent<Ball>();
                ballWait.setColor(map.getColor());
                ballWait.transform.SetParent(map.transform);
                ballWait.transform.localPosition = new Vector3(4.5f, 1, -7);
            }
        }
        else
        {
            if (pause && Input.GetButtonDown("Pause"))
            {
                pause = false;
                idle = false;
            }
        }
    }
}