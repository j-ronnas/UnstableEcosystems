using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [SerializeField]
    Text speciesTypeText;

    [SerializeField]
    Slider sizeSlider;

    [SerializeField]
    Slider speedSlider;

    [SerializeField]
    Slider survivabilitySlider;

    [SerializeField]
    Slider reproductionSlider;

    [SerializeField]
    Image colorImage;

    public void Start()
    {
        return;
        speciesTypeText = GetComponentsInChildren<Text>()[0];
        colorImage = GetComponentsInChildren<Image>()[1];

        Slider[] sliders = GetComponentsInChildren<Slider>();
        sizeSlider = sliders[0];
        speedSlider = sliders[1];
        survivabilitySlider = sliders[2];
        reproductionSlider = sliders[3];

        

    }
    public void SetupOptionPanel(SpeciesInfo info)
    {
        Debug.Log(info);
        Debug.Log(speciesTypeText);

        speciesTypeText.text = info.speciesType.ToString();
        colorImage.color = info.color;
        sizeSlider.value = info.size;
        speedSlider.value = info.speed;
        survivabilitySlider.value = info.survivabilityRating;
        reproductionSlider.value = info.reproduceRating;
    }
}
