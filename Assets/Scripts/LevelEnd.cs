using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

	public string nextLevel;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start () {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        sr.transform.Rotate(0, 0, -100 * Time.deltaTime);
    }

    void OnTriggerEnter2D (Collider2D other) {
    	if (other.tag != "Player") {
    		return;
    	}

    	
    	
    	SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }
}