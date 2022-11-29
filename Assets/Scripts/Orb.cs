using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int player;
    public GameObject explosionVFXPrefeb;


    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.NameToLayer("Player");    
    }

    // Update is called once per frame
    void Update()
    {        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == player)
        {
            gameObject.SetActive(false);
            Instantiate(explosionVFXPrefeb, transform.position, transform.rotation);
            AudioManager.PlayOrbAudio();
        }
    }
}
