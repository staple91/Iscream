using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PangGom;
using No;
public class SliderComponent : MonoBehaviour
{
    public Slider backgroundSlider;
    public Slider effectSlider;
    public Slider micSensitiveSlider;

    private void Start()
    {
        backgroundSlider.onValueChanged.AddListener((vol) => { SoundManager.Instance.curBgm.volume = vol; });
        effectSlider.onValueChanged.AddListener((vol) => { SoundManager.Instance.volume = vol; });
    }
}
