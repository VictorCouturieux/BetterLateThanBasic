using TMPro;
using UnityEngine;

public class ShowSwagPointScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreP1;
    [SerializeField] private TMP_Text scoreP2;

    private void Start() {
        scoreP1.text = GameManager.Instance.scorePlayer1.ToString();
        scoreP2.text = GameManager.Instance.scorePlayer2.ToString();
    }
}
