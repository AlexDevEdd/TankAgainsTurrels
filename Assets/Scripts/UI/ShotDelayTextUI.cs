using UnityEngine;
using UnityEngine.UI;

public class ShotDelayTextUI : MonoBehaviour
{
    [SerializeField] private Text text;
    public Text Text => text;     
    public void UpdateTextDelay(float shotDelay)
    {
        float result = ((int)(shotDelay * 100)) / 100f;
        text.text = result.ToString();
    }
}

