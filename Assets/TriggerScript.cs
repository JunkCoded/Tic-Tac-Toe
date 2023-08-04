using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private GameController controller;

    // Start is called before the first frame update
    void Start() {
        controller = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    private void OnMouseDown() {
        controller.MakeMove(int.Parse(gameObject.name));
    }
}
