using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Text textMessage;
    [SerializeField] private Button buttonContinue, buttonClose;

    public string Message { get => textMessage.text; set => textMessage.text = value; }

    private void Start() { 
        buttonClose.onClick.AddListener(Close);
    }

    public void Show(string message, UnityAction onPressOK)
    {
        Message = message;
        buttonContinue.onClick.AddListener(Processing);
        buttonContinue.onClick.AddListener(onPressOK);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        buttonContinue.interactable = buttonClose.interactable = true;
        buttonContinue.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    void Processing()
    {
        buttonContinue.interactable = buttonClose.interactable = false;
        Message += "\n\nProcessing, please wait ...";
    }
}