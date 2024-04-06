using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class punto2 : MonoBehaviour
{
    public AudioClip shyFrogClip; // Referencia al archivo de audio pregrabado
    private AudioSource audioSource; // Referencia al componente AudioSource
    private float targetVolume = 0.5f; // Nivel de volumen deseado para todos los clips

    void Start()
    {
        // Obtener o agregar componente AudioSource al GameObject
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Obtener referencias a los botones
        Button button1 = GameObject.Find("Button1").GetComponent<Button>();
        Button button2 = GameObject.Find("Button2").GetComponent<Button>();
        Button button3 = GameObject.Find("Button3").GetComponent<Button>();
        Button button4 = GameObject.Find("Button4").GetComponent<Button>();
        Button button5 = GameObject.Find("Button5").GetComponent<Button>();
        Button button6 = GameObject.Find("Button6").GetComponent<Button>();

        // Asociar métodos a los eventos de clic de los botones
        button1.onClick.AddListener(() => PlayAudioWith1Bit());
        button2.onClick.AddListener(() => PlayAudioWith8Bits());
        button3.onClick.AddListener(() => PlayAudioWith12Bits());
        button4.onClick.AddListener(() => PlayAudioWith16Bits());
        button5.onClick.AddListener(() => PlayAudioWith24Bits());
        button6.onClick.AddListener(() => PlayAudioWith32Bits());
    }

    void PlayAudioWith1Bit()
    {
        PlayAudioWithResolution(shyFrogClip, 1);
    }

    void PlayAudioWith8Bits()
    {
        PlayAudioWithResolution(shyFrogClip, 8);
    }

    void PlayAudioWith12Bits()
    {
        PlayAudioWithResolution(shyFrogClip, 12);
    }

    void PlayAudioWith16Bits()
    {
        PlayAudioWithResolution(shyFrogClip, 16);
    }

    void PlayAudioWith24Bits()
    {
        PlayAudioWithResolution(shyFrogClip, 24);
    }

    void PlayAudioWith32Bits()
    {
        PlayAudioWithResolution(shyFrogClip, 32);
    }

    void PlayAudioWithResolution(AudioClip clip, int resolution)
    {
        // Obtener las muestras de audio
        float[] audioData;
        AudioDataHandler.ConvertAudioClipToPCMFloat(clip, out audioData);

        // Aplicar la resolución deseada a las muestras de audio
        AdjustResolution(audioData, resolution);

        // Crear nuevo clip de audio con las muestras modificadas
        AudioClip newClip = AudioClip.Create("NewClip", audioData.Length, 1, clip.frequency, false);
        newClip.SetData(audioData, 0);

        // Reproducir el nuevo clip de audio
        audioSource.clip = newClip;

        // Ajustar el volumen del clip
        AdjustVolume(audioData);

        audioSource.Play();
    }

    void AdjustResolution(float[] audioData, int resolution)
    {
        if (resolution == 1)
        {
            for (int i = 0; i < audioData.Length; i++)
            {
                audioData[i] = (audioData[i] >= 0) ? 1f : -1f;
            }
        }
        else if (resolution == 32)
        {
            // Para 32 bits, simplemente se mantienen las muestras originales sin ajuste
            // No necesitamos aplicar ningún cambio en este caso
        }
        else
        {
            float maxSampleValue = Mathf.Pow(2, resolution - 1) - 1;
            for (int i = 0; i < audioData.Length; i++)
            {
                audioData[i] *= maxSampleValue / Mathf.Pow(2, 15); // Ajustar según el rango de las muestras originales (-1 a 1)
            }
        }
    }

    void AdjustVolume(float[] audioData)
    {
        float maxAmplitude = 0f;
        for (int i = 0; i < audioData.Length; i++)
        {
            maxAmplitude = Mathf.Max(maxAmplitude, Mathf.Abs(audioData[i]));
        }

        float currentVolume = audioSource.volume;
        float newVolume = targetVolume / maxAmplitude;
        audioSource.volume = Mathf.Min(currentVolume, newVolume);
    }
}

public static class AudioDataHandler
{
    public static void ConvertAudioClipToPCMFloat(AudioClip clip, out float[] audioData)
    {
        audioData = new float[clip.samples * clip.channels];
        clip.GetData(audioData, 0);
    }
}
