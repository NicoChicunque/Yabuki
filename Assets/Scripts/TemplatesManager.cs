using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TemplatesManager : MonoBehaviour
{
    Dictionary<string, string> templates = new Dictionary<string, string>();

    public void ScanTemplates(string unityInstallPath)
    {
        var templateFolder = Path.Combine(Path.GetDirectoryName(unityInstallPath), "Data/Resources/PackageManager/ProjectTemplates/");
        
        //if ( ! Directory.Exists(templateFolder)) return templates;

        List<string> fileEntries = Directory.GetFiles(templateFolder).ToList();

        for (int i = 0; i < fileEntries.Count; i++)
        {
            
            // check if its tgz
            if (fileEntries[i].IndexOf(".tgz") == -1)
            {
                Debug.Log(fileEntries[i]);
            }
            //else
            //{
            //    // cleanup name
            string name = Path.GetFileName(fileEntries[i]).Replace("com.unity.template.", "").Replace(".tgz", "");
            templates.Add(name, fileEntries[i]);
            //}
        }

        //foreach (var template in templates)
        //{
        //    Debug.Log(template.Key + " --- " + template.Value);
        //}
    }
}
