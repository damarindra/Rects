using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;
    private AudioSource audioSource;

    public AudioClip srinkRectSound;
    public AudioClip buttonSound;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
	}

    public void PlaySrink()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audioSource.clip = srinkRectSound;
            audioSource.Play();
        }
    }

    public void ButtonSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audioSource.clip = buttonSound;
            audioSource.Play();
        }
    }
}
