using UnityEngine;
using UnityEngine.UI;

public class Module : MonoBehaviour
{
    [SerializeField] private Text textName;
    public Toggle toggleDownload;

    public string Name { get => textName.text; set => textName.text = value; }
    public bool IsDownloaded { get => ! toggleDownload.gameObject.activeSelf;  
        set { 
            toggleDownload.gameObject.SetActive(! value); 
            toggleDownload.interactable = ! value;
            //IsCheckedToDownload = value;
        } 
    }
    public bool IsCheckedToDownload { get => toggleDownload.isOn; set => toggleDownload.isOn = value; }
}
