using UnityEngine;
using UnityEngine.UI;

public class ToggleNewProject : MonoBehaviour {
    [SerializeField] private GameObject newProjectPanel;
    void Start(){
        GetComponent<Toggle>().onValueChanged.AddListener(
            value => {
                if ( ! value) { return; }
                newProjectPanel.SetActive(true);
            }
        );
    }
}
