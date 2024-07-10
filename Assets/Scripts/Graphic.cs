using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Graphic : MonoBehaviour
{
    [SerializeField] public Toggle healthy;
    [SerializeField] public Toggle notKnow;
    [SerializeField] public Toggle know;
    [SerializeField] public Toggle dead;
    [SerializeField] public Text numX, numY, maxSicked;
    [SerializeField] public Transform axisX;
    [SerializeField] public Transform axisY;
    [SerializeField] public LineRenderer lrd;

    private static readonly (string name, LineRenderer line)[] _def =
    {
        ("Healthy", null),
        ("Not know", null),
        ("Know", null),
        ("Dead", null)
    };

    private readonly Color[][] _color =
    {
        new[] {Color.green}, // Healthy
        new[] {Color.yellow}, // Not know
        new[] {Color.red}, // Know
        new[] {Color.gray} // Dead
    };

    private Vector2 _pilot = new Vector2(5, 25);

    private void Awake()
    {
        maxSicked.text = Get.MaxSicked.ToString();

        var max = new Vector2Int(Get.Graph[Get.Graph.Count - 1][0], Get.Healthy + Get.Dead);
        Vector2 step = GetComponent<RectTransform>().sizeDelta / max;
        float z = transform.position.z;

        if (_pilot.y * 2 >= max.y) _pilot.y = 5;

        for (ushort i = 1; i <= max.x; i++)
        {
            // Days
            Text num = Instantiate(numX, new Vector3(i * step.x, -1, z), Quaternion.identity, axisX);
            num.text = i.ToString();
            if (i % _pilot.x == 0)
                DrawLine(new Vector3[] {new Vector3(i * step.x, 0), new Vector3(i, max.y) * step}, .375f, axisX);
        }

        for (int i = 5; i <= max.y; i += 5)
        {
            // People
            Text num = Instantiate(numY, new Vector3(-1, i * step.y, z), Quaternion.identity, axisY);
            num.text = i.ToString();
            if (i % _pilot.y == 0)
                DrawLine(new Vector3[] {new Vector3(0, i * step.y), new Vector3(max.x, i) * step}, .375f, axisY);
        }

        for (byte i = 0; i < _def.Length; i++)
        {
            var pos = new Vector3[Get.Graph.Count];
            for (ushort j = 0; j < pos.Length; j++)
            {
                pos[j] = new Vector3(Get.Graph[j][0], Get.Graph[j][i + 1]) * step;
            }

            _def[i].line = DrawLine(pos, 1, cl: _color[i], lineName: _def[i].name, visible: i != 3);
        }

        healthy.onValueChanged.AddListener(val => Choose(val, 0, healthy));
        notKnow.onValueChanged.AddListener(val => Choose(val, 1, notKnow));
        know.onValueChanged.AddListener(val => Choose(val, 2, know));
        dead.onValueChanged.AddListener(val => Choose(val, 3, dead));
    }

    private void Choose(bool val, byte i, Component el)
    {
        _def[i].line.enabled = val;
        StartCoroutine(AnimateForward(el.transform.Find("Background")?.GetComponent<Image>()));
    }

    private IEnumerator AnimateForward(Image img)
    {
        yield return new WaitForSeconds(.0001f);
        if (img == null) yield break;
        if (img.gameObject.name != "") img.gameObject.name = "";

        img.fillAmount -= .01f;

        if (img.fillAmount > 0)
        {
            StartCoroutine(AnimateForward(img));
        }
        else
        {
            img.fillClockwise = false;
            StartCoroutine(AnimateBackward(img));
        }
    }

    private IEnumerator AnimateBackward(Image img)
    {
        yield return new WaitForSeconds(.0001f);

        img.fillAmount += .01f;

        if (img.fillAmount < 1)
        {
            StartCoroutine(AnimateBackward(img));
        }
        else
        {
            img.fillClockwise = true;
            img.gameObject.name = "Background";
        }
    }

    private LineRenderer DrawLine(Vector3[] pos, float width = .5f, Transform parent = null, Color[] cl = null,
        string lineName = "Visual_Line", bool visible = true)
    {
        LineRenderer line = Instantiate(lrd, parent ? parent : transform);

        line.enabled = visible;
        line.name = lineName;
        line.startWidth = width;
        line.endWidth = width;

        if (cl != null)
        {
            line.startColor = cl[0];
            line.endColor = cl[0];
        }

        line.positionCount = pos.Length;
        line.SetPositions(pos);
        return line;
    }

    public void Again()
    {
        SceneManager.LoadScene("Choose");
    }

    public void Close()
    {
        Application.Quit();
    }
}