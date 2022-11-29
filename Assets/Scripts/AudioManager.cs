using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class AudioManager : MonoBehaviour
{
	static AudioManager current;

	[Header("env audio")]
	public AudioClip AmbientClip;
	public AudioClip MusicClip;

	[Header("VFX")]
	public AudioClip deadFXClip;
	public AudioClip orbFXClip;

	[Header("robbie")]
	public AudioClip[] walkStepClips;
	public AudioClip[] crouchStepClips;
	public AudioClip jumpClip;
	public AudioClip jumpVoiceClip;
	public AudioClip deadthClip;
	public AudioClip deathVoiceClip;
	public AudioClip orbVoiceClip;

	AudioSource ambientSource;
	AudioSource musicSource;
	AudioSource fxSource;
	AudioSource playerSource;
	AudioSource voiceSource;

	private void Awake()
    {
		current = this;
		DontDestroyOnLoad(gameObject);

		ambientSource = gameObject.AddComponent<AudioSource>();
		musicSource = gameObject.AddComponent<AudioSource>();
		fxSource = gameObject.AddComponent<AudioSource>();
		playerSource = gameObject.AddComponent<AudioSource>();
		voiceSource = gameObject.AddComponent<AudioSource>();

		StartLevelAudio();
	}


	void StartLevelAudio()
	{
		current.ambientSource.clip = current.AmbientClip;
		current.ambientSource.loop = true;
		current.ambientSource.Play();

		current.musicSource.clip = current.MusicClip;
		current.musicSource.loop = true;
		current.musicSource.Play();
	}


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public static void PlayFootstepAudio()
    {
		int index = Random.RandomRange(0, current.walkStepClips.Length);
		current.playerSource.clip = current.walkStepClips[index];
		current.playerSource.Play();
    }

	public static void PlayCrouchFootstepAudio()
	{
		int index = Random.RandomRange(0, current.crouchStepClips.Length);
		current.playerSource.clip = current.crouchStepClips[index];
		current.playerSource.Play();

	}

	public static void PlayJumpAudio()
    {
		current.playerSource.clip = current.jumpClip;
		current.playerSource.Play();

		current.voiceSource.clip = current.jumpVoiceClip;
		current.voiceSource.Play();
    }

	public static void PlayerDeathAudio()
    {
		current.playerSource.clip = current.deadthClip;
		current.playerSource.Play();

		current.voiceSource.clip = current.deathVoiceClip;
		current.voiceSource.Play();

		current.fxSource.clip = current.deadFXClip;
		current.fxSource.Play();

	}

	public static void PlayOrbAudio()
    {
		current.fxSource.clip = current.orbFXClip;
		current.fxSource.Play();

		current.voiceSource.clip = current.orbVoiceClip;
		current.voiceSource.Play();
    }




}
