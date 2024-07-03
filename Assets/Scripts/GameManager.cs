using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

using MyJsonControllerGenerics;
using LogicJsonClass;

public class GameManager : MonoBehaviour
{
    public GameObject pixelPrefab;
    private GameObject[] pixels;
    private List<List<int>> connections = new List<List<int>>();
    private float time = 0.0f;

    public int numPixels = 5;//生成するピクセルの数
    public int maxCordinaters = 1;//最大コーディネータ数
    public int numCordinaters = 0;
    public bool isAuto = false;

    private JsonController<LogicJsonObject> JC;
    private LogicJsonObject logicJsonObject;//LogicJsonのオブジェクト
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numPixels; i++)
        {
            Instantiate(pixelPrefab, new Vector3(2*i - 3 - numPixels/2, 0, 0), Quaternion.identity);//ピクセルの生成
        }
        pixels = GameObject.FindGameObjectsWithTag("Pixel");
        int count = 0;
        InstanceInfo[] instinfo = new InstanceInfo[0];
        foreach (GameObject pixel in pixels)
        {
            Pixel pix = pixel.GetComponent<Pixel>();
            pix.number = count;
            if(count == 0)//最初のピクセルをコーディネータにする
            {
                pix.isCordinater = true;
            }
            Array.Resize(ref instinfo, count + 1);
            instinfo[count] = new InstanceInfo();
            instinfo[count].instance_id = count;
            if (pix.isCordinater)
            {
                instinfo[count].instance_name = "cordinater";
            }
            else
            {
                instinfo[count].instance_name = "router";
            }
            count++;
        }
        for (int i=0; i<pixels.Length; i++)
        {
            List<int> temp = new List<int>();
            connections.Add(temp);
        }
        //logicJson出力
        JC = new JsonController<LogicJsonObject>("LogicJson.json");
        JC.InitializeJsonFile();
        logicJsonObject = new LogicJsonObject();
        logicJsonObject.number_of_instances = pixels.Length;
        logicJsonObject.instances_info = instinfo;
        logicJsonObject.number_of_events = 0;
        logicJsonObject.events = new LogicEvent[0];
        JC.UpdateJsonFile(logicJsonObject);
    }
    void Update()
    {
        time += 1000*Time.deltaTime;
    }

    public void CountCordinaters()
    {
        numCordinaters = 0;
        foreach (GameObject pixel in pixels)
        {
            Pixel pix = pixel.GetComponent<Pixel>();
            if (pix.isCordinater)
            {
                numCordinaters++;
            }
        }
    }
    public void CordinaterToRouter(int numc, int numr)//cordinaterとrouterの接続
    {
        Pixel pixc = pixels[numc].GetComponent<Pixel>();
        Pixel pixr = pixels[numr].GetComponent<Pixel>();
        pixr.connected = true;
        // List<int> temp = new List<int>();
        // temp.Add(numc);
        // temp.Add(numr);
        if (!connections[numc].Contains(numr)) connections[numc].Add(numr);
        if (!connections[numr].Contains(numc)) connections[numr].Add(numc);
        foreach (var connection in connections)
        {
            foreach (var num in connection)
            {
                Debug.Log(num);
            }
            Debug.Log(" ");
        }
        if (isAuto)
        {
            AddEvent();
        }
    }

    public void RouterToRouter(int numr1, int numr2)//router間の接続
    {
        Pixel pixr1 = pixels[numr1].GetComponent<Pixel>();
        Pixel pixr2 = pixels[numr2].GetComponent<Pixel>();
        if(pixr1.connected || pixr2.connected)
        {
            pixr1.connected = true;
            pixr2.connected = true;
        }
        if (!connections[numr1].Contains(numr2)) connections[numr1].Add(numr2);
        if (!connections[numr2].Contains(numr1)) connections[numr2].Add(numr1);
        else return;
        foreach (var connection in connections)
        {
            foreach (var num in connection)
            {
                Debug.Log(num);
            }
            Debug.Log(" ");
        }
        if (isAuto)
        {
            AddEvent();
        }
    }

    public void ConnectionOff(int num1, int num2)//接続を切断する
    {
        connections[num1].Remove(num2);
        connections[num2].Remove(num1);
        Pixel pix1 = pixels[num1].GetComponent<Pixel>();
        Pixel pix2 = pixels[num2].GetComponent<Pixel>();
        pix1.connected = false;
        pix2.connected = false;
        if (pix1.isCordinater)
        {
            RouterOff(num2);
        }
        if (pix2.isCordinater)
        {
            RouterOff(num1);
        }
        if(isAuto) AddEvent();
    }

    public void CordinaterOff(int numc)//コーディネータを切断する（クリックに起因）
    {
        Pixel pixc = pixels[numc].GetComponent<Pixel>();
        pixc.DisableCollider();
        pixc.isCordinater = false;
        pixc.connected = false;
        Debug.Log("Count");
        Debug.Log(numc);
        Debug.Log(connections[numc].Count);
        var count = connections[numc].Count;
        for (int i = 0; i < count; i++)
        {
            RouterOff(connections[numc][0]);
            //connections[numc].RemoveAt(0);
            Debug.Log("RouterOff");
        }
        pixc.EnableCollider();
        foreach (var connection in connections)
        {
            foreach (var num in connection)
            {
                Debug.Log(num);
            }
            Debug.Log(" ");
        }
    }

    private void RouterOff(int numr)//routerを切断する
    {
        Pixel pixr = pixels[numr].GetComponent<Pixel>();
        pixr.connected = false;
        var count = connections[numr].Count;
        if (connections[numr].Count == 0)
        {
            return;
        }
        for (int j = 0; j < count; j++)
        {
            var numr2 = connections[numr][0];
            connections[numr].RemoveAt(0);
            connections[numr2].Remove(numr);
            RouterOff(numr2);
        }
    }

    public void AddEvent()//JSONにイベントを追加
    {
        logicJsonObject.number_of_events++;
        Array.Resize(ref logicJsonObject.events, logicJsonObject.number_of_events);
        logicJsonObject.events[logicJsonObject.number_of_events - 1] = new LogicEvent();
        logicJsonObject.events[logicJsonObject.number_of_events - 1].timer = (int)time;
        logicJsonObject.events[logicJsonObject.number_of_events - 1].instances = new Instance[0];
        for (int i = 0; i < pixels.Length; i++)
        {
            Array.Resize(ref logicJsonObject.events[logicJsonObject.number_of_events - 1].instances, i + 1);
            logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i] = new Instance();
            logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].instance_name = i;
            logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].neighbor = new int[0];
            foreach (var connection in connections[i])
            {
                Array.Resize(ref logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].neighbor, logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].neighbor.Length + 1);
                logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].neighbor[logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].neighbor.Length - 1] = connection;
            }
        }
        JC.UpdateJsonFile(logicJsonObject);
    }

    public void Quit()//終了時にゼロ行列を追加する。。。はずだったが、終了時にcolliderが消えるせいでConnectionOffが呼ばれるので必要ないかも
    {
        logicJsonObject.number_of_events++;
        Array.Resize(ref logicJsonObject.events, logicJsonObject.number_of_events);
        logicJsonObject.events[logicJsonObject.number_of_events - 1] = new LogicEvent();
        logicJsonObject.events[logicJsonObject.number_of_events - 1].timer = (int)time;
        logicJsonObject.events[logicJsonObject.number_of_events - 1].instances = new Instance[0];
        for (int i = 0; i < pixels.Length; i++)
        {
            Array.Resize(ref logicJsonObject.events[logicJsonObject.number_of_events - 1].instances, i + 1);
            logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i] = new Instance();
            logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].instance_name = i;
            logicJsonObject.events[logicJsonObject.number_of_events - 1].instances[i].neighbor = new int[0];
        }
        JC.UpdateJsonFile(logicJsonObject);
    }

    public void UpdateInstances()
    {
        logicJsonObject.instances_info = new InstanceInfo[0];
        for (int i = 0; i < pixels.Length; i++)
        {
            Array.Resize(ref logicJsonObject.instances_info, i + 1);
            logicJsonObject.instances_info[i] = new InstanceInfo();
            logicJsonObject.instances_info[i].instance_id = i;
            Pixel pix = pixels[i].GetComponent<Pixel>();
            if (pix.isCordinater)
            {
                logicJsonObject.instances_info[i].instance_name = "cordinater";
            }
            else
            {
                logicJsonObject.instances_info[i].instance_name = "router";
            }
        }
        JC.UpdateJsonFile(logicJsonObject);
    }
}
