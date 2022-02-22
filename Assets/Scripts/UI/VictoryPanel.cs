using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private Button _button;

    private Player _player;
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _button.onClick.AddListener(Restart);
        _player.OnVictory += ShowVictoryPanel;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void ShowVictoryPanel()
    {
        _victoryPanel.SetActive(true);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ShowVictoryPanel);
    }
}

