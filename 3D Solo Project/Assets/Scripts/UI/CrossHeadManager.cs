using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHeadManager : MonoBehaviour
{
    public void OnCrossHead()
    {
        gameObject.SetActive(true);
    }

    public void OffCrossHead()
    {
        gameObject.SetActive(false);
    }
}
