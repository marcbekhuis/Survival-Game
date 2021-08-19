using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private Dropdown qualityDropdown;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    public void SetupGraghicsUI()
    {
        //Quality dropdown
        qualityDropdown.ClearOptions();
        foreach (var name in QualitySettings.names)
        {
            qualityDropdown.AddOptions(new List<Dropdown.OptionData> {new Dropdown.OptionData(name) });
        }
    }
}
 