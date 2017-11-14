using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUI : MonoBehaviour {
    Launcher launcher;

	// Use this for initialization
	void Start () {
        launcher = GameObject.FindGameObjectWithTag("launcher").GetComponent<Launcher>();
    }

    // Update is called once per frame
    void Update () {
        if (launcher.pause)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);

        if (launcher.victory)
            transform.GetChild(1).gameObject.SetActive(true);
        else
            transform.GetChild(1).gameObject.SetActive(false);

        if (launcher.defeat)
            transform.GetChild(2).gameObject.SetActive(true);
        else
            transform.GetChild(2).gameObject.SetActive(false);
    }

    public void Menu()
    {
        launcher.pause = false;
        launcher.victory = false;
        launcher.defeat = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play(int lvl)
    {
        transform.GetChild(3).gameObject.SetActive(false);
        launcher.GetComponentInParent<Map>().reset(lvl);
        launcher.reset();
    }
}
