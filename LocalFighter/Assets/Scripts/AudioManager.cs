using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    //test
    public AudioSource _AttackSwing;
    public AudioSource _TakeDamageMain;
    public AudioSource _TakeDamageAccent;

    //DH: playing around using a common damage sound for now.
    //Moving forward will possibly use the common damage sound approach and layer a distinct sound unique to the fighter class over it
    //E.g. metallic clang for Thor's hammers
    //More likely will be cleaner to have attack sounds unique to each fighter class
    //Might still use common attack sounds like smash does but won't use a universal take damage sound...

    //arena
    public AudioSource _Goal;
    public AudioSource _Announcer;
    public AudioClip[] _AnnouncerClips;
    public AudioSource _Combo;
    public AudioClip[] _ComboClips;
    public AudioSource _ComboBeep;
    public AudioClip[] _ComboBeepClips;
    public AudioSource _WallHit;
    public AudioSource _Rumble;
    public AudioClip[] _RumbleClips;

    //ambience
    public AudioSource _Ambience;
    public AudioClip[] _AmbienceClips;

    //music
    public AudioClip[] clips;
    public AudioSource _TempMusic;
    int clipOrder = 0;
    private bool randomPlay = true;

    //mixer
    public AudioMixer _Mixer;
    private bool _MusicMuted;
    private bool _SFXMuted;
    private bool _AmbienceMuted;

    public static AudioManager _Main;

    void Awake()
    {
        if (_Main != null)
        {
            Destroy(gameObject);
            return;
        }
        _Main = this;
        DontDestroyOnLoad(gameObject);

        //Invoke("FadeInMusic", 0.5f);

        _MusicMuted = true;
        _SFXMuted = true;
        _AmbienceMuted = true;
    }


    //MIXER//

    public void ToggleMusic() {

        _MusicMuted = !_MusicMuted;

        if (_MusicMuted) { _Mixer.SetFloat("MusicFader", -80); }
        else { _Mixer.SetFloat("MusicFader", 0); }

    }

    public void ToggleSFX()
    {

        _SFXMuted = !_SFXMuted;

        if (_SFXMuted) { _Mixer.SetFloat("SFXFader", -80); }
        else { _Mixer.SetFloat("SFXFader", 0); }

    }

    public void ToggleAmbience()
    {

        _AmbienceMuted = !_AmbienceMuted;

        if (_AmbienceMuted) { _Mixer.SetFloat("AmbienceFader", -80); }
        else { _Mixer.SetFloat("AmbienceFader", 0); }

    }

    public void FadeInMusic()
    {
        StartCoroutine(FadeMixerGroup.StartFade(_Mixer, "MusicFader", 3f, 1f));
    }

    public void FadeOutMusic()
    {
        StartCoroutine(FadeMixerGroup.StartFade(_Mixer, "MusicFader", 3f, 0f));
    }

    public void SetMasterVol(float sliderVal)
    {
        sliderVal = Mathf.Clamp(sliderVal, 0.0001f, 1);
        sliderVal = Mathf.Log10(sliderVal) * 20;
        _Mixer.SetFloat("MasterFader", sliderVal);
    }


    //STINGS//

    public void PlayAttackSwing() { _AttackSwing.Play(); }
    public void PlayGoal() { _Goal.Play(); }

    public void PlayTakeDamage()
    {
        float randomPitchAccent = Random.Range(0.5f, 1f);
        float randomPitchMain = Random.Range(0.9f, 1.1f);
        _TakeDamageAccent.pitch = randomPitchAccent;
        _TakeDamageMain.pitch = randomPitchMain;
        _TakeDamageAccent.Play();
        _TakeDamageMain.Play();
    }

    public void PlayCombo(int comboState)
    {
        if (comboState < 2)
        {
            return;
        }

        else if (comboState == 2)
        {
            _Combo.clip = _ComboClips[0];
            _ComboBeep.clip = _ComboBeepClips[0];
        }

        else if (comboState == 3)
        {
            _Combo.clip = _ComboClips[1];
            _ComboBeep.clip = _ComboBeepClips[1];
        }

        else if (comboState > 3)
        {
            _Combo.clip = _ComboClips[2];
            _ComboBeep.clip = _ComboBeepClips[2];
        }

        _Combo.Play();
        _ComboBeep.Play();
    }

    public void PlayAnnouncer(int announcerState)
    {
        _Announcer.clip = _AnnouncerClips[announcerState];
        _Announcer.Play();
    }

    public void PlayWallHit (float intensity)
    {
        _WallHit.volume = intensity;
        _WallHit.Play();
    }

    public void PlayRumble()
    {
        float random = Random.value;
        if (random > 0.5f) { _Rumble.clip = _RumbleClips[0]; }
        else { _Rumble.clip = _RumbleClips[1]; }
        _Rumble.Play();
    }

    //MUSIC//

    // Update is called once per frame
    void Update()
    {
        if (!_TempMusic.isPlaying)
        {
            // if random play is selected
            if (randomPlay == true)
            {
                _TempMusic.clip = GetRandomClip();
                _TempMusic.Play();
                // if random play is not selected
            }
            else
            {
                _TempMusic.clip = GetNextClip();
                _TempMusic.Play();
            }
        }
    }

    // function to get a random clip
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    // function to get the next clip in order, then repeat from the beginning of the list.
    private AudioClip GetNextClip()
    {
        if (clipOrder >= clips.Length - 1)
        {
            clipOrder = 0;
        }
        else
        {
            clipOrder += 1;
        }
        return clips[clipOrder];
    }
}
