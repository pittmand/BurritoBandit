using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credit_Scroller : MonoBehaviour {

    public Text PressButtonText;

    void Start()
    {
        PressButtonText.enabled = false;
    }

    void Update()
    {
        if (transform.position.y >= 1200.0f)
        {
            PressButtonText.enabled = true;

            //if (Input.GetKey(KeyCode.Space)) //Change KeyCode Here
            {
                //Input Command Here
            }
        }
    }
}

