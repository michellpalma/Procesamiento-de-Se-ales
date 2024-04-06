using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioPlayer : MonoBehaviour
{
    public string fileName = "grabacion.wav"; // Nombre del archivo de grabación
    private AudioClip audioClip;

    void Start()
    {
        // Carga el archivo de audio
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            audioClip = LoadAudioClip(filePath);
            if (audioClip != null)
            {
                // Reproduce el audio con la frecuencia de muestreo especificada
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
            }
            else
            {
                Debug.LogError("Error al cargar el archivo de audio.");
            }
        }
        else
        {
            Debug.LogError("Archivo de grabación no encontrado");
        }
    }

    AudioClip LoadAudioClip(string path)
    {
        // Carga el archivo de audio en un AudioClip
        WWW www = new WWW("file://" + path);
        while (!www.isDone) { }
        AudioClip clip = www.GetAudioClip(false, false);
        return clip;
    }

    public void PlayRecordingAt250Hz()
    {
        if (audioClip != null)
        {
            PlayAudioAtFrequency(250);
        }
    }

    public void PlayRecordingAt500Hz()
    {
        if (audioClip != null)
        {
            PlayAudioAtFrequency(500);
        }
    }

    public void PlayRecordingAt1KHz()
    {
        if (audioClip != null)
        {
            PlayAudioAtFrequency(1000);
        }
    }

    public void PlayRecordingAt4KHz()
    {
        if (audioClip != null)
        {
            PlayAudioAtFrequency(4000);
        }
    }

    void PlayAudioAtFrequency(int frequency)
    {
        int standardFrequency = GetNearestStandardFrequency(frequency);
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.pitch = (float)standardFrequency / audioClip.frequency;
        audioSource.Play();
    }

    int GetNearestStandardFrequency(int desiredFrequency)
    {
        // Devuelve la frecuencia estándar más cercana a la frecuencia deseada
        int[] standardFrequencies = { 44100, 48000, 96000 }; // Frecuencias estándar comunes
        int nearestFrequency = standardFrequencies[0];
        int minDifference = Mathf.Abs(standardFrequencies[0] - desiredFrequency);
        foreach (int freq in standardFrequencies)
        {
            int difference = Mathf.Abs(freq - desiredFrequency);
            if (difference < minDifference)
            {
                nearestFrequency = freq;
                minDifference = difference;
            }
        }
        return nearestFrequency;
    }
}
