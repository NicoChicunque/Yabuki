using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TemplatesManager : MonoBehaviour
{
    [SerializeField] private Transform installed, download;
    //Dictionary<string, string> downloadTemplates = new Dictionary<string, string>();
    [SerializeField] private List<string> downloadTemplates = new List<string>();
    [SerializeField] private TogglePackage togglePackageTemplate;
    private List<TogglePackage> tooglePackages = new List<TogglePackage>();

    public void UpdateTemplates(string unityInstallPath)
    {
        for (int i = 0; i < tooglePackages.Count; i++) { Destroy(tooglePackages[i].gameObject); }
        tooglePackages.Clear();

        string templateFolder = Path.Combine(Path.GetDirectoryName(unityInstallPath), "Data/Resources/PackageManager/ProjectTemplates/");        
        if ( ! Directory.Exists(templateFolder)) return;
        List<string> fileEntries = Directory.GetFiles(templateFolder).ToList();

        for (int i = 0; i < downloadTemplates.Count; i++)
        {            
            // check if its tgz
            //if (fileEntries[i].IndexOf(".tgz") == -1)
            //{
            //    Debug.Log(fileEntries[i]);
            //}
            //else
            //{
            //    // cleanup name
            //string name = Path.GetFileName(fileEntries[i]).Replace("com.unity.template.", "").Replace(".tgz", "");
            //downloadTemplates.Add(fileEntries[i]);
            Transform parent = download;
            for (int j = 0; j < fileEntries.Count; j++)
            {
                if (fileEntries[j].Contains(downloadTemplates[i])){ parent = installed; break; }
            }

            TogglePackage togglePackage = Instantiate(togglePackageTemplate, parent);
            togglePackage.gameObject.SetActive(true);
            togglePackage.Name = downloadTemplates[i];
            tooglePackages.Add(togglePackage);
        }
    }
}
