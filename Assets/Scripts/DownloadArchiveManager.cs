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
    [SerializeField] private int rowsChildAllowed = 3;
    [SerializeField] private Transform rowTemplate, rowN;
    private List<Transform> rows = new List<Transform>();

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
            if (i % rowsChildAllowed == 0)
            {
                rowN = Instantiate(rowTemplate, rowTemplate.parent);
                rowN.gameObject.SetActive(true);
                rows.Add(rowN);
            }            

            string[] row = receivedList[i].Split(',');
            ToggleEditor toggleEditor = Instantiate(toggleEditorTemplate, rowN);
            toggleEditor.gameObject.SetActive(true);
            toggleEditor.Version = row[6].Trim('"');
            toggleEditor.ReleaseDate = DateTime.ParseExact(row[3], "MM/dd/yyyy", CultureInfo.InvariantCulture);            
            toggleEditors.Add(toggleEditor);
            //toggleEditor.transform.parent = rowN;
        }
    }
}
