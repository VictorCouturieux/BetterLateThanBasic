using UnityEngine;

public class InstanceCarScoreMenu : MonoBehaviour
{
    [SerializeField] private int playerId = 1;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("CarPlayers : " + GameManager.Instance.carPlayer1 + " " + GameManager.Instance.carPlayer2);
        if (playerId == 1 && GameManager.Instance.carPlayer1 != null) {
            GameObject newCar = Instantiate(GameManager.Instance.carPlayer1, transform);
            newCar.transform.parent = transform;
            newCar.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newCar.transform.localPosition = Vector3.zero;
        } else if (playerId == 2 && GameManager.Instance.carPlayer2 != null ) {
            GameObject newCar = Instantiate(GameManager.Instance.carPlayer2, transform);
            newCar.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newCar.transform.parent = transform;
            newCar.transform.localPosition = Vector3.zero;
        }
    }

}
