using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource bgmSource;

    void Start()
    {
        if (!bgmSource.isPlaying)
            bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public void PlayBGM()
    {
        if (!bgmSource.isPlaying)
            bgmSource.Play();
    }
}
