using UnityEngine;

public class InstanceCarScoreMenu : MonoBehaviour
{
    
    [SerializeField] private int playerId = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("CarPlayers : " + GameManager.Instance.carPlayer1 + " " + GameManager.Instance.carPlayer2);
        
        if (playerId == 1 && GameManager.Instance.carPlayer1 != null) {
            Instantiate(GameManager.Instance.carPlayer1, transform);
        } else if (playerId == 2 && GameManager.Instance.carPlayer2 != null ) {
            Instantiate(GameManager.Instance.carPlayer2, transform);
        }
    }

}
