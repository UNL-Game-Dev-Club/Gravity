using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

	public bool exit;

	public string nextLevel;
    public string previousLevel;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start () {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        sr.transform.Rotate(0, 0, -100 * Time.deltaTime);

        if (exit) {
        	return;
        }
#if UNITY_WEBGL
        if (Input.GetKeyUp("2"))
            LoadScene(nextLevel);
        else if (Input.GetKeyUp("1"))
            LoadScene(previousLevel);
#else
        if (Input.GetKey("2"))
            LoadScene(nextLevel);
        else if (Input.GetKey("1"))
            LoadScene(previousLevel);
#endif
    }

    void OnTriggerEnter2D (Collider2D other) {
    	if (other.tag != "Player") {
    		return;
    	}

        if (other.gameObject.GetComponent<Player>().dead)
        {
            return;
        }

        LoadScene(nextLevel);
    }

    void LoadScene(string level)
    {
        if (nextLevel == "Quit")
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            return;
        }
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}