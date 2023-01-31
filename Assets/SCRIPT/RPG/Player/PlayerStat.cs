using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public delegate void UpdateLife(int pLife, int pLifeMax);
    public static event UpdateLife onUpdateLife;

    [SerializeField] int PV;
    [SerializeField] int PV_Max;

    public void GetDamage(uint pAmount)
    {
        GameManager.FX.DisplayDamage((int)pAmount, Camera.main.transform.position + new Vector3(10f, 0f, 4f));

        PV = Mathf.Max(PV - (int)pAmount, 0);
        onUpdateLife?.Invoke(PV, PV_Max);

        if (PV <= 0)
        {
            OnDead();
        }

    }

    void OnDead()
    {
        Debug.Log("You're dead :(");
    }
}
