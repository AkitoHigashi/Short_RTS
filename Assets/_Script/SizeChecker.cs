using UnityEngine;

public class SizeChecker : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 size = renderer.bounds.size;
            Debug.Log($"オブジェクトのサイズ:{size}");
        }
    }
}
