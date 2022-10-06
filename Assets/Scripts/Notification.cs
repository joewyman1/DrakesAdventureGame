using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Notifications
{
    public class Notification : ScriptableObject
    {
        public String Name { get; set; }
        public System.Object Object { get; set; }
        public Dictionary<String, System.Object> userInfo { get; set; }
        public Notification() : this("NotificationName")
        {
        }

        public Notification(String name) : this(name, null)
        {
        }

        public Notification(String name, System.Object obj) : this(name, obj, null)
        {
        }

        public Notification(String name, System.Object obj, Dictionary<String, System.Object> userInfo)
        {
            this.Name = name;
            this.Object = obj;
            this.userInfo = userInfo;
        }
    }
}
