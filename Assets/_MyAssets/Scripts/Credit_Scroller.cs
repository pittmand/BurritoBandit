using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit_Scroller : MonoBehaviour {

	public GameObject Camera;
	public int speed = 1;
	public string level;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Camera>().transform.Translate (Vector3.down * Time.deltaTime * speed);

        StartCoroutine(waitFor());
	}


    IEnumerator waitFor()
    {
        yield return new WaitForSeconds(20);
        //Application.LoadLevel(level);
        SceneManager.LoadScene("BurritoBandit");
        
    }
}

