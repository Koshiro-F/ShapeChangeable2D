using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GridMode : MonoBehaviour {
 
    public ObjectDragTransform objectDragTransform;

    public void Start()
    {
        objectDragTransform = GetComponent<ObjectDragTransform>();
    }
    public void OnClick()
    {
        Debug.Log("押された!");  // ログを出力
    }
}