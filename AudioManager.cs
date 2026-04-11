using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioSource menuAudioSource;
    [SerializeField] private AudioSource winAudioSource; // 🔥 Thêm Win AudioSource

    [Header("SFX Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reLoadClip;
    [SerializeField] private AudioClip energyClip;
    [SerializeField] private AudioClip winClip; // 🔥 Thêm Win Clip

    // ===================== SFX =========================
    public void PlayShootSound()
    {
        if (effectAudioSource != null && shootClip != null)
            effectAudioSource.PlayOneShot(shootClip);
    }

    public void PlayReLoadSound()
    {
        if (effectAudioSource != null && reLoadClip != null)
            effectAudioSource.PlayOneShot(reLoadClip);
    }

    public void PlayEnergySound()
    {
        if (effectAudioSource != null && energyClip != null)
            effectAudioSource.PlayOneShot(energyClip);
    }

    // ===================== MUSIC =========================
    public void PlayDefaultAudio()
    {
        StopAllMusic();
        if (defaultAudioSource != null)
            defaultAudioSource.Play();
    }

    public void PlayBossAudio()
    {
        StopAllMusic();
        if (bossAudioSource != null)
            bossAudioSource.Play();
    }

    public void PlayMenuAudio()
    {
        StopAllMusic();
        if (menuAudioSource != null)
            menuAudioSource.Play();
    }

    public void PlayWinAudio()
    {
        StopAllMusic();
        if (winAudioSource != null && winClip != null)
            winAudioSource.PlayOneShot(winClip);
    }

    // Dừng toàn bộ nhạc nền (không dừng SFX)
    public void StopAllMusic()
    {
        if (defaultAudioSource != null) defaultAudioSource.Stop();
        if (bossAudioSource != null) bossAudioSource.Stop();
        if (menuAudioSource != null) menuAudioSource.Stop();
        if (winAudioSource != null) winAudioSource.Stop();
    }

    // Dừng tất cả audio (cả nhạc + SFX)
    public void StopAllAudio()
    {
        StopAllMusic();
        if (effectAudioSource != null) effectAudioSource.Stop();
    }
}
