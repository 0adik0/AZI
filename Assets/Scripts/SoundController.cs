// UI Pack : Toony PRO
// Version: 1.0.0
// Author: Gold Experience Team (http://ge-team.com/pages/unity-3d/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to support e-mail (geteamdev@gmail.com)

#region Namespaces

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Object = System.Object;

#endregion

// ######################################################################
// This class is Singleton pattern class.
// It contains AudioClips to play background music and sound of buttons
// Note this is just sample class to work with sample script of  UI Pack: Toony. So it has limiation features.
// ######################################################################
public enum ESounds
{
    None,
    Pass,
    Azi,
    Take,
    Win,
    FishkaOnTable,
    FishkaToCon,
    GiveCards,
    SborCard,
    ShaflCard,
    FishkiToPlayer,
    Card,
}

public class SoundController : SerializedMonoBehaviour
{
    public static int MaxPlaying = 2;
    public static List<AudioSource> Sources = new List<AudioSource>();

    public Dictionary<ESounds, AudioClip> Sounds;
    // public SoundsDictionaty Sounds;
    // Private reference which can be accessed by this class only
    private static SoundController instance;
    public AudioSource Music;
    public class DelayedSound
    {
        public float TimeLeft { get; set; }
        public AudioClip Clip { get; set; }
        public float Length { get; set; }
    }
    /// <summary>
    /// список всех отложенных звуков
    /// </summary>
    private static List<DelayedSound> _delayedSounds= new List<DelayedSound>(5);
    /// <summary>
    /// список запрещенных звуков
    /// </summary>
    private static List<DelayedSound> _lockedSounds= new List<DelayedSound>(5);

