using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVitals : MonoBehaviour
{
    public RectTransform healthBar;
    public RectTransform manaBar;
    public RectTransform staminaBar;

    public void SetHealthBar(float sizeNormalized)
    {
        healthBar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetManaBar(float sizeNormalized)
    {
        manaBar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetStaminaBar(float sizeNormalized)
    {
        staminaBar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
