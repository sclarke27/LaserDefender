using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionsPanel : MonoBehaviour
{

    public Text tapToStartText;

    // Use this for initialization
    void Start()
    {
        if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) && tapToStartText != null)
        {
            tapToStartText.text = "Tap here to start";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
