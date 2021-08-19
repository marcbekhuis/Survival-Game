using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSave
{
    public List<AudioVolume> audioVolumes;

    public AudioSave(List<AudioVolume> audioVolumes)
    {
        this.audioVolumes = audioVolumes;
    }

    public float GetVolume(string group)
    {
        foreach (var audioVolume in audioVolumes)
        {
            if (audioVolume.group.Contains(group)) return audioVolume.volume;
        }

        return 0;
    }

    public void SetVolume(string group, float volume)
    {
        foreach (var audioVolume in audioVolumes)
        {
            if (audioVolume.group.Contains(group))
            {
                audioVolume.volume = volume;
                return;
            }
        }
    }

    public bool HasVolume(string group)
    {
        foreach (var audioVolume in audioVolumes)
        {
            if (audioVolume.group.Contains(group)) return true;
        }

        return false;
    }

    public void RemoveVolume(string group)
    {
        for (int i = 0; i < audioVolumes.Count; i++)
        {
            if (audioVolumes[i].group.Contains(group))
            {
                audioVolumes.RemoveAt(i);
                return;
            }
        }
    }

    public void AddVolume(string group, float volume)
    {
        audioVolumes.Add(new AudioVolume(group, volume));
    }

    [System.Serializable]
    public class AudioVolume 
    {
        public string group;
        public float volume;

        public AudioVolume(string group, float volume)
        {
            this.group = group;
            this.volume = volume;
        }
    }
}
