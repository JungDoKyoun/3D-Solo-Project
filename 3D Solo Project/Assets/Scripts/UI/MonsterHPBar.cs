using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;
    private Vector3 offset = Vector3.zero;
    private Transform targetTr;

    public Vector3 Offset { get => offset; set => offset = value; }
    public Transform TargetTr { get => targetTr; set => targetTr = value; }

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        if(screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        rectHp.localPosition = localPos;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    //[SerializeField] private GameObject hpSlider;
    //private Slider slider;
    //private List<Transform> monsterPos;
    //private List<GameObject> hpSliderList;
    //private Camera cam;

    //private void Start()
    //{
    //    cam = Camera.main;
    //    monsterPos = new List<Transform>();
    //    hpSliderList = new List<GameObject>();
    //}

    //private void LateUpdate()
    //{
    //    UpdateHpSliderPos();
    //    KeepSizeConstant();
    //}


    //public void Init()
    //{
    //    GameObject[] monster = GameObject.FindGameObjectsWithTag("Enemy");
    //    for (int i = 0; i < monster.Length; i++)
    //    {
    //        monsterPos.Add(monster[i].transform);
    //        GameObject hpBar = Instantiate(hpSlider, monster[i].transform.position, Quaternion.identity, transform);
    //        hpSliderList.Add(hpBar);
    //        slider = GetComponentInChildren<Slider>();
    //    }
    //}

    //public void UpdateHpSliderPos()
    //{
    //    for(int i = 0; i < monsterPos.Count; i++)
    //    {
    //        hpSliderList[i].transform.position = cam.WorldToScreenPoint(monsterPos[i].position + new Vector3(0, 1.7f, 0));
    //    }
    //}

    //private void KeepSizeConstant()
    //{
    //    for (int i = 0; i < hpSliderList.Count; i++)
    //    {
    //        float distance = Vector3.Distance(cam.transform.position, monsterPos[i].position);
    //        float scaleFactor = Mathf.Clamp(5f / distance, 0.5f, 1.2f);
    //        hpSliderList[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    //    }
    //}

    //public void SetMaxHP(int maxHP)
    //{
    //    slider.maxValue = maxHP;
    //    slider.value = maxHP;
    //}

    //public void UpdateHP(int currentHP)
    //{
    //    slider.value = currentHP;
    //}

    //public void DestroyHPBar()
    //{
    //    Destroy(gameObject);
    //}
}
