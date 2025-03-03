using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class So : MonoBehaviour
{
    private static So Sos;
    [SerializeField] MonsterDataSO m;
    [SerializeField] WeaponItemData a;
    [SerializeField] WeaponItemData b;
    [SerializeField] WeaponItemData c;
    [SerializeField] WeaponItemData d;
    [SerializeField] WeaponItemData e;
    [SerializeField] WeaponItemData f;
    [SerializeField] ArmorTopItemData g;
    [SerializeField] ArmorBottomItemData h;
    [SerializeField] PortionItemData i;

    private void Awake()
    {
        if (Sos == null)
        {
            Sos = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static So Instance
    {
        get
        {
            if (Sos == null)
            {
                return null;
            }
            return Sos;
        }
    }
}
