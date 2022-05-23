using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SwitchMenu(int menu)
    {
        animator.SetInteger("Menu", menu);
    }
}
