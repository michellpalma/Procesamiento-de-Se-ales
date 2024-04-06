using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioRecorder : MonoBehaviour
{
    private string fileName = "grabacion.wav";
    private int duration = 5; // Duraci�n de la grabaci�n en segundos
    private AudioClip recordedClip;
    private bool isRecording = false;

    void Start()
    {
        // Asigna la funci�n StartRecording() al evento onClick del bot�n
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartRecording);
    }

    void StartRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            // Inicia la grabaci�n desde el micr�fono por el tiempo especificado
            recordedClip = Microphone.Start(null, false, duration, 44100);
            // Espera a que termine la grabaci�n
            Invoke("StopRecording", duration);
        }
    }

    void StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            // Detiene la grabaci�n y guarda el clip
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

        Debug.Log("Grabaci�n guardada como " + filePath);
    }

    // M�todo para reproducir el audio grabado
    public void PlayRecording()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            StartCoroutine(PlayAudioFile(filePath));
        }
        else
        {
            Debug.LogError("Archivo de grabaci�n no encontrado");
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

        // Espera hasta que se complete la reproducci�n
        yield return new WaitForSeconds(audioSource.clip.length);

        // Elimina el componente AudioSource despu�s de reproducir el audio
        Destroy(audioSource);
    }

}
