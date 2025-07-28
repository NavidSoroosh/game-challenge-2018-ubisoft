using UnityEngine;
using System.Collections;

public static class GameTime
{
    public static int Hours;
    public static int Minutes;
}
public class AutoIntensity : MonoBehaviour
{

    public Gradient nightDayColor;

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;


    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public float dayAtmosphereThickness = 0.4f;
    public float nightAtmosphereThickness = 0.87f;

    public int StartHour = 0;

    float skySpeed = 1;


    Light mainLight;
    Skybox sky;
    Material skyMat;
    public float timeMultiplier;

    private float lastTime;


    void Start()
    {

        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;
        lastTime = (StartHour * 15);
    }

    void Update()
    {

        float tRange = 1 - minPoint;
        float dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
        float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        mainLight.intensity = i;

        tRange = 1 - minAmbientPoint;
        dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        RenderSettings.ambientIntensity = i;

        mainLight.color = nightDayColor.Evaluate(dot);
        RenderSettings.ambientLight = mainLight.color;

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat("_AtmosphereThickness", i);

        float IngameTime = lastTime +  (Time.deltaTime * timeMultiplier) ;
        lastTime = IngameTime;
        transform.localEulerAngles = new Vector3(IngameTime - 90, -30, 0);
        CalculateTime(IngameTime);
    }

    private void CalculateTime(float RealTime)
    {
        float inGameSeconds = RealTime / (90.0f/21600.0f);

        float tmp = (inGameSeconds / 3600);
        int inGameHours = (int)(inGameSeconds / 3600) % 24;
        int inGameMinutes = (int)((tmp - inGameHours) * 60);
        GameTime.Hours = inGameHours;
        GameTime.Minutes = inGameMinutes;
        //print("Current Time: " + inGameHours + ":" + inGameMinutes);
    }
}
