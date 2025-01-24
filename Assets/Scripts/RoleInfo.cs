using UnityEngine;

public class RoleInfo :MonoBehaviour
{
    public bool camp;
    public ERole eRole;
    public void SetRole(bool camp, ERole eRole)
    {
        this.camp = camp;
        this.eRole = eRole;
    }
}

