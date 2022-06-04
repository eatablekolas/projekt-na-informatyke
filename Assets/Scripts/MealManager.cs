using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MealManager : MonoBehaviour
{
    // Zewnêtrzne skrypty
    [SerializeField] DatabaseManager database;
    [SerializeField] CalendarScript calendar;
    // Pokazywanie posi³ków
    [SerializeField] Text CalorieSumText;
    [SerializeField] Transform ListsTransform;
    bool isFirstLoad = true;
    Dictionary<string, string> MealDictionary = new Dictionary<string, string>();
    List<string>[] MealLists = new List<string>[5];
    // Dodawanie posi³ków
    [SerializeField] GameObject MealAddingMenu;
    [SerializeField] RectTransform ScrollViewContent;
    [SerializeField] RectTransform ExampleButton;
    [SerializeField] float ButtonHeight;
    int editedMealTime;

    void LoadMealPickingMenu(string search = "")
    {
        string TextFileDirectory = Application.streamingAssetsPath + "/lista_kalorii.txt";
#if UNITY_EDITOR
        IEnumerable<string> lines = File.ReadLines(TextFileDirectory);
#else
        WWW reader = new WWW(TextFileDirectory);
        while (!reader.isDone) { }
        string[] lines = reader.text.Split('\n');
#endif

        int i = 0;
        foreach (string line in lines)
        {
            string[] mealData = line.Split(',');
            string name = mealData[0];
            string calories = mealData[1];

            calories = calories.Replace(" ", string.Empty);
            calories = calories.Replace("kcal", string.Empty);

            if (isFirstLoad)
            {
                MealDictionary[name] = calories;
            }

            if (!name.Contains(search)) continue;

            RectTransform newButton = Instantiate(ExampleButton, ScrollViewContent);
            newButton.anchoredPosition = ExampleButton.anchoredPosition + (new Vector2(0, -120) * i);
            newButton.Find("Name").GetComponent<Text>().text = name;
            newButton.Find("Calories").GetComponent<Text>().text = calories;
            newButton.name = $"Meal{i}";
            newButton.gameObject.SetActive(true);

            i++;
        }

        if (isFirstLoad) isFirstLoad = false;
        ScrollViewContent.sizeDelta = new Vector2(0, ButtonHeight) * i;
    }

    void Start()
    {
        LoadMealPickingMenu();
        for (int i = 0; i < 5; i++)
        {
            MealLists[i] = new List<string>();
        }
    }

    public void SwitchMealAdding(Transform buttonTransform)
    {
        MealAddingMenu.SetActive(buttonTransform.name == "Add");

        if (buttonTransform.name == "Add")
        {
            editedMealTime = int.Parse(buttonTransform.parent.name.Replace("List", ""));
        }
    }

    void ReloadCalorieSum()
    {
        int calorieSum = 0;

        foreach (Transform list in ListsTransform)
        {
            calorieSum += int.Parse(list.Find("Sum").GetComponent<Text>().text);
        }

        CalorieSumText.text = calorieSum.ToString();
    }

    public void ReloadMealLists(Dictionary<int, List<string>> MealData)
    {
        for (int mealTime = 1; mealTime <= 5; mealTime++)
        {
            MealLists[mealTime - 1] = MealData[mealTime - 1];
            ReloadMealList(mealTime);
        }
    }

    void ReloadMealList(int index)
    {
        string[] meals = { "", "", "" };
        string[] calories = { "", "", "" };
        int calorieSum = 0;

        int i = 0;
        foreach (string meal in MealLists[index - 1])
        {
            meals[i] = meal;
            calories[i] = MealDictionary[meal];
            calorieSum += int.Parse(MealDictionary[meal]);

            i++;
        }

        string mealStr = $"- {meals[0]}\n- {meals[1]}\n- {meals[2]}";
        string calorieStr = $"- {calories[0]}\n- {calories[1]}\n- {calories[2]}";

        Transform listTransformToReload = ListsTransform.Find("List" + index);
        listTransformToReload.GetComponent<Text>().text = mealStr;
        listTransformToReload.Find("Calories").GetComponent<Text>().text = calorieStr;
        listTransformToReload.Find("Sum").GetComponent<Text>().text = calorieSum.ToString();

        ReloadCalorieSum();
    }

    public void PickMeal(Transform buttonTransform)
    {
        if (MealLists[editedMealTime - 1].Count >= 3) return;

        string name = buttonTransform.Find("Name").GetComponent<Text>().text;

        if (MealLists[editedMealTime - 1].Contains(name)) return;

        database.AddMeal(name, editedMealTime - 1, calendar.viewedDate);

        MealLists[editedMealTime - 1].Add(name);
        ReloadMealList(editedMealTime);

        MealAddingMenu.SetActive(false);
    }

    public void RemoveMeal(Transform buttonTransform)
    {
        int mealTime = int.Parse(buttonTransform.parent.name.Replace("List", ""));

        if (MealLists[mealTime - 1].Count < 1) return;

        int lastMealIndex = MealLists[mealTime - 1].Count - 1;
        string mealName = MealLists[mealTime - 1][lastMealIndex];
        database.RemoveMeal(mealName, mealTime - 1, calendar.viewedDate);

        MealLists[mealTime - 1].RemoveAt(lastMealIndex);
        ReloadMealList(mealTime);
    }
}