    // Public static reference that can be accesd from anywhere
    public static SoundController Instance
    {
        get
        {
            // Check if instance has not been set yet and set it it is not set already
            // This takes place only on the first time usage of this reference
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SoundController>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    // Max number of AudioSource components
    public int m_MaxAudioSource = 8;

    // AudioClip component for music
    public AudioClip m_Music = null;


    // Sound volume
    public float m_SoundVolume = 1.0f;

    // Music volume
    public float m_MusicVolume = 1.0f;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (instance == null)
        {
            // Make the current instance as the singleton
            instance = this;

            // Make it persistent  
            DontDestroyOnLoad(this);
        }
        else
        {
            // If more than one singleton exists in the scene find the existing reference from the scene and destroy it
            if (this != instance)
            {
                InitAudioListener();
                Destroy(this.gameObject);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        // Initial AudioListener
        //  InitAudioListener();
        //Config.Instance.CheckLoad();
        // Automatically play music if it is not playing
        // print("IsMusicPlaying "+ IsMusicPlaying());
        if (IsMusicPlaying() == false)
        {
            // Play music
            Play_Music();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (DelayedSound s in _delayedSounds)
        {
            s.TimeLeft -= Time.deltaTime;
            if (s.TimeLeft <= 0)
            {
                PlaySound(s.Clip, s.Length);
            }
        }
        _delayedSounds.RemoveAll(x => x.TimeLeft <=0);
        foreach (DelayedSound s in _lockedSounds)
        {
            s.TimeLeft -= Time.deltaTime;
        }
        _lockedSounds.RemoveAll(x => x.TimeLeft <=0);
    }


    // Initial AudioListener
    // This function remove all AudioListener in other objects then it adds new one this object.
    void InitAudioListener()
    {
        // Destroy other's AudioListener components
        AudioListener[] pAudioListenerToDestroy = GameObject.FindObjectsOfType<AudioListener>();
        foreach (AudioListener child in pAudioListenerToDestroy)
        {
            //if (child.gameObject.GetComponent<SoundController>() == null)
            //{
            //    Destroy(child);
            //}
        }
        //for (int i = 0; i < 10; i++)
        //{
        //    // Adds new AudioListener to this object
        //    Sources.Add(gameObject.AddComponent<AudioSource>());
        //}

        // Adds new AudioListener to this object
        AudioListener pAudioListener = gameObject.GetComponent<AudioListener>();
        if (pAudioListener == null)
        {
            pAudioListener = gameObject.AddComponent<AudioListener>();
        }
    }

    // Play music
    AudioSource PlayMusic(AudioClip pAudioClip)
    {
        print("PlayMusic " + pAudioClip);
        // Return if the given AudioClip is null
        if (pAudioClip == null)
            return null;

        AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
        AudioSource pAudioSource = null;
        if (pAudioListener != null)
        {


            // If there is not enough AudioListener to play AudioClip then add new one and play it
            if (Music == null)
            {
                Music = pAudioListener.gameObject.AddComponent<AudioSource>();
                Music.rolloffMode = AudioRolloffMode.Linear;
                Music.loop = true;

                Music.clip = pAudioClip;
                // print("!@#!@# " + Music.clip);
                //  pAudioSource.volume = Config.Instance.m_Music;
                Music.ignoreListenerVolume = true;
                Music.playOnAwake = false;
                Music.Play();
            }
            else
            {
                Music.clip = pAudioClip;
                Music.Play();
                //print("!1@#!@# " + Music.clip);
            }
        }
        return Music;
    }
    /// <summary>
    /// прогрываем выбранный звук
    /// </summary>
    /// <param name="sound"></param>
    public static void Play(ESounds sound, float length = 0f)
    {
    //    print("play " + sound);
        AudioClip c;
        if (Instance.Sounds.TryGetValue(sound, out c))
        {
            if (IsAllowToPlay(c))
                Instance.PlaySound(c, length);
  //          else
//                print(c+" blocked");
        }
    }
    /// <summary>
    /// проверяем наличие клипа в списке на запрет к проигрыванию
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public static bool IsAllowToPlay(AudioClip clip)
    {
        return _lockedSounds.All(b => b.Clip != clip);
    }
    /// <summary>
    /// проигрываем клип с задержкой
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="length"></param>
    /// <param name="delay"></param>
    public static void PlayDelayed(ESounds sound, float length = 0f, float delay = 0f)
    {
     //   print("play " + sound);
        AudioClip c;
        if (delay > 0)
        {
            if (Instance.Sounds.TryGetValue(sound, out c))
            {
                _delayedSounds.Add(new DelayedSound() {Clip = c,Length = length,TimeLeft = delay});
            }
        }
    }
    /// <summary>
    /// останавливаем проигрывание выбранного клипа
    /// </summary>
    /// <param name="sound"></param>
    public static void StopSingleSound(ESounds sound)
    {
        AudioClip c;
        if (Instance.Sounds.TryGetValue(sound, out c))
        {
            AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
            //  print("play " + pAudioClip + ":" + pAudioListener);
            if (pAudioListener != null)
            {
                bool IsPlaySuccess = false;
                AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
                print("pAudioSourceList.Length  " + pAudioSourceList.Length);
                for (int i = 0; i < pAudioSourceList.Length; i++)
                {
                    //print(pAudioSourceList[i]);
            //        print(pAudioSourceList[i].clip);
                    if (pAudioSourceList[i].clip == c)
                    {
                        pAudioSourceList[i].Stop();
                    }
                }
            }
            //Instance.StopMusic();//StopMusic();//PlaySoundOneShot(c);
        }
    }
    /// <summary>
    /// останавливаем воспроизведение всех звуков
    /// </summary>
    public static void StopAllSound()
    {
        print("StopAllSound");
        AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
        AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
        for (int i = 1; i < pAudioSourceList.Length; i++)
        {
            //print(i);
            pAudioSourceList[i].Stop();
        }
    }

    public static bool CheckPlaying(AudioClip clip)
    {
        bool flag = true;
        int count = 0;
        if (Sources.Count >= MaxPlaying)
        {
            List<AudioSource> a = new List<AudioSource>(Sources);
            foreach (AudioSource source in a)
            {
                if (source == null || !source.isPlaying || source.clip == null)
                {
                    Sources.Remove(source);
                }
                else
                {
                    if (source.clip == clip)
                    {
                        count++;
                    }
                }
            }
            if (count > MaxPlaying)
                flag = false;
        }
        return flag;
    }
    public static void PlayClip(AudioClip clip, GameObject go)
    {
        if (clip == null)
        {
            // Debug.LogWarning("Нет звукового клипа");
            return;
        }
        if (go == null)
        {
            Debug.LogWarning("Нет GO для проигрывания клипа");
            return;
        }
        if (!CheckPlaying(clip))
        {
            Debug.Log("block " + clip.name);
            return;
        }
        AudioSource source = go.GetComponent<AudioSource>();
        if (source == null)
            source = go.AddComponent<AudioSource>();
        if (source.isPlaying == false)
        {
            source.PlayOneShot(clip);
        }
        Sources.Add(source);
    }
    /// <summary>
    /// блокируем воспроизведение определенного типа звуков на указанное время
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="duration"></param>
    public static void LockSoundPlay(ESounds sound, float duration = 0)
    {
        StopSingleSound(sound);
        AudioClip c;
        if (duration>0 && Instance.Sounds.TryGetValue(sound, out c))
        {
            _lockedSounds.Add(new DelayedSound() {Clip =c,Length = 0,TimeLeft = duration});
        }
    }

    public AudioSource PlaySound(AudioClip pAudioClip, float length = 0f)
    {
        bool found = false;
        AudioSource pAudioSource = null;
        AudioSource[] pAudioSourceList = gameObject.GetComponents<AudioSource>();
        for (int i = 0; i < pAudioSourceList.Length; i++)
        {
            if (pAudioSourceList[i].isPlaying == false)
            {
                pAudioSource = pAudioSourceList[i];
                pAudioSource.clip = pAudioClip;
                pAudioSource.loop = false;
                pAudioSource.Play();
                if (length > 0)
                {
                    pAudioSource.SetScheduledEndTime(AudioSettings.dspTime + length);
                    print(length + "  >  " + pAudioSource.clip.length);
                    if (length > pAudioSource.clip.length)
                        pAudioSource.loop = true;
                }

                found = true;
                break;
            }
        }

        if (!found)
        {
            pAudioSource = gameObject.AddComponent<AudioSource>();
            pAudioSource.clip = pAudioClip;
            pAudioSource.loop = false;
            pAudioSource.Play();
            if (length > 0)
            {
                pAudioSource.SetScheduledEndTime(AudioSettings.dspTime + length);
                if (length > pAudioSource.clip.length)
                    pAudioSource.loop = true;
            }
        }

        return pAudioSource;
    }


    // Play sound one shot
    public AudioSource PlaySoundOneShot(AudioClip pAudioClip, float length = 0)
    {

        // Return if the given AudioClip is null
        if (pAudioClip == null)
            return null;

        //// We wait for a while after scene loaded
        //if (TimeLeft.timeSinceLevelLoad < 1.5f)
        //    return;
        AudioSource pAudioSource = null;
        // Look for an AudioListener component that is not playing background music or sounds.
        AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
        //  print("play " + pAudioClip + ":" + pAudioListener);
        if (pAudioListener != null)
        {
            bool IsPlaySuccess = false;
            AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
            if (pAudioSourceList.Length > 0)
            {
                for (int i = 0; i < pAudioSourceList.Length; i++)
                {
                    if (pAudioSourceList[i].isPlaying == false)
                    {
                        // Play sound
                        pAudioSourceList[i].PlayOneShot(pAudioClip);
                        pAudioSource = pAudioSourceList[i];
                        //          print("!! " + Config.Instance.m_Sound);
                        // pAudioSourceList[i].volume = Config.Instance.m_Sound;
                        break;
                    }
                }
            }

            // If there is not enough AudioListener to play AudioClip then add new one and play it
            if (IsPlaySuccess == false && pAudioSourceList.Length < 16)
            {
                print("else  " + pAudioClip);
                // Play sound
                pAudioSource = pAudioListener.gameObject.AddComponent<AudioSource>();
                pAudioSource.rolloffMode = AudioRolloffMode.Linear;
                //    print("!!11 " + Config.Instance.m_Sound);
                pAudioSource.playOnAwake = false;
                pAudioSource.PlayOneShot(pAudioClip);
            }
        }

        return pAudioSource;
    }

    // Set music volume between 0.0 to 1.0
    public void SetMusicVolume(float volume)
    {
        // print("SetMusicVolume " + volume);
        if (Music != null)
            Music.volume = volume;
        //m_MusicVolume = volume;

        //AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
        //if (pAudioListener != null)
        //{
        //    AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
        //    if (pAudioSourceList.Length > 0)
        //    {
        //        for (int i = 0; i < pAudioSourceList.Length; i++)
        //        {
        //            if (pAudioSourceList[i].ignoreListenerVolume)
        //            {
        //                pAudioSourceList[i].volume = volume;
        //               // print("!" + pAudioSourceList[i].volume);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.LogError("не нашли AudioListener");
        //}
    }

    // If music is playing, return true.
    public bool IsMusicPlaying()
    {
        AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
        if (pAudioListener != null)
        {
            AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
            if (pAudioSourceList.Length > 0)
            {
                for (int i = 0; i < pAudioSourceList.Length; i++)
                {
                    if (pAudioSourceList[i].ignoreListenerVolume == true)
                    {
                        if (pAudioSourceList[i].isPlaying == true)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    // Set sound volume between 0.0 to 1.0
    public void SetSoundVolume(float volume)
    {
        m_SoundVolume = volume;
        AudioListener.volume = volume;
    }

    // Play music
    public void Play_Music(AudioClip clip = null)
    {
        //print("Play_Music " + clip);
        if (clip == null)
        {
            Music = PlayMusic(m_Music);
        }
        else
        {
            Music = PlayMusic(clip);
        }
        SetMusicVolume(m_MusicVolume);
    }

    public void StopMusic()
    {
        Music.Stop();
    }

   
}

