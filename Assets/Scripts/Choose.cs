using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Choose : MonoBehaviour
{
    [SerializeField] public Slider peopleSlider;
    [SerializeField] public Slider masksSlider;
    [SerializeField] public Slider startDaySlider;
    [SerializeField] public Slider vaccinePerDaySlider;
    [SerializeField] public Text peopleCounter;
    [SerializeField] public Text masksCounter;
    [SerializeField] public Text startDayCounter;
    [SerializeField] public Text vaccineInDayCounter;
    [SerializeField] public Toggle isQuarantine;
    [SerializeField] public Toggle isVaccine;
    [SerializeField] public GameObject startDay;
    [SerializeField] public GameObject vaccinePerDay;

    private void Awake()
    {
        isVaccine.onValueChanged.AddListener(evt =>
        {
            if (evt)
            {
                transform.localPosition += Vector3.down * 75;
                startDay.transform.localPosition += Vector3.forward * 1000;
                vaccinePerDay.transform.localPosition += Vector3.forward * 1000;
            }
            else
            {
                transform.localPosition += Vector3.up * 75;
                startDay.transform.localPosition += Vector3.back * 1000;
                vaccinePerDay.transform.localPosition += Vector3.back * 1000;
            }
        });

        peopleSlider.onValueChanged.AddListener(val => peopleCounter.text = val.ToString());
        masksSlider.onValueChanged.AddListener(val => masksCounter.text = val + "%");
        startDaySlider.onValueChanged.AddListener(val => startDayCounter.text = val.ToString("0.##"));
        vaccinePerDaySlider.onValueChanged.AddListener(val => vaccineInDayCounter.text = val.ToString("0.##"));
    }

    private void Start()
    {
        isVaccine.isOn = Get.Vaccine;
        vaccinePerDaySlider.value = Get.VaccineDtime;
        startDaySlider.value = Get.VaccineDelay;
        peopleSlider.value = Get.Healthy + Get.Dead;
        isQuarantine.isOn = Get.Quarantine;
        masksSlider.value = Get.ChanceMask;
    }

    public void StartButton()
    {
        Get.Clear();
        Get.Vaccine = isVaccine.isOn;
        Get.VaccineDtime = (ushort)vaccinePerDaySlider.value;
        Get.VaccineDelay = (ushort)startDaySlider.value;
        Get.Healthy = (ushort)peopleSlider.value;
        Get.Quarantine = isQuarantine.isOn;
        Get.ChanceMask = (byte)masksSlider.value;
        SceneManager.LoadScene("Simulation");
    }
}