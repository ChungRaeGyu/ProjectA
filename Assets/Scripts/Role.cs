using System.Collections.Generic;
using UnityEngine;

public static class Role
{
    public static List<RoleInfo> roleList = new List<RoleInfo>();

    public static void SuffleRole()
    {
        for(int i=0; i<roleList.Count; i++) {
            int rand = Random.Range(0,roleList.Count);
            RoleInfo tempInfo = roleList[rand];
            roleList[rand] = roleList[i];
            roleList[i] = tempInfo;
        }
    }
}

