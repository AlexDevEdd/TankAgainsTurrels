using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private Button _button;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _button.onClick.AddListener(Restart);
        _playerHealth.OnLose += ShowLosePanel;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void ShowLosePanel() =>
        _losePanel.SetActive(true);
      
    private void OnDisable() =>
        _button.onClick.RemoveListener(Restart);
}

