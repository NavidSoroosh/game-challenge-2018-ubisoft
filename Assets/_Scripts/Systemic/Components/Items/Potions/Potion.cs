using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class AEffectFactory
{
    public enum EffectType
    {
        SPEEDP,
        SPEEDM,
        RADIUSP,
        RADIUSM,
        SHIELD,
        STUN,
        PANACEE
    }

    public EffectType Type = EffectType.SPEEDP;

    public SpeedPlus SpeedPlus = new SpeedPlus();
    public SpeedMoins SpeedMoins = new SpeedMoins();
    public Stun Stun = new Stun();
    public Shield Shield = new Shield();
    public ZoneP ZoneP = new ZoneP();
    public ZoneM ZoneM = new ZoneM();
    public Panacee Panacee = new Panacee();

    public AEffect Create()
    {
        return GetAbilityFromType(Type);
    }

    public System.Type GetClassType(EffectType type)
    {
        return GetAbilityFromType(type).GetType();
    }

    private AEffect GetAbilityFromType(EffectType type)
    {
        switch (type)
        {
            case EffectType.SPEEDP:
                return SpeedPlus;
            case EffectType.SPEEDM:
                return SpeedMoins;
            case EffectType.RADIUSP:
                return ZoneP;
            case EffectType.RADIUSM:
                return ZoneM;
            case EffectType.SHIELD:
                return Shield;
            case EffectType.STUN:
                return Stun;
            case EffectType.PANACEE:
                return Panacee;
            default:
                return SpeedPlus;
        }
    }
}
[System.Serializable]
[RequireComponent(typeof(SphereCollider))]
public class Potion : EntityComponent
{
    public float duration;
    public float areaDuration;
    public float radius;
    public GameObject splashPrefab;
    public AEffectFactory factory = new AEffectFactory();

    private List<EffectReceiver> m_targets;
    private SphereCollider m_sc;
    private AEffect effect;
    private Rigidbody rb;
    private MeshRenderer mr;
    private float m_startSplash = -1;
    private void Start()
    {
        m_targets = new List<EffectReceiver>();
        effect = factory.Create();
        m_sc = GetComponent<SphereCollider>();
        m_sc.radius = radius;
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null)
        {
            EffectReceiver i = e.GetComponent<EffectReceiver>();
            if (i != null)
                m_targets.Add(i);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null)
        {
            EffectReceiver i = e.GetComponent<EffectReceiver>();
            if (i != null)
                m_targets.Remove(i);
        }
    }

    public void ApplyEffect(GameObject target)
    {
        effect.ApplyEffect(target, duration);
    }

    private void Update()
    {
        if (transform.position.y <= 0.5f) // pas fou...
        {
            if (m_startSplash == -1)
            {
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                mr.enabled = false;
                m_startSplash = Time.time;
                var go = Instantiate(splashPrefab, transform.position, Quaternion.identity);
                Destroy(go, areaDuration);
            }
            foreach (EffectReceiver e in m_targets)
            {
                ApplyEffect(e.gameObject);
            }
            if (Time.time - m_startSplash > areaDuration)
                Destroy(this.gameObject);
        }
    }
}


[CanEditMultipleObjects]
[CustomEditor(typeof(Potion), true)]
public class PotionEditor : Editor
{
    protected static string FACTORY_NAME = "factory";
    protected Potion Object;
    protected SerializedObject serializedLock;
    protected SerializedProperty SerializedLockCondition;
    void OnEnable()
    {
        Object = (Potion)target;
        serializedLock = new SerializedObject(Object);
        SerializedLockCondition = serializedLock.FindProperty(FACTORY_NAME);
    }
    public override void OnInspectorGUI()
    {
        serializedLock.Update();
        DrawPropertiesExcluding(serializedLock, new string[] { FACTORY_NAME });
        DrawPrimarySpell();
        serializedLock.ApplyModifiedProperties();
    }

    protected void DrawPrimarySpell()
    {
        EditorGUILayout.LabelField("Effect Type", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(SerializedLockCondition.FindPropertyRelative("Type"));

        AEffectFactory abilityFactory = Object.factory;
        System.Type typeOfAbility = abilityFactory.GetClassType(abilityFactory.Type);
        SerializedProperty specificAbility = (SerializedLockCondition.FindPropertyRelative(typeOfAbility.ToString())).Copy();
        string parentPath = specificAbility.propertyPath;
        while (specificAbility.NextVisible(true) && specificAbility.propertyPath.StartsWith(parentPath))
        {
            EditorGUILayout.PropertyField(specificAbility);
        }
    }
}