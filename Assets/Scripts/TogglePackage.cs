using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePackage : MonoBehaviour
{
    [SerializeField] private Text textName;

    public string Name { get => textName.text; set => textName.text = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
