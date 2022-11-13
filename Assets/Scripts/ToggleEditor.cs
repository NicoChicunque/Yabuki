using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ToggleEditor : MonoBehaviour
{
    [SerializeField] private string path, uninstallerPath;//, platformsCombined;
    [SerializeField] private DateTime? installed, releaseDate;
    [SerializeField] private List<string> platformsNames = new List<string>();
    [SerializeField] private int projectCount;
    [SerializeField] private bool isPreferred;

    
    public string Path { get => path; set => path = value; }
    //public string PlatformsCombined { get => platformsCombined; set => platformsCombined = value; }
    public DateTime? Installed { get => installed; set => installed = value; }
    public List<string> PlatformsNames { get => platformsNames; set => platformsNames = value; }
    public int ProjectCount { get => projectCount; set => projectCount = value; }//No debe ir, deben ir directamente los projects si se quiere
    public bool IsPreferred { get => isPreferred; set => isPreferred = value; }
    public string UninstallerPath { get => uninstallerPath; set => uninstallerPath = value; }
    //public List<Project> projects { set; get; }

    [SerializeField] private Text textVersion;
    [SerializeField] private Toggle toggleUse;
    [SerializeField] private ModulesManager modulesManager;
    public string Version { get => textVersion.text; set => textVersion.text = value; }
    public bool IsCheckedToUse { get => toggleUse.isOn; set => toggleUse.isOn = value; }
    public DateTime? ReleaseDate { get => releaseDate; set => releaseDate = value; }

    private void Start()
    {        
        toggleUse.onValueChanged.AddListener(
            value => {
                if (!value) { return; }
                modulesManager.UpdateModulesIsDownloaded(PlatformsNames);

                //Hay que dar una espera...
                //modulesManager.UpdatePlatformIsCheckedToUse("WebGL");
            }
        );
        //Version = "2077.7.7";
        //IsCheckedToUse = true; //Llamar desde open project de acuerdo a la version del editor del proyecto
    }
}
