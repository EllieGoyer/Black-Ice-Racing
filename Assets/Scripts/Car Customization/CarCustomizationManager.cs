using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CarCustomizationManager : MonoBehaviour
{
    public EventSystem eventSystem;

    [HideInInspector] public CarDataHolder dataHolder;
    public CarController customizerCar;

    GameObject[] buttons = new GameObject[4];

    public CarBody[] carBodies;
    public CarEngine[] carEngines;
    public CarAero[] carAeroes;
    public CarTire[] carTires;

    [Header("UI Elements")]
    public Image bodySprite;
    public Image engineSprite;
    public Image aeroSprite;
    public Image tireSprite;

    public TextMeshProUGUI speedStat;
    public TextMeshProUGUI accelStat;
    public TextMeshProUGUI handlingStat;
    public TextMeshProUGUI brakingStat;
    public TextMeshProUGUI weightStat;

    public float maxSpeedStat;
    public float maxAccelStat;
    public float maxHandlingStat;
    public float maxBrakingStat;

    int currentBody;
    int currentEngine;
    int currentAero;
    int currentTire;

    bool joyStickPushed = false;

    GameObject lastSelected;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            buttons[i] = transform.GetChild(i).gameObject;
        }

        lastSelected = new GameObject();

        dataHolder = FindObjectOfType<CarDataHolder>();
    }

    private void Update()
    {
        DisableMouse();
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetAxis("Vertical") > -0.5 && Input.GetAxis("Vertical") < 0.5)
        {
            joyStickPushed = false;
        }
        if (!joyStickPushed)
        {
            if (eventSystem.currentSelectedGameObject == buttons[0])
            {
                if (Input.GetAxis("Vertical") > 0.5)
                {
                    joyStickPushed = true;
                    currentBody++;
                    if (currentBody >= carBodies.Length)
                    {
                        currentBody = 0;
                    }
                    DisplayParts();
                }
                else if (Input.GetAxis("Vertical") < -0.5)
                {
                    joyStickPushed = true;
                    currentBody--;
                    if (currentBody < 0)
                    {
                        currentBody = carBodies.Length - 1;
                    }
                    DisplayParts();
                }
            }
            else if (eventSystem.currentSelectedGameObject == buttons[1])
            {
                if (Input.GetAxis("Vertical") > 0.5)
                {
                    joyStickPushed = true;
                    currentEngine++;
                    if (currentEngine >= carEngines.Length)
                    {
                        currentEngine = 0;
                    }
                    DisplayParts();
                }
                else if (Input.GetAxis("Vertical") < -0.5)
                {
                    joyStickPushed = true;
                    currentEngine--;
                    if (currentEngine < 0)
                    {
                        currentEngine = carEngines.Length - 1;
                    }
                    DisplayParts();
                }
            }
            else if (eventSystem.currentSelectedGameObject == buttons[2])
            {
                if (Input.GetAxis("Vertical") > 0.5)
                {
                    joyStickPushed = true;
                    currentAero++;
                    if (currentAero >= carAeroes.Length)
                    {
                        currentAero = 0;
                    }
                    DisplayParts();
                }
                else if (Input.GetAxis("Vertical") < -0.5)
                {
                    joyStickPushed = true;
                    currentAero--;
                    if (currentAero < 0)
                    {
                        currentAero = carAeroes.Length - 1;
                    }
                    DisplayParts();
                }
            }
            else if (eventSystem.currentSelectedGameObject == buttons[3])
            {
                if (Input.GetAxis("Vertical") > 0.5)
                {
                    joyStickPushed = true;
                    currentTire++;
                    if (currentTire >= carTires.Length)
                    {
                        currentTire = 0;
                    }
                    DisplayParts();
                }
                else if (Input.GetAxis("Vertical") < -0.5)
                {
                    joyStickPushed = true;
                    currentTire--;
                    if (currentTire < 0)
                    {
                        currentTire = carTires.Length - 1;
                    }
                    DisplayParts();
                }
            }
        }
    }

    void DisplayParts()
    {
        bodySprite.sprite = carBodies[currentBody].sprite;
        engineSprite.sprite = carEngines[currentEngine].sprite;
        aeroSprite.sprite = carAeroes[currentAero].sprite;
        tireSprite.sprite = carTires[currentTire].sprite;

        customizerCar.Clear();
        customizerCar.body = carBodies[currentBody];
        customizerCar.engine = carEngines[currentEngine];
        customizerCar.aero = carAeroes[currentAero];
        customizerCar.tire = carTires[currentTire];
        customizerCar.transform.position += new Vector3(0, 0.2f, 0);
        customizerCar.Setup();

        speedStat.text = customizerCar.maxVelocity.ToString() + "/" + maxSpeedStat;
        accelStat.text = customizerCar.maxMotorTorque.ToString() + "/" + maxAccelStat;
        handlingStat.text = customizerCar.maxSteerAngle.ToString() + "/" + maxHandlingStat;
        brakingStat.text = customizerCar.brakeMod.ToString() + "/" + maxBrakingStat;
        weightStat.text = customizerCar.GetComponent<Rigidbody>().mass + "kg";
    }

    public void SetParts()
    {
        dataHolder.body = carBodies[currentBody];
        dataHolder.engine = carEngines[currentEngine];
        dataHolder.aero = carAeroes[currentAero];
        dataHolder.tire = carTires[currentTire];
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(2);
    }

    void DisableMouse()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(lastSelected);
        }
        else
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }
    }
}
