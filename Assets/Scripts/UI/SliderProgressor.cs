using UnityEngine;
using UnityEngine.UI;

public class SliderProgressor : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Slider _healthProgress;
    [SerializeField] private Text _healthtext;

    private Vector3 screenPos;

    private void LateUpdate()
    {
        screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        target.transform.position = screenPos;
    }

    public void UpdateBar(float health)
    {
        float result = ((int)(health * 100)) / 100f;
        _healthProgress.value = result;
        _healthtext.text = (result.ToString() + " %");
    }

}

