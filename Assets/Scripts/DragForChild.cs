using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragForChild : MonoBehaviour
{
    //オブジェクトをクリックしてドラッグ状態にある間呼び出される関数（Unityのマウスイベント）
    void OnMouseDrag()
    {
        //マウスカーソル及びオブジェクトのスクリーン座標を取得
        Vector3 objectScreenPoint = 
            new Vector3(Input.mousePosition.x,Input.mousePosition.y,10);

        //スクリーン座標をワールド座標に変換
        Vector3 objectWorldPoint = Camera.main.ScreenToWorldPoint(objectScreenPoint);

        //オブジェクトの座標を変更する
        GameObject parent = this.transform.parent.gameObject;
        parent.transform.position = objectWorldPoint;
    }
    void Update()
    {
        //オブジェクトの回転をリセット
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
