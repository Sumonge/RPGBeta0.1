using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptopn;
    public void ShowStatToolTip(string _text)
    {
        descriptopn.text=_text;

        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        descriptopn.text = "";
        gameObject.SetActive(false);
    }
}
