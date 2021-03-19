using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<Text>();
    }

    Text myText;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void promptText(string s)
    {
        myText.text = s;
    }

}
