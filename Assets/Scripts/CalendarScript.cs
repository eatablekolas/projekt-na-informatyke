using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScript : MonoBehaviour
{
    [SerializeField] Text DateText;
    [SerializeField] Transform Calendar;
    Text[] CalendarDays;
    DateTime viewedDate;
    float selectTime;
    const float selectInterval = 1f;

    void InitiateCalendarTable()
    {
        CalendarDays = new Text[Calendar.childCount];
        for (int i = 0; i < CalendarDays.Length; i++)
        {
            Transform dayTransform = Calendar.Find("Day" + (i + 1).ToString());
            Transform dayTextTransform = dayTransform.GetChild(0);
            Text dayText = dayTextTransform.GetComponent<Text>();
            CalendarDays[i] = dayText;
        }
    }

    void LoadCalendarDays()
    {
        int daysInMonth = DateTime.DaysInMonth(viewedDate.Year, viewedDate.Month);
        // normalnie niedziela zwraca 0, ta kalkulacja sprawia, że poniedziałek zwraca 0
        int firstDayOfWeek = (int)(new DateTime(viewedDate.Year, viewedDate.Month, 1).DayOfWeek + 6) % 7;
        for (int i = firstDayOfWeek; i < daysInMonth + firstDayOfWeek; i++)
        {
            CalendarDays[i].text = (i + 1 - firstDayOfWeek).ToString();
        }
    }

    void Start()
    {
        viewedDate = DateTime.Now;
        selectTime = Time.time;
        DateText.text = viewedDate.ToString("Y");
        InitiateCalendarTable();
        LoadCalendarDays();
    }

    void ResetCalendar()
    {
        foreach (Text dayText in CalendarDays)
        {
            dayText.text = string.Empty;
        }
    }

    public void SwitchMonth(int delta)
    {
        viewedDate = viewedDate.AddMonths(delta);
        DateText.text = viewedDate.ToString("Y");
        ResetCalendar();
        LoadCalendarDays();
    }

    public void SelectDay(Transform buttonTransform)
    {
        if (Time.fixedTime - selectTime < selectInterval) return;
        selectTime = Time.fixedTime;

        Transform textTransform = buttonTransform.GetChild(0);
        Text textComponent = textTransform.GetComponent<Text>();
        int day = int.Parse(textComponent.text);
        viewedDate = new DateTime(viewedDate.Year, viewedDate.Month, day);
    }
}
