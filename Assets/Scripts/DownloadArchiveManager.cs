using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class DownloadArchiveManager : MonoBehaviour
{
    [SerializeField] private PopUp popUp;
    [SerializeField] private ToggleEditor toggleEditorTemplate;
    private List<ToggleEditor> toggleEditors = new List<ToggleEditor>();

    async void Start()
    {        
        var items = await Scan();
        if (items.Equals(string.Empty)) { return; }
        Parse(items);
    }

    public async Task<string> Scan()
    {
        string result = string.Empty;
        // download list of Unity versions
        using (WebClient webClient = new WebClient())
        {
            Task<string> downloadStringTask = webClient.DownloadStringTaskAsync(new Uri(@"http://symbolserver.unity3d.com/000Admin/history.txt"));
            try
            {
                result = await downloadStringTask;
            }
            catch (WebException webException)
            {
                popUp.Show($"WebException {webException.Message}", () => { popUp.Close(); });
            }
            catch (Exception exception)
            {
                popUp.Show($"WebException {exception.Message}", () => { popUp.Close(); });
            }
        }
        return result;
    }

    public void Parse(string items)
    {
        for (int i = 0; i < toggleEditors.Count; i++) { Destroy(toggleEditors[i].gameObject); }
        toggleEditors.Clear();
        string[] receivedList = items.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        if (receivedList == null && receivedList.Length < 1) return;
        Array.Reverse(receivedList);

        for (int i = 0; i < 10/*receivedList.Length*/; i++)
        {
            string[] row = receivedList[i].Split(',');
            ToggleEditor toggleEditor = Instantiate(toggleEditorTemplate, toggleEditorTemplate.transform.parent);
            toggleEditor.gameObject.SetActive(true);
            toggleEditor.Version = row[6].Trim('"');
            toggleEditor.ReleaseDate = DateTime.ParseExact(row[3], "MM/dd/yyyy", CultureInfo.InvariantCulture);            
            toggleEditors.Add(toggleEditor);
        }
    }
}
