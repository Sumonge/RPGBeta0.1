using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinmumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField]
    private AudioSource[] bgm;

    public bool playBGM;
    private int bgmIndex;
    public bool canPlaySFX = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        canPlaySFX = false;
        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if (!playBGM)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    public void PlaySFX(int sfxIndex, Transform source)
    {
        if (canPlaySFX == false)
            return;

        if (source != null && Vector2.Distance(PlayerManager.instance.transform.position, source.position) > sfxMinmumDistance)
            return;

        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].pitch = Random.Range(0.8f, 1.2f);
            sfx[sfxIndex].Play();
        }
    }

    public void StopSFX(int index)
    {
        sfx[index].Stop();
    }

    public void StopSFXWithFade(int index)
    {
        StartCoroutine(DecreaseVolume(sfx[index]));
    }

    private IEnumerator DecreaseVolume(AudioSource audio)
    {
        float defaultVolume = audio.volume;

        while (audio.volume > 0.1f)
        {
            audio.volume -= audio.volume * Time.deltaTime;
            yield return new WaitForSeconds(0.25f);

            if (audio.volume <= 0.1f)
            {
                audio.Stop();
                audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int bgmIndex)
    {
        this.bgmIndex = bgmIndex;
        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}