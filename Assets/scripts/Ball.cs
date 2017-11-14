using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private int color;
    public bool isFixed = false;
    public float mSpeed;
    private Rigidbody mRigidBody;
    private bool dropping = false;
    private GameObject upWall;
    [SerializeField]
    private GameObject particles;

    // Initialisation
    private void Awake()
    {
        upWall = GameObject.FindGameObjectWithTag("upWall");
        mRigidBody = GetComponent<Rigidbody>();
        mRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        isFixed = true;
    }

    // Destruction of the ball when it goes off the map
    private void Update()
    {
        if (dropping && transform.localPosition.z < -11)
        {
            GetComponentInParent<Map>().dropping--;
            Destroy(gameObject);
        }
    }

    // Fire the ball in the direction given
    public void Fire(Vector3 angle)
    {
        mRigidBody.constraints = RigidbodyConstraints.None;
        mRigidBody.AddForce(GameObject.FindGameObjectWithTag("aim").transform.up * (mSpeed + Mathf.Abs(angle.x * mSpeed / 3)));
        isFixed = false;
    }

    // Returns the color of the ball
    public int getColor()
    {
        return (color);
    }

    // Sets the color of the ball
    public void setColor(int colorInput)
    {
        Color[] colorList = { Color.red, Color.blue, Color.yellow, Color.green, Color.cyan };
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = colorList[colorInput];
        color = colorInput;
    }

    // Destroy the ball without animation
    public void destruct()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Drop the ball to the floor
    public void drop()
    {
        dropping = true;
        mRigidBody.constraints = RigidbodyConstraints.None;
    }

    // Collision handler
    private void OnCollisionEnter(Collision collision)
    {
        if (!isFixed) // check if already fixed, in that case we don't need collision anymore
        {
            if (collision.collider.CompareTag("upWall")) // check if it goes into the upper wall, to stick into it
            {
                isFixed = true;
                mRigidBody.velocity = Vector3.zero;
                mRigidBody.angularVelocity = Vector3.zero;
                mRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                int line = 1 - (int)(upWall.transform.localPosition.z - Mathf.RoundToInt(transform.localPosition.z)) % 2;
                if (Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) < 0)
                    transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) - 2.5f - line * 0.5f, 0.5f, Mathf.RoundToInt(transform.localPosition.z));
                else if (Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) > 7)
                    transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) - 4.5f - line * 0.5f, 0.5f, Mathf.RoundToInt(transform.localPosition.z));
                else
                    transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) - 3.5f - line * 0.5f, 0.5f, Mathf.RoundToInt(transform.localPosition.z));
            }
            else if (collision.collider.CompareTag("ball")) // if it collides with a ball, we have to stick into it
            {
                isFixed = true;
                mRigidBody.velocity = Vector3.zero;
                mRigidBody.angularVelocity = Vector3.zero;
                mRigidBody.constraints = RigidbodyConstraints.FreezeAll;
                int line = 1 - (int)(upWall.transform.localPosition.z - Mathf.RoundToInt(transform.localPosition.z)) % 2;
                if (Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) < 0)
                    transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) - 2.5f - line * 0.5f, 0.5f, Mathf.RoundToInt(transform.localPosition.z));
                else if (Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) > 7)
                    transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) - 4.5f - line * 0.5f, 0.5f, Mathf.RoundToInt(transform.localPosition.z));
                else
                    transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x + 3.5f + line * 0.5f) - 3.5f - line * 0.5f, 0.5f, Mathf.RoundToInt(transform.localPosition.z));
            }
            else
                mRigidBody.velocity = new Vector3(mRigidBody.velocity.x, Mathf.Max(mRigidBody.velocity.z, 4f), Mathf.Max(mRigidBody.velocity.z, 4f));
        }
        else if (collision.collider.CompareTag("ball"))
        {
            collision.collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.collider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}