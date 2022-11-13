using UnityEngine;
using UnityEngine.UI;

public class ButtonUninstallEditor : MonoBehaviour
{
    [SerializeField] private ToggleEditor editor;
    [SerializeField] private PopUp popUp;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(
            ()=>{
                popUp.Show($"Do you want to uninstall Unity {editor.Version}?", () => {
                    //
                    //popUp.Close();
                });
            }
        );
    }
}
