using UnityEngine;

public class Dead : MonoBehaviour
{
    private static GameObject _grave;
    private static Vector3 _pos;
    private static Transform _parent;

    private void Awake()
    {
        _grave = Resources.Load<GameObject>("Prefabs/Grave1");
        _pos = transform.position;
        _parent = transform;
    }

    public static void Die(GameObject obj)
    {
        Destroy(obj);
        Instantiate(_grave, _pos + new Vector3(Random.Range(-25, 25), 0.3f, Random.Range(-25, 25)),
            Quaternion.Euler(-90, 0, Random.Range(0, 360)), _parent);
    }
}