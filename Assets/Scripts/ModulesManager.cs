using System.Collections.Generic;
using UnityEngine;

public class ModulesManager : MonoBehaviour
{
    [SerializeField] private Transform installed, download;
    [SerializeField] private Platform platformTemplate;
    [SerializeField] private Module moduleTemplate;
    [SerializeField] private List<string> downloadPlatforms = new List<string>(), downloadModules = new List<string>();
       // ,installedPlatforms = new List<string>(),installedModules = new List<string>();
    private List<Platform> platforms = new List<Platform>();
    private List<Module> modules = new List<Module>();

    //private void Start()
    //{
        //UpdateModulesIsDownloaded(new List<string> { "Windows", "Android" });
        //UpdatePlatformIsCheckedToUse("OSX");        
    //}

    public void UpdateModulesIsDownloaded(List<string> installedPlatformsNames)
    {
        for (int i = 0; i < platforms.Count; i++) { Destroy(platforms[i].gameObject); }
        platforms.Clear();
        for (int i = 0; i < modules.Count; i++) { Destroy(modules[i].gameObject); }
        modules.Clear();

        for (int i = 0; i < downloadPlatforms.Count; i++)
        {
            Platform platform = Instantiate(platformTemplate, installedPlatformsNames.Contains(downloadPlatforms[i]) ? installed : download);
            platform.gameObject.SetActive(true);
            platform.Name = downloadPlatforms[i];
            platform.IsDownloaded = installedPlatformsNames.Contains(downloadPlatforms[i]);
            platforms.Add(platform);
        }

        for (int i = 0; i < downloadModules.Count; i++)
        {
            Module module = Instantiate(moduleTemplate, installedPlatformsNames.Contains(downloadModules[i]) ? installed : download);
            module.gameObject.SetActive(true);
            module.Name = downloadModules[i];
            module.IsDownloaded = installedPlatformsNames.Contains(downloadModules[i]);
            modules.Add(module);
        }

        if(platforms.Count.Equals(0)) { return; }
        UpdatePlatformIsCheckedToUse(platforms[0].Name);
    }

    public void UpdatePlatformIsCheckedToUse(string platformName)
    {
        Platform platformToUse = platforms.Find(x => x.Name == platformName);
        platformToUse.IsCheckedToUse = true;
    }
}
