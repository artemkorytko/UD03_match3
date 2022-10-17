using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game
{
   public class ScoreCounter : MonoBehaviour
   {
      [SerializeField] private TextMeshProUGUI textMesh;
      [SerializeField] private float duration = 0.5f;

      private int _score;
   
      public void Initialize(int value)
      {
         _score = value;
         textMesh.text = _score.ToString();
      }
   
      public void AddScore(int value)
      {
         DOTween.To(val => { textMesh.text = ((int)val).ToString(); }, _score, _score += value, duration);
      }
   }
}
