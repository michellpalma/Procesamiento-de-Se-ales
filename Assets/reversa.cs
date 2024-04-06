using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Revers: MonoBehaviour
{
    public AudioClip seassons; // Asigna el AudioClip "seassons" desde el Inspector

    public void PlayNormalAudio()
    {
        // Reproducir el audio normal
        PlayAudio(seassons);
    }

    public void PlayReversedAudio()
    {
        // Reproducir el audio al rev�s
        InvertAndPlayReversedAudio(seassons);
    }

    void PlayAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("No se ha asignado ning�n AudioClip.");
            return;
        }

        // Reproducir el audio
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        // Destruir el AudioSource para liberar recursos despu�s de la reproducci�n
        Destroy(audioSource, clip.length);
    }

    void InvertAndPlayReversedAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("No se ha asignado ning�n AudioClip.");
            return;
        }

        // Invertir las muestras del AudioClip
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        System.Array.Reverse(samples);

        // Crear un nuevo AudioClip con las muestras invertidas
        AudioClip reversedAudioClip = AudioClip.Create("ReversedAudio", samples.Length, clip.channels, clip.frequency, false);
        reversedAudioClip.SetData(samples, 0);

        // Reproducir el audio invertido
        AudioSource reversedAudioSource = gameObject.AddComponent<AudioSource>();
        reversedAudioSource.clip = reversedAudioClip;
        reversedAudioSource.Play();

        // Destruir el AudioSource para liberar recursos despu�s de la reproducci�n
        Destroy(reversedAudioSource, reversedAudioClip.length);
    }

}
