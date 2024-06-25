// MyClass.cs
using System;
using UnityEngine;

namespace LogicJsonClass
{

    // 似非 Object クラス
    [Serializable]
    public class LogicJsonObject
    {
        public int number_of_instances;
        public InstanceInfo[] instances_info;
        public int number_of_events;
        public LogicEvent[] events;

        // 基底コンストラクタ
        public LogicJsonObject()
        {
            this.number_of_instances = 0;
            this.instances_info = new InstanceInfo[0];
            this.number_of_events = 0;
            this.events = new LogicEvent[0];
        }

        // MyObject を文字列として返す
        public string toString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }

    [Serializable]
    public class InstanceInfo
    {
        public int instance_id;
        public string instance_name;//cordinater or router
    }

    [Serializable]
    public class LogicEvent
    {
        public int timer;
        public Instance[] instances;
    }

    [Serializable]
    public class Instance
    {
        public int instance_name;
        public int[] neighbor;
    }
}