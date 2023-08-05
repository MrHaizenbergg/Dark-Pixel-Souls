using UnityEngine;

public class BloodDestroyer : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 3f);
    }
}