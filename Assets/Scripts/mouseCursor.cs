using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour
{

    //private SpriteRenderer rend;
    //public Sprite cursor;
    public float timebtwSpawn = 0.1f;
    public GameObject trailEffect;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        //rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        //rend.sprite = cursor;
        if(timebtwSpawn <= 0)
        {
            Instantiate(trailEffect,cursorPos,Quaternion.identity);
            timebtwSpawn = 0.1f;
        }
        else
        {
            timebtwSpawn -= Time.deltaTime;
        }
    }
}
