using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectDragTransform : MonoBehaviour
{
    //オブジェクトをクリックしてドラッグ状態にある間呼び出される関数（Unityのマウスイベント）

    [SerializeField]private bool Grid = false;
    private double downTime = 0.0f;

    private Pixel pixel;
    private GameManager GM;
    

    void Start()
    {
        pixel = GetComponent<Pixel>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void OnMouseDrag()
    {
        downTime += Time.deltaTime;
        //マウスカーソル及びオブジェクトのスクリーン座標を取得
        var mousex = (int)Math.Round(Input.mousePosition.x);
        var mousey = (int)Math.Round(Input.mousePosition.y);
        int posx;
        int posy;
        if (Grid)
        {
            posx = (mousex/100)*100;
            posy = (mousey/100)*100;
        }
        else
        {
            posx = mousex;
            posy = mousey;
        }
        Vector3 objectScreenPoint = 
            new Vector3(posx,posy,10);

        //Debug.Log(objectScreenPoint);

        //スクリーン座標をワールド座標に変換
        Vector3 objectWorldPoint = Camera.main.ScreenToWorldPoint(objectScreenPoint);

        //オブジェクトの座標を変更する
        transform.position = objectWorldPoint;
    }
    void OnMouseUp()
    {
        if(downTime<0.1)
        {
            Debug.Log("Click");
            GM.CountCordinaters();
            if(!pixel.isCordinater)
            {
                if(GM.numCordinaters>=GM.maxCordinaters)
                {
                    return;
                }
                GM.numCordinaters++;
                pixel.isCordinater = !pixel.isCordinater;
                pixel.connected = !pixel.connected;
                pixel.change = true;
            }
            else
            {
                GM.numCordinaters--;
                GM.CordinaterOff(pixel.number);
            }
            GM.UpdateInstances();
        }
        downTime = 0.0f;
    }
    
    void Update()
    {
        //オブジェクトの回転をリセット
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
