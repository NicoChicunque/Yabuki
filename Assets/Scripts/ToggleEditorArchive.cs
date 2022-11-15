using UnityEngine;
using UnityEngine.UI;

public class ToggleEditorArchive : MonoBehaviour
{
    [SerializeField] private Text textMajorVersion;
    [SerializeField] private EditorsManager editorsManager;

    public string MajorVersion { get => textMajorVersion.text; set => textMajorVersion.text = value; }

    void OnEnable()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(value =>
        {
            if (!value) { return; }
            editorsManager.CreateToggleEditors(MajorVersion);
        });
    }
}
