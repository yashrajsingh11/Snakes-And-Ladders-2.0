using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        
    }

    public GameObject promptMenu;
    StateManager theStateManager;
    // Update is called once per frame
    void Update()
    {
        if(theStateManager.hasUsedSpecialItem == false)
        {
            
            promptMenu.SetActive(false);   
            Time.timeScale = 1f;
        }
        else
        {
            promptMenu.SetActive(true);
            //Invoke("ok", 2);
        }
    }



    public void ok()
    {
        theStateManager.hasUsedSpecialItem = false;        
        //promptMenu.SetActive(false);
        //Debug.Log(theStateManager.hasUsedSpecialItem);
        Time.timeScale = 1f;
    }

}
