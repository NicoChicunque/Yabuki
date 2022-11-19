using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EditorsManager : MonoBehaviour
{
    [SerializeField] private EditorsCreator editorsCreatorInstalled, editorsCreatorDownloadArchive;
    [SerializeField] private Text textEditorsPath;
    [SerializeField] private PopUp popUp;
    [SerializeField] private List<string> installedEditors = new List<string>(), downloadArchiveEditors = new List<string>();

    [SerializeField] private ToggleEditorArchive editorArchiveTemplate;
    private List<ToggleEditorArchive> toggleEditorArchives = new List<ToggleEditorArchive>();

    [SerializeField] private TemplatesManager templatesManager;

    public string EditorsPath { get => textEditorsPath.text; set {
            textEditorsPath.text = value;
            GetInstalledEditors();
            GetDownloadArchiveEditors();//SIN INTERNET                      
        }
    }//"C:\\Program Files\\Unity\\Hub\\Editor"

    void Start() { EditorsPath = textEditorsPath.text; }

    private void GetInstalledEditors()
    {
        string[] directories = Directory.GetDirectories(EditorsPath);
        installedEditors.Clear();
        List<List<string>> platformNamesList = new List<List<string>>();

        string path = string.Empty;//Prueba

        for (int i = 0; i < directories.Length; i++)
        {
            string editorFolder = Path.Combine(directories[i], "Editor");
            if (! Directory.Exists(editorFolder)) { continue; }
            string unityExePath = Path.Combine(editorFolder, "Unity.exe");
            if (! File.Exists(unityExePath)) { continue; }
            string dataFolder = Path.Combine(editorFolder, "Data");
            string[] versionParts = FileVersionInfo.GetVersionInfo(unityExePath).ProductVersion.Split('_');
            installedEditors.Add(versionParts[0]);
            platformNamesList.Add(GetInstalledPlatforms(dataFolder));
            //toggleEditor.Path = unityExePath;
            //toggleEditor.UninstallerPath = Path.Combine(editorFolder, "Uninstall.exe");
            //toggleEditor.Installed = (File.Exists(dataFolder) || Directory.Exists(dataFolder)) ? File.GetLastWriteTime(dataFolder) : null;
            //toggleEditor.IsPreferred = false;//No lo veo necesario...
            //toggleEditor.ProjectCount = 0;
            //
            if(i.Equals(directories.Length-1)) { path = unityExePath; }//Prueba
        }
        installedEditors.Reverse();
        editorsCreatorInstalled.CreateToggleEditors(installedEditors, platformNamesList);

        templatesManager.UpdateTemplates(path);//Prueba
    }

    private List<string> GetInstalledPlatforms(string dataFolder)
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

    private void GetDownloadArchiveEditors()
    {
        string allEditors = Task.Run(GetAllEditorsText).Result;
        if (allEditors.Contains("Error") || allEditors.Equals(string.Empty)) 
        {
            popUp.Show($"ErrorGetDownloadArchiveEditors {allEditors}", () => { popUp.Close(); });
            return; 
        }

        List<string> dirtyEditorsList = new List<string>(allEditors.Split(new[] { Environment.NewLine }, StringSplitOptions.None));

        if (dirtyEditorsList.Count.Equals(0))
        {
            popUp.Show("http://symbolserver.unity3d.com/000Admin/history.txt don´t get any Editors", () => { });
            return;
        }

        downloadArchiveEditors.Clear();

        List<string> majorVersions = new List<string>();

        for (int i = 0; i < dirtyEditorsList.Count; i++)
        {
            string[] row = dirtyEditorsList[i].Split(',');
            string version = row[6].Trim('"');            
            if (string.IsNullOrEmpty(version)) { continue; }
            if (downloadArchiveEditors.Contains(version)) { continue; }
            if (installedEditors.Contains(version)) { continue; }
            downloadArchiveEditors.Add(version);
            string[] versionParts = version.Split('.');
            if (!majorVersions.Contains(versionParts[0]) && !versionParts[0].Equals("5"))
            {
                majorVersions.Add(versionParts[0]);
            }
            //toggleEditor.ReleaseDate = DateTime.ParseExact(row[3], "MM/dd/yyyy", CultureInfo.InvariantCulture);   
        }

        for (int i = 0; i < toggleEditorArchives.Count; i++) { Destroy(toggleEditorArchives[i].gameObject); }
        toggleEditorArchives.Clear();

        majorVersions.Reverse();

        for (int i = 0; i < majorVersions.Count; i++)
        {
            ToggleEditorArchive toggleEditorArchive = Instantiate(editorArchiveTemplate, editorArchiveTemplate.transform.parent);
            toggleEditorArchive.gameObject.SetActive(true);
            toggleEditorArchive.MajorVersion = majorVersions[i];
            toggleEditorArchives.Add(toggleEditorArchive);
        }

        toggleEditorArchives[0].GetComponent<Toggle>().isOn = true;
    }

    private async Task<string> GetAllEditorsText()
    {
        string result = string.Empty;
        using (WebClient webClient = new WebClient())
        {
            Task<string> downloadStringTask = webClient.DownloadStringTaskAsync(new Uri(@"http://symbolserver.unity3d.com/000Admin/history.txt"));
            try
            {
                result = await downloadStringTask;
            }
            catch (WebException webException)
            {
                result = webException.Message;
            }
            catch (Exception exception)
            {
                result = exception.Message;
            }
        }
        return result;
    }

    public void CreateToggleEditors(string year)
    {
        List<string> versions = downloadArchiveEditors.Where(e => e.Contains(year)).ToList();
        versions.Reverse();
        List<List<string>> platformNamesList = new List<List<string>>();
        for (int i = 0; i < versions.Count; i++) { platformNamesList.Add(new List<string>()); } //No lo veo tan necesario, podria confirmar si esta vacio en la creacion de toggles???
        editorsCreatorDownloadArchive.CreateToggleEditors(versions, platformNamesList);
    }
}
