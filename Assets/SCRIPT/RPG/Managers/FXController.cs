using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FXController : MonoBehaviour, IFXController
{
    [SerializeField] TextMeshPro DamageFXText;
    [SerializeField] GameObject SlashFX;
    public void DisplayDamage(int pAmount, Vector3 pPosition)
    {
        TextMeshPro DamageText = Instantiate<TextMeshPro>(DamageFXText, pPosition, transform.rotation);
        DamageText.text = $"-{pAmount}";
    }

    public void AddSlash(Vector3 pPosition)
    {
        Instantiate(SlashFX, pPosition, transform.rotation);
    }
}
