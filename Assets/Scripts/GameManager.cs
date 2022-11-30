using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	static GameManager instance;
	SceneFader fader;
	List<Orb> orbs;
	public int orbNum;
	public int deathNum;

	private void Awake()
	{
		if(instance !=null ) {
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(this);

		orbs = new List<Orb>();
	}

	public static void PlayerDied()
    {
		instance.Invoke("RestartScene", 1.5f);
		instance.deathNum += 1;
    }

	public static void RegisterOrb(Orb orb)
    {
		if(!instance.orbs.Contains(orb))
        {
			instance.orbs.Add(orb);
        }
    }

	void RestartScene()
    {
		instance.fader.FadeOut();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		instance.orbs.Clear();
	}

	public static void RegisterSceneFader(SceneFader obj)
    {
		instance.fader = obj;

    }

    private void Update()
    {
		orbNum = orbs.Count;
    }


	public static void PlayerGrabbedOrb(Orb orb)
    {
		if (!instance.orbs.Contains(orb))
        {
			return;
        }
		instance.orbs.Remove(orb);
    }
}
