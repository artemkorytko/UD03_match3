using UnityEngine;

namespace Game
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject gamePanel;
        
        public void ShowMenuPanel()
        {
            gamePanel.gameObject.SetActive(false);
            menuPanel.gameObject.SetActive(true);
        }

        public void ShowGamePanel()
        {
            gamePanel.gameObject.SetActive(true);
            menuPanel.gameObject.SetActive(false);
        }
    }
}