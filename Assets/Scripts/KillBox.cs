using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
    public bool dontKillEnemy;

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.tag;
        
        if (objTag == "Player")
            collision.gameObject.GetComponent<Player>().KillPlayer();
        else if (objTag == "Enemy" && !dontKillEnemy)
            Destroy(collision.gameObject);

        yield return new WaitForSeconds(1);
        if (objTag == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}