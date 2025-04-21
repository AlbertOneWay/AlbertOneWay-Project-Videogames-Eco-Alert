using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{
    public Slider contaminationSlider;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        float currentContamination = GameManager.Instance.GetContamination();

        if (contaminationSlider != null)
            contaminationSlider.value = currentContamination;
        
    }
}