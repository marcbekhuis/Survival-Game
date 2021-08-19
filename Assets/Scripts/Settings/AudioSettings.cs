using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private Transform slidersParent;
    [SerializeField] private ContentSizer sliderContentSizer;

    private AudioSave audioSave;

    private void Start()
    {
        if (!JsonSavingLoading.LoadData<AudioSave>(out audioSave, "Settings", "AudioSetting"))
        {
            SetupAudioSave();
        }
        SetupSliders();
    }

    public void SetupAudioSave()
    {
        audioSave = new AudioSave(new List<AudioSave.AudioVolume>());
        foreach (var group in audioMixer.FindMatchingGroups("Master"))
        {
            float volume = 0;
            audioMixer.GetFloat(group.name + " Volume", out volume);
            audioSave.AddVolume(group.name, volume);
        }
    }

    public void SetupSliders()
    {
        foreach (var group in audioMixer.FindMatchingGroups("Master"))
        {
            GameObject sliderGameObject = Instantiate(sliderPrefab, slidersParent);
            sliderGameObject.GetComponentInChildren<Text>().text = group.name;

            Slider slider = sliderGameObject.GetComponentInChildren<Slider>();
            slider.value = audioSave.GetVolume(group.name);
            slider.onValueChanged.AddListener((float lol) => SetVolume(slider.value, group.name));
        }

        //sliderContentSizer.SetContentSize();
    }

    private void SetVolume(float volume, string group)
    {
        audioMixer.SetFloat(group + " Volume", volume);
        if (audioSave.HasVolume(group)) audioSave.SetVolume(group, volume);
        else audioSave.AddVolume(group, volume);
        JsonSavingLoading.SaveData<AudioSave>(audioSave, "Settings", "AudioSetting");
    }
}
