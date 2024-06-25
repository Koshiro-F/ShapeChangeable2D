using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordButton : MonoBehaviour
{
    private Button record_button;
    private GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        record_button = GetComponent<Button>();
        record_button.onClick.AddListener(Record);
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Record()
    {
        GM.AddEvent();
    }
}
