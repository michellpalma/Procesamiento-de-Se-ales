using UnityEngine;
using System;

public static class WavUtility
{
    public static AudioClip ToAudioClip(byte[] wavData, string name = "temp")
    {
        // Extraer los datos del archivo WAV
        int pos = 0;
        pos += 20; // Salta los primeros 20 bytes del encabezado WAV (no necesarios para crear el AudioClip)

        // Obtener el formato de audio
        int format = wavData[pos] & 0xFF;
        pos += 2;

        // Obtener el número de canales
        int channels = wavData[pos] & 0xFF;
        pos += 2;

        // Obtener la frecuencia de muestreo
        int sampleRate = BitConverter.ToInt32(wavData, pos);
        pos += 4;

        pos += 6; // Salta los siguientes 6 bytes del encabezado WAV (no necesarios para crear el AudioClip)

        // Obtener el tamaño del bloque de muestras y la precisión de bits
        int blockAlign = wavData[pos] & 0xFF;
        pos += 2;
        int bitDepth = wavData[pos] & 0xFF;
        pos += 2;

        // Verificar si el formato de audio es compatible (PCM no comprimido)
        if (format != 1 || bitDepth != 16)
        {
            Debug.LogError("Formato de audio no compatible. El formato debe ser PCM y la profundidad de bits debe ser de 16.");
            return null;
        }

        // Obtener los datos de audio
        int dataSize = BitConverter.ToInt32(wavData, pos);
        pos += 4;

        // Crear un arreglo de muestras
        float[] samples = new float[dataSize / 2]; // 2 bytes por muestra

        // Convertir los datos de audio a muestras
        for (int i = 0; i < dataSize / 2; i++)
        {
            short sample = BitConverter.ToInt16(wavData, pos);
            samples[i] = sample / 32768f; // Normalizar las muestras a valores entre -1 y 1
            pos += 2;
        }

        // Crear el AudioClip
        AudioClip audioClip = AudioClip.Create(name, dataSize / 2, channels, sampleRate, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }
}