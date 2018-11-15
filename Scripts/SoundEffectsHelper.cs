using UnityEngine;
using System.Collections;

/// <summary>
/// Creating instance of sounds from code with no effort
/// </summary>
public class SoundEffectsHelper : MonoBehaviour
{

    /// <summary>
    /// Singleton
    /// </summary>
    public static SoundEffectsHelper Instance;

    public AudioSource explosionSound;
    public AudioSource TauntSound;
    public AudioSource enemyShotSound;
    public AudioSource bulletSound;
    public AudioSource sambaSound;
    public AudioSource horseSound;
    public AudioSource bomerangSound;
    public AudioSource gabbarSound;
    public AudioSource taunt1;
    public AudioSource taunt2;
    public AudioSource taunt3;
    public AudioSource taunt4;
    public AudioSource laganishana;

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundEffectsHelper!");
        }
        Instance = this;
        explosionSound = GameObject.Find("explosionsound").GetComponent<AudioSource>();
        bulletSound = GameObject.Find("dhichkyau").GetComponent<AudioSource>();
        sambaSound = GameObject.Find("sambha").GetComponent<AudioSource>();
        horseSound = GameObject.Find("tigdik").GetComponent<AudioSource>();
        enemyShotSound = GameObject.Find("enemyShotSound").GetComponent<AudioSource>();
        bomerangSound = GameObject.Find("bomerangSound").GetComponent<AudioSource>();
        gabbarSound = GameObject.Find("gabbarLaugh").GetComponent<AudioSource>();
        taunt1 = GameObject.Find("taunt1").GetComponent<AudioSource>();
        taunt2 = GameObject.Find("taunt2").GetComponent<AudioSource>();
        taunt3 = GameObject.Find("taunt3").GetComponent<AudioSource>();
        taunt4 = GameObject.Find("taunt4").GetComponent<AudioSource>();
        laganishana = GameObject.Find("laganishana").GetComponent<AudioSource>();
    }

    public void MakeExplosionSound()
    {
        explosionSound.Play();
    }

    public void MakeTauntSound()
    {
    }

    public void MakeEnemyShotSound()
    {
        enemyShotSound.Play();
    }

    public void MakeSambaSound()
    {
        sambaSound.Play();
    }
     
    public void MakeHorseSound()
    {
        horseSound.Play();
    }

    public void MakeBulletSound()
    {
        bulletSound.Play();
    }

    public void MakeBomerangSound()
    {
        bomerangSound.Play();
    
    }

    public void MakeGabbarSound()
    {
        gabbarSound.Play();
    }

    public void MakeLaganishanaSound()
    {
        laganishana.Play();
    }

    public void MaketauntSound(int index)
    {
        if (index == 1) taunt1.Play();
        if (index == 2) taunt2.Play();
        if (index == 3) taunt3.Play();
        if (index == 4) taunt4.Play();
        if (index == 5) gabbarSound.Play();
    }

    public void StopHorseSound()
    {
        horseSound.Stop();
    }

    public void StopBomerangSound()
    {
        bomerangSound.Stop();
    }
    /// <summary>
    /// Play a given sound

}