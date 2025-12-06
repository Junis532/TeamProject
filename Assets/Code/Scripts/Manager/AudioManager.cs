using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("배경음악")]
    public AudioSource bgmSource;
   
    [Header("효과음")]
    public AudioSource sfxSource;

    [Header("효과음 리스트")]
    public AudioClip testSound;

    // 배경음 재생
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    // 배경음 정지
    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource != null && clip != null) sfxSource.PlayOneShot(clip, volume);
    }

    // 편의 함수
    public void PlayHitSound(float volume = 1f) => PlaySFX(testSound, volume);
}
