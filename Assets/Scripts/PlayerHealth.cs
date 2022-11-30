using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    int trapsLayer;
    public GameObject deathVFXPrefeb;

    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==trapsLayer)
        {
            Instantiate(deathVFXPrefeb, transform.position,transform.rotation);
            gameObject.SetActive(false);
            AudioManager.PlayerDeathAudio();

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.PlayerDied(); 
        }
    }
}
