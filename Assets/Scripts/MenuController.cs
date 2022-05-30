using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Zamienia menu wyświetlane na ekranie (0 - główne menu, 1 - kalkulator BMI, 2 - dziennik kalorii)
    public void SwitchMenu(int menuId)
    {
        animator.SetInteger("Menu", menuId);
    }
}
