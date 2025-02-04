using System;
using UnityEngine;

[Serializable]
public class RoleInfo
{
    public bool camp;
    public string name;
    public void SetRole(bool camp, string name)
    {
        this.camp = camp;
        this.name = name;
    }
}

