using UnityEngine;

public class ParallaxController : MonoBehaviour
{
[System.Serializable]
public class ParallaxLayer
{
public Transform layer; // parent object of the layer
[Range(0f, 2f)]
public float movementMultiplier; // how much parallax movement to apply

[HideInInspector] public Vector3 startPos; // stored initial position
}

public ParallaxLayer[] layers;

private Vector3 cameraStartPos;

private void Start()
{
cameraStartPos = transform.position;

// store initial positions of each layer
foreach (var layer in layers)
{
if (layer.layer != null)
layer.startPos = layer.layer.position;
}
}

private void LateUpdate()
{
Vector3 camOffset = transform.position - cameraStartPos;

foreach (var layer in layers)
{
if (layer.layer == null) continue;

Vector3 targetPos = layer.startPos + camOffset * layer.movementMultiplier;
targetPos.z = layer.layer.position.z; // ensure depth does not change
layer.layer.position = targetPos;
}
}
}