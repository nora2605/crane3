using Assets.Source.Scripts.Game;
using UnityEngine;

public class LevelCameraController : MonoBehaviour
{
    private bool moving = false;
    private float currentDegrees;
    private Vector2 mousePos;
    float currentDistance;
    Camera cam;
    private float initcamy;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        currentDistance = 10 * Mathf.Sqrt(Level.dimensions.Item1 * Level.dimensions.Item1 + Level.dimensions.Item2 * Level.dimensions.Item2);
        initcamy = cam.transform.position.y;
        float destinationDegrees = 45;
        cam.transform.position = new Vector3(
            currentDistance * Mathf.Cos(destinationDegrees * Mathf.PI / 180),
            initcamy,
            currentDistance * Mathf.Sin(destinationDegrees * Mathf.PI / 180)
        );
        cam.transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x <= Screen.width / 2 && Input.mousePosition.y >= Screen.height * 0.4)
        {
            moving = true;
            mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 flatCam = cam.transform.position - new Vector3(0, cam.transform.position.y, 0);
            currentDegrees = Mathf.Atan2(flatCam.y, flatCam.x) * 180 / Mathf.PI;
        }
        if (moving && Input.mousePosition.sqrMagnitude != mousePos.sqrMagnitude)
        {
            float destinationDegrees = currentDegrees - (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mousePos).x;
            cam.transform.position = new Vector3(
                currentDistance * Mathf.Cos(destinationDegrees * Mathf.PI / 180), 
                initcamy + (mousePos.y - Input.mousePosition.y), 
                currentDistance * Mathf.Sin(destinationDegrees * Mathf.PI / 180)
            );
            cam.transform.LookAt(Vector3.zero);
        }
        if (Input.GetMouseButtonUp(0))
        {
            moving = false;
        }
    }
}
