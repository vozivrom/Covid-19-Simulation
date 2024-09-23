using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Get : MonoBehaviour
{
    public const float DayTime = 2.25f;
    private static Text[] _text;

    private static ushort _healthy = 250;
    private static ushort _notKnow, _know, _dead;
    public static ushort MaxSicked;
    public static ushort VaccineDtime = 2, VaccineDelay = 20;
    private static ushort _day;
    public static bool Quarantine = true, Vaccine = false;
    public static byte ChanceMask = 50;
    public static readonly List<ushort[]> Graph = new List<ushort[]>(20); // day, healthy, sick(don't know), sick(know), dead
    [SerializeField] private Transform peopleField;

    public static ushort Healthy
    {
        get => _healthy;
        set
        {
            _healthy = value;
            if (SetGraph(value, 1)) _text[0].text = value.ToString();
        }
    }

    public static ushort NotKnow
    {
        get => _notKnow;
        set
        {
            _notKnow = value;
            if (SetGraph(value, 2)) _text[1].text = value.ToString();
        }
    }

    public static ushort Know
    {
        get => _know;
        set
        {
            _know = value;
            if (SetGraph(value, 3))
            {
                _text[2].text = value.ToString();
                switch (value)
                {
                    case 0:
                        if (_notKnow == 0)
                        {
                            SceneManager.LoadScene("Graphic");
                        }

                        IsMask = false;
                        break;
                    case 1:
                        IsMask = true;
                        break;
                }
            }
        }
    }

    public static ushort Dead
    {
        get => _dead;
        set
        {
            _dead = value;
            if (SetGraph(value, 4)) _text[3].text = value.ToString();
        }
    }

    public static bool IsMask { get; private set; }

    private void Awake()
    {
        _text = GetComponentsInChildren<Text>().Where(e => e.name == "Counter").ToArray();
        Graph.Add(new[] {_day, _healthy, _notKnow, _know, _dead});
        StartCoroutine(Day());
        if (Vaccine) StartCoroutine(Vaccinate(VaccineDelay));
    }

    public static void Clear()
    {
        Graph.Clear();
        _day = _dead = _know = _notKnow = MaxSicked = 0;
        IsMask = false;
    }

    private static bool SetGraph(ushort val, byte i)
    {
        if (Graph.Count == 0) return false;
        if (Graph.Last()[0] == _day) Graph.Last()[i] = val;
        else Graph.Add(new[] {_day, Healthy, NotKnow, Know, Dead});
        return true;
    }

    private IEnumerator Day()
    {
        yield return new WaitForSeconds(DayTime);
        _day++;
        _text[4].text = _day.ToString();
        StartCoroutine(Day());
    }

    private IEnumerator Vaccinate(float delay = 0)
    {
        yield return new WaitForSeconds(delay * DayTime + DayTime / VaccineDtime);
        GameObject people = peopleField.Find("Healthy")?.gameObject;

        if (people == null)
        {
            if (_know == 0) SceneManager.LoadScene("Graphic");
            yield break;
        }

        people.name = "Immunity";
        foreach (Renderer r in new[]
                 {
                     people.transform.GetChild(0).GetComponent<Renderer>(),
                     people.transform.GetChild(1).GetComponent<Renderer>()
                 })
        {
            r.material.color = Color.cyan;
        }

        StartCoroutine(Vaccinate());
    }
}