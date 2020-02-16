using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.tag;
        if (objTag == "Player")
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        else
            Destroy(collision.gameObject);
        yield return new WaitForSeconds(1);
        if (objTag == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}