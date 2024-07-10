using UnityEngine;

public class Hospital : MonoBehaviour
{
    private const float _safe = 5;
    private static Vector3 _size, _pos;

    private void Awake()
    {
        Bounds b = GetComponent<MeshRenderer>().bounds;
        _pos = b.min + new Vector3(_safe, 0, _safe);
        _size = b.size - new Vector3(_safe, 0, _safe) * 2;
    }

    public static void ToQuarantine(GameObject obj)
    {
        obj.transform.position = _pos + new Vector3(Random.Range(0, _size.x), 0, Random.Range(0, _size.z));
    }
}