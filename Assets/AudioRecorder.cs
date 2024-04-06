using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioRecorder : MonoBehaviour
{
    private string fileName = "grabacion.wav";
    private int duration = 5; // Duración de la grabación en segundos
    private AudioClip recordedClip;
    private bool isRecording = false;

    void Start()
    {
        // Asigna la función StartRecording() al evento onClick del botón
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartRecording);
    }

    void StartRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            // Inicia la grabación desde el micrófono por el tiempo especificado
            recordedClip = Microphone.Start(null, false, duration, 44100);
            // Espera a que termine la grabación
            Invoke("StopRecording", duration);
        }
    }

    void StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            // Detiene la grabación y guarda el clip
            Microphone.End(null);
            SaveRecording();
        }
    }

    void SaveRecording()
    {
        // Convierte el clip a un array de bytes
        float[] samples = new float[recordedClip.samples];
        recordedClip.GetData(samples, 0);
        byte[] byteArray = new byte[samples.Length * 2];
        System.Buffer.BlockCopy(samples, 0, byteArray, 0, byteArray.Length);

        // Crea el archivo WAV
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, byteArray);

        Debug.Log("Grabación guardada como " + filePath);
    }

    // Método para reproducir el audio grabado
    public void PlayRecording()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            StartCoroutine(PlayAudioFile(filePath));
        }
        else
        {
            Debug.LogError("Archivo de grabación no encontrado");
        }
    }

    IEnumerator PlayAudioFile(string filePath)
    {
        // Carga el archivo de audio
        WWW www = new WWW("file://" + filePath);
        yield return www;

        // Reproduce el audio
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = www.GetAudioClip();
        audioSource.Play();

        // Espera hasta que se complete la reproducción
        yield return new WaitForSeconds(audioSource.clip.length);

        // Elimina el componente AudioSource después de reproducir el audio
        Destroy(audioSource);
    }

}
