using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] public float backgroundSize;
    [SerializeField] public float parallaxSpeed;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;

    private float lastCameraX;
    private float lastCameraY;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;

        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
            layers[i].transform.position = new Vector2(layers[i].transform.position.x, lastCameraY);
        }
        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }
    private void Update()
    {
        float deltaY = cameraTransform.position.y - lastCameraY; 
        float deltaX = cameraTransform.position.x - lastCameraX;
        
        transform.position += new Vector3(deltaX * parallaxSpeed, deltaY, 0);

        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;

        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
        {
            ScrollLeft();
        }
        if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
        {
            ScrollRight();
        }
    }

    private void ScrollLeft()
    {
        int lastRight = rightIndex;
        layers[rightIndex].position = new Vector3(layers[leftIndex].position.x - backgroundSize, lastCameraY, 0);
        leftIndex = rightIndex;
        rightIndex--;

        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        int lastLeft = leftIndex;
        layers[leftIndex].position = new Vector3(layers[rightIndex].position.x + backgroundSize, lastCameraY, 0);
        rightIndex = leftIndex;
        leftIndex++;

        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }
}
