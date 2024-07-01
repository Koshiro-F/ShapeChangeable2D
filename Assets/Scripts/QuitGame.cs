using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    private Button quit_button;
    private GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        quit_button = GetComponent<Button>();
        quit_button.onClick.AddListener(Quit);
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Quit()
    {
        Debug.Log("Quit");
        //GM.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
