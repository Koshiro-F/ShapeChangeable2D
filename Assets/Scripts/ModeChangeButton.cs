using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeChangeButton : MonoBehaviour
{
    private Button change_button;

    [SerializeField] private Button record_button;

    private GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        change_button = GetComponent<Button>();
        change_button.onClick.AddListener(ChangeMode);
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(GM.isAuto)
        {
            record_button.interactable = false;
        }
        else
        {
            record_button.interactable = true;
        }
    }

    public void ChangeMode()
    {
        if (GM.isAuto)
        {
            GM.isAuto = false;
            record_button.interactable = true;
        }
        else
        {
            GM.isAuto = true;
            record_button.interactable = false;
        }
    }
}
