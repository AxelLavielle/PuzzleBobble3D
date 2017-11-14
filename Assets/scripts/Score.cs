using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {
    TextMesh text;
    Map map;
	// Use this for initialization
	void Start () {
        text = transform.GetComponent<TextMesh>();
        map = transform.parent.parent.GetComponent<Map>();
    }
	
	// Update is called once per frame
	void Update () {
        text.text = map.score.ToString();
	}
}
