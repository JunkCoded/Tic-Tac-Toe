using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private GameObject zeroPrefab;
    [SerializeField] private GameObject winLinePrefab;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private GameObject[] triggers = new GameObject[9];

    private int[] moves = new int[9]; // 0 - Свободно, 1 - Крестик, 2 - Нолик
    private int stepsLeft = 9;
    private bool zeroStep;
    private bool gameEnd = false;
    private int[,] winCombos = { {0, 1, 2, 90}, {3, 4, 5, 90}, {6, 7, 8, 90},
                                    {0, 3, 6, 0}, {1, 4, 7, 0}, {2, 5, 6, 0},
                                    {0, 4, 8, 45}, {2, 4, 6, -45}
                                };

    // Start is called before the first frame update
    void Start() {
        zeroStep = Random.Range(0, 2) == 1;
        statusText.text = (zeroStep ? "O" : "X") + "'s turn";
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void MakeMove(int areaId) {
        if (gameEnd) return;
        if (moves[areaId] != 0) return;

        moves[areaId] = zeroStep ? 2 : 1;
        zeroStep = !zeroStep;
        stepsLeft--;

        CreateSign(zeroStep, areaId);

        if (stepsLeft == 0) {
            statusText.text = "Draw!";
            gameEnd = true;
        } else {
            statusText.text = (zeroStep ? "O" : "X") + "'s turn";
        }

        CheckWin();
    }

    private void CreateSign(bool isZero, int areaId) {
        GameObject prefab = isZero ? crossPrefab : zeroPrefab;
        Vector2 pos = triggers[areaId].transform.position;

        Instantiate(prefab, pos, Quaternion.identity);
    }

    private void CheckWin() {
        for (int i = 0; i < winCombos.GetLength(0); i++) {
            int crossWin = 3;
            int zeroWin = 3;

            for (int j = 0; j < winCombos.GetLength(1) - 1; j++) {
                if (moves[winCombos[i, j]] == 1) crossWin--;
                if (moves[winCombos[i, j]] == 2) zeroWin--;
            }

            if (crossWin == 0) {
                Win(false, i);
                return;
            }

            if (zeroWin == 0) {
                Win(true, i);
                return;
            }
        }
    }

    private void Win(bool isZero, int winComboNum) {
        int zAngle = winCombos[winComboNum, 3];
        Transform centerTrigger = triggers[winCombos[winComboNum, 1]].transform;
    
        Vector3 pos = new Vector3(centerTrigger.position.x, centerTrigger.position.y, -2);
        Vector3 angle = new Vector3(0, 0, zAngle);

        GameObject winLine = Instantiate(winLinePrefab, pos, Quaternion.Euler(angle));

        Vector3 scale = winLine.transform.localScale;
        scale.y += (winComboNum == 6 || winComboNum == 7) ? 1 : 0;
        winLine.transform.localScale = scale;

        statusText.text = (isZero ? "O" : "X") + "'s WIN!";

        gameEnd = true;
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
