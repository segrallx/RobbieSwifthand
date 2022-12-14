using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{

    Animator anim;
    int faderID;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        faderID = Animator.StringToHash("Fade");

        GameManager.RegisterSceneFader(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        anim.SetTrigger(faderID);
    }
}
