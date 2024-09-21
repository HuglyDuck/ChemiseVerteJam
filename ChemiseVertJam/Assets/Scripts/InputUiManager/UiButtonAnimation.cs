using UnityEngine;

public class UiButtonManager : MonoBehaviour
{
    public JoystickInput inputManager;

    [SerializeField] private GameObject buttonA;
    [SerializeField] private GameObject buttonB;
    [SerializeField] private GameObject buttonX;


    [SerializeField] private float moveDistance = 0.1f;


    private Vector3 originalPositionA;
    private Vector3 originalPositionB;
    private Vector3 originalPositionX;

    private void Start()
    {

        originalPositionA = buttonA.transform.localPosition;
        originalPositionB = buttonB.transform.localPosition;
        originalPositionX = buttonX.transform.localPosition;
    }

    private void Update()
    {

        if (inputManager._aPushed > 0f)
        {
            PressButton(buttonA, originalPositionA);
        }
        else
        {
            ResetButton(buttonA, originalPositionA);
        }

        if (inputManager._bPushed > 0f)
        {
            PressButton(buttonB, originalPositionB);
        }
        else
        {
            ResetButton(buttonB, originalPositionB);
        }

        if (inputManager._xPushed > 0f)
        {
            PressButton(buttonX, originalPositionX);
        }
        else
        {
            ResetButton(buttonX, originalPositionX);
        }
    }

    private void PressButton(GameObject button, Vector3 originalPosition)
    {
        Vector3 newPosition = originalPosition;
        newPosition.y -= moveDistance;
        button.transform.localPosition = newPosition;
    }

    private void ResetButton(GameObject button, Vector3 originalPosition)
    {
        button.transform.localPosition = originalPosition;
    }
}
