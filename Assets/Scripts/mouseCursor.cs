using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
    }

    public float timebtwSpawn = 0.1f;
    public GameObject trailEffect;

    // Update is called once per frame
    void Update() {
        
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        if(timebtwSpawn <= 0) {
            Instantiate(trailEffect,cursorPos,Quaternion.identity);
            timebtwSpawn = 0.1f;
        }
        else {
            timebtwSpawn -= Time.deltaTime;
        }
    }
}
