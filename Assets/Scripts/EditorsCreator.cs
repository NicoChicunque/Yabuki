using System.Collections.Generic;
using UnityEngine;

public class EditorsCreator : MonoBehaviour
{
    [SerializeField] private Transform rowTemplate;
    [SerializeField] private ToggleEditor toggleEditorTemplate;
    [SerializeField] private int rowsChildAllowed = 3;
    Transform rowN;
    private List<ToggleEditor> toggleEditors = new List<ToggleEditor>();
    private List<Transform> rows = new List<Transform>();

    public void CreateToggleEditors(List<string> versions, List<List<string>> platformsNamesList)
    {
        for (int i = 0; i < toggleEditors.Count; i++) { Destroy(toggleEditors[i].gameObject); }
        toggleEditors.Clear();
        for (int i = 0; i < rows.Count; i++) { Destroy(rows[i].gameObject); }
        rows.Clear();
        for (int i = 0; i < versions.Count; i++)
        {
            if (i % rowsChildAllowed == 0)
            {
                rowN = Instantiate(rowTemplate, rowTemplate.parent);
                rowN.gameObject.SetActive(true);
                rows.Add(rowN);
            }

            ToggleEditor toggleEditor = Instantiate(toggleEditorTemplate, rowN);
            toggleEditor.gameObject.SetActive(true);
            toggleEditor.Version = versions[i];
            toggleEditor.PlatformsNames = platformsNamesList[i];
            toggleEditors.Add(toggleEditor);
        }
    }
}
