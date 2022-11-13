using System.Collections.Generic;
using UnityEngine;

public class ModulesManager : MonoBehaviour
{
    [SerializeField] private List<Module> modules;
    [SerializeField] private List<Platform> platforms;

    //private void Start()
    //{
    //    UpdateModulesIsDownloaded(new List<string> { "Windows", "Android" });
    //    UpdatePlatformIsCheckedToUse("OSX");
    //}

    public void UpdateModulesIsDownloaded(List<string> modulesNames)
    {
        List<Module> allModules = new List<Module>();
        allModules.AddRange(modules);
        allModules.AddRange(platforms);

        for (int i = 0; i < allModules.Count; i++)
        {
            allModules[i].IsDownloaded = modulesNames.Contains(allModules[i].Name);
        }
    }

    public void UpdatePlatformIsCheckedToUse(string platformName)
    {
        Platform platformToUse = platforms.Find(x => x.Name == platformName);
        platformToUse.IsCheckedToUse = true;
    }
}
