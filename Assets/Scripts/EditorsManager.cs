using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class EditorsManager : MonoBehaviour
{
    [SerializeField] private string editorsPath = "C:/Program Files/Unity/Hub/Editor/";
    [SerializeField] private ToggleEditor toggleEditorTemplate;
    private List<ToggleEditor> toggleEditors = new List<ToggleEditor>();
    [SerializeField] private int rowsChildAllowed = 3;
    [SerializeField] private Transform rowTemplate, rowN;
    private List<Transform> rows = new List<Transform>();

    public string EditorsPath { get => editorsPath; set {           
            if (string.IsNullOrWhiteSpace(value)) { print("NewValueForEditorsPathIsNullOrWhiteSpace"); return; }
            editorsPath = value;
        } 
    }

    void Start()
    {
        GetEditors();
    }

    private void GetEditors()
    {
        for (int i = 0; i < toggleEditors.Count; i++) { Destroy(toggleEditors[i].gameObject); }
        toggleEditors.Clear();
        string[] directories = Directory.GetDirectories(EditorsPath);
        for (int i = 0; i < directories.Length; i++)
        {
            string editorFolder = Path.Combine(directories[i], "Editor");
            if (! Directory.Exists(editorFolder)) { continue; }
            string unityExePath = Path.Combine(editorFolder, "Unity.exe");
            if (! File.Exists(unityExePath)) { continue; }
            string dataFolder = Path.Combine(editorFolder, "Data");

            if (i % rowsChildAllowed == 0)
            {
                rowN = Instantiate(rowTemplate, rowTemplate.parent);
                rowN.gameObject.SetActive(true);
                rows.Add(rowN);
            }

            ToggleEditor toggleEditor = Instantiate(toggleEditorTemplate, rowN);
            toggleEditor.gameObject.SetActive(true);
            toggleEditor.Version = FileVersionInfo.GetVersionInfo(unityExePath).ProductVersion;// FileVersionInfo.GetVersionInfo(unityExePath).ProductName.Replace("(64-bit)", "").Replace("(32-bit)", "").Replace("Unity", "").Trim();
            toggleEditor.Path = unityExePath;
            toggleEditor.UninstallerPath = Path.Combine(editorFolder, "Uninstall.exe");
            toggleEditor.Installed = (File.Exists(dataFolder) || Directory.Exists(dataFolder)) ? File.GetLastWriteTime(dataFolder) : null;
            toggleEditor.IsPreferred = false;//No lo veo necesario...
            toggleEditor.ProjectCount = 0;
            toggleEditor.PlatformsNames = GetPlatforms(dataFolder);
            //if (toggleEditors.Contains(toggleEditor)) { continue; }
            toggleEditors.Add(toggleEditor);
        }
    }

    public List<string> GetPlatforms(string dataFolder)
    {
        Dictionary<string, string> platformNames = new Dictionary<string, string> {
            { "windowsstandalonesupport", "Windows" },
            { "androidplayer", "Android" },
            { "linuxstandalonesupport", "Linux" },
            { "linuxstandalone", "Linux" },
            { "osxstandalone", "OSX" },
            { "webglsupport", "WebGL" },
            { "metrosupport", "UWP" },
            { "iossupport", "iOS" }
            //Saber mas nombres de las carpetas, por ejemplo consolas, ojo en este diccionario esta todo en minusculas
        };

        List<string> directories = new List<string>(Directory.GetDirectories(Path.Combine(dataFolder, "PlaybackEngines")));

        for (int i = 0; i < directories.Count; i++)
        {
            string foldername = Path.GetFileName(directories[i]).ToLower();
            directories[i] = platformNames.ContainsKey(foldername) ?  platformNames[foldername] : foldername;
        }

        return directories;
    }
}
