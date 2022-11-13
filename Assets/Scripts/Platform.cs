using UnityEngine;
using UnityEngine.UI;

public class Platform : Module
{
    [SerializeField] private Toggle toggleCreate;
    public bool IsCheckedToUse { get => toggleCreate.isOn;  set =>  toggleCreate.isOn = value; }

    private void OnEnable()
    {
        toggleCreate.onValueChanged.AddListener(
            value => {
                //if (!IsDownloaded)
                //{
                //    IsCheckedToDownload = true;
                //}
                if (IsDownloaded) { return; }
                IsCheckedToDownload = value;
                toggleDownload.interactable = ! value;
            }
        );
    }
}
