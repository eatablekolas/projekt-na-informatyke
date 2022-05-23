using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BMICalculator : MonoBehaviour
{
    [SerializeField] InputField HeightInputField;
    [SerializeField] InputField WeightInputField;
    [SerializeField] Text ResultText;
    float height = 0;
    float weight = 0;

    // Sprawdza, czy wpisana wartość nie zawiera znaków niedozwolonych ('-' i '0' na początku, oraz "." w środku)
    bool CheckForBannedMarks(string valueString, InputField affectedInputField)
    {
        if (valueString.Length < 1) return false;

        if (valueString.Substring(0, 1) == "-" || valueString.Substring(0, 1) == "0")
        {
            affectedInputField.text = valueString.Substring(1, valueString.Length - 1);
            return true;
        }
        else if (valueString.Contains("."))
        {
            affectedInputField.text = valueString.Remove(valueString.Length - 1);
            return true;
        }
        
        return false;
    }

    // Pokazuje aktualną wartość BMI, lub informuje o braku wymaganych informacji
    void UpdateBMI()
    {
        if (height != 0 && weight != 0)
        {
            float BMI = weight / (height * height);
            ResultText.text = BMI.ToString("F2");
        }
        else
        {
            ResultText.text = "-";
        }
    }

    // Funkcja wywoływana przez zmianę w polu tekstowym wysokości
    public void OnHeightChanged(string heightString)
    {
        if (CheckForBannedMarks(heightString, HeightInputField)) return;

        height = heightString.Length >= 1 ? float.Parse(heightString) / 100f : 0; // dzielone przez 100 by zamienić z cm na m
        UpdateBMI();
    }

    // Funkcja wywoływana przez zmianę w polu tekstowym masy
    public void OnWeightChanged(string weightString)
    {
        if (CheckForBannedMarks(weightString, WeightInputField)) return;

        weight = weightString.Length >= 1 ? float.Parse(weightString) : 0;
        UpdateBMI();
    }
}
