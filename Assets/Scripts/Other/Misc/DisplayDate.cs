using UnityEngine;
using System;
using UnityEngine.UI;

public class DisplayDate : MonoBehaviour
{
    private DateTime _date;
    public Text textDate;

    private void Start()
    {
        _date = DateTime.Now;
        textDate.text = _date.ToString("dd/MM/yyyy");
    }
}

