using UnityEngine;
using UnityEngine.EventSystems;

public class AudioSelectable : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AudioSource audioSource; 

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.Play();
    }
}
