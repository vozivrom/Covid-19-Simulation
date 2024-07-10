using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject girl, boy;
    [SerializeField] private Transform parent;
    private const float _safe = 5;
    public static Vector3 Size, Pos;

    private GameObject _people;

    private void Awake()
    {
        Bounds b = GetComponent<MeshRenderer>().bounds;
        Pos = b.min + new Vector3(_safe, 0, _safe);
        Size = b.size - new Vector3(_safe, 0, _safe) * 2;
        
        for (ushort i = 0; i < Get.Healthy - 1; i++)
        {
            _people = Random.Range(0, 2) == 1 ? girl : boy;
            Instantiate(_people, Pos + new Vector3(Random.Range(0, Size.x), 0, Random.Range(0, Size.z)),
                Quaternion.identity, parent);
        }

        GameObject infected = Instantiate(_people,
            Pos + new Vector3(Random.Range(0, Size.x), 0, Random.Range(0, Size.z)), Quaternion.identity, parent);
        infected.GetComponent<Virus>().enabled = true;
    }
}