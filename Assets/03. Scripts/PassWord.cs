using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PassWord : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onAccessGranted;
    [SerializeField] private UnityEvent onAccessDenied;
    [Header("Combination Code (9 Numbers Max)")]
    [SerializeField] private int keypadCombo = 12345;
    [Header("Settings")]
    [SerializeField] private string accessGrantedText = "Granted";
    [SerializeField] private string accessDeniedText = "Denied";
    [Header("Colors")]
    [SerializeField] private Color screenNormalColor = new(0.98f, 0.50f, 0.032f, 1f); //orangy
    [SerializeField] private Color screenDeniedColor = new(1f, 0f, 0f, 1f); //red
    [SerializeField] private Color screenGrantedColor = new(0f, 0.62f, 0.07f); //greenish
    [SerializeField] private TMP_Text keypadDisplayText;
    private string currentInput;
    private bool displayingResult = false;
    private bool accessWasGranted = false;
    [SerializeField] private float displayResultTime = 1f;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private float screenIntensity = 2.5f;

    private void Awake()
    {
        ClearInput();
        backgroundImage.color = screenNormalColor;
    }
    
    public void AddInput(string input)
    { 
        if (displayingResult || accessWasGranted) return;
        switch (input)
        {
            case "enter":
                CheckCombo();
                break;
            default:
                if (currentInput != null && currentInput.Length == 9) // 9 max passcode size 
                {
                    return;
                }
                currentInput += input;
                keypadDisplayText.text = currentInput;
                break;
        }
    }
    public void CheckCombo()
    {
        if (int.TryParse(currentInput, out var currentKombo))
        {
            bool granted = currentKombo == keypadCombo;
            if (!displayingResult)
            {
                StartCoroutine(DisplayResultRoutine(granted));
            }
        }
        else
        {
            Debug.LogWarning("Couldn't process input for some reason..");
        }

    }
    private IEnumerator DisplayResultRoutine(bool granted)
    {
        displayingResult = true;

        if (granted) AccessGranted();
        else AccessDenied();

        yield return new WaitForSeconds(displayResultTime);
        displayingResult = false;
        if (granted) yield break;
        ClearInput();
        backgroundImage.color = screenNormalColor;

    }
    private void AccessDenied()
    {
        keypadDisplayText.text = accessDeniedText;
        onAccessDenied?.Invoke();
        backgroundImage.color = screenDeniedColor;
        
    }

    private void ClearInput()
    {
        currentInput = "";
        keypadDisplayText.text = currentInput;
    }

    private void AccessGranted()
    {
        accessWasGranted = true;
        keypadDisplayText.text = accessGrantedText;
        onAccessGranted?.Invoke();
        backgroundImage.color = screenGrantedColor;      
    }
}
