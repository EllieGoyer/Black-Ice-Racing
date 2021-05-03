using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDataHolder : MonoBehaviour
{
    public static CarDataHolder instance;

    public CarBody body;
    public CarEngine engine;
    public CarAero aero;
    public CarTire tire;

    public GameObject driveTrain;
    public GameObject driveTrainSpinner;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
