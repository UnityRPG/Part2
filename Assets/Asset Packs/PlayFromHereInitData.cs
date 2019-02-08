using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PlayFromHereData")]
public class PlayFromHereInitData : ScriptableObject
{
    public GameObject[] m_arOriginPrefabs;
    public GameObject[] m_arPositionPrefabs;
    public LayerMask m_layerMask = ~0;

    public void SpawnMissingObjects(Vector3 _v3Position)
    {
        for (int i = 0; i < m_arOriginPrefabs.Length; i++)
        {
            if(m_arOriginPrefabs[i] != null)
            {
                if (GameObject.Find(m_arOriginPrefabs[i].name) == null)
                {
                    Instantiate(m_arOriginPrefabs[i]);

                }
            }
        }

        for (int i = 0; i < m_arPositionPrefabs.Length; i++)
        {
            if (m_arPositionPrefabs[i] != null)
            {
                GameObject goInScene = GameObject.Find(m_arPositionPrefabs[i].name);
                if (goInScene == null)
                {
                    Instantiate(m_arPositionPrefabs[i], _v3Position, Quaternion.identity);
                }
                else
                {
                    goInScene.transform.position = _v3Position;
                }
            }
        }
    }
}

[InitializeOnLoad]
public class PlayFromHere
{
    private const string c_strPlayFromHereObjectPath = "Assets/Game/Editor/PlayFromHereData.asset";

    private static double s_startClickTime;
    private static GameObject s_goHackObject;
    private static Vector3 s_v3Position;

    private static PlayFromHereInitData s_initDataObject;

    private const string c_strHackName = "Play From Here Hack Object";

    static PlayFromHere()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        s_goHackObject = GameObject.Find(c_strHackName);
        if (s_goHackObject != null)
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }
    }

    private static void OnPlayModeChanged(PlayModeStateChange _eStateChange)
    {
        switch (_eStateChange)
        {
            case PlayModeStateChange.EnteredEditMode:
                //If we are coming back from PlayMode which was started via PlayFromHere, we delete the helper game object
                if (s_goHackObject != null)
                {
                    GameObject.DestroyImmediate(s_goHackObject);
                }
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                //If we entered PlayMode and a Helper Object exists, we should use its position to initialize the PlayFromHere-Objects
                if (s_goHackObject != null)
                {
                    RefreshPlayFromHereDataObject();

                    if (s_initDataObject != null)
                    {
                        s_initDataObject.SpawnMissingObjects(s_goHackObject.transform.position);
                    }
                }
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
            default:
                break;
        }
    }

    private static void RefreshPlayFromHereDataObject()
    {
        if (s_initDataObject == null)
        {
            s_initDataObject = AssetDatabase.LoadAssetAtPath<PlayFromHereInitData>(c_strPlayFromHereObjectPath);
        }
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        //Check for right mouse button
        if (Event.current.isMouse && Event.current.button == 1  && !EditorApplication.isPlaying)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                //Save time of mouse down Event to later check for click duration
                s_startClickTime = EditorApplication.timeSinceStartup;
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                //Check time since mouse down and open menu for short click
                if (EditorApplication.timeSinceStartup - s_startClickTime < .25f)
                {
                    Camera camera = sceneView.camera;
                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    RaycastHit rayhit;

                    //Default layermask is "Everything"
                    LayerMask layerMask = ~0;

                    //Try to get layermask from DataObject
                    RefreshPlayFromHereDataObject();
                    if (s_initDataObject != null)
                    {
                        layerMask = s_initDataObject.m_layerMask;
                    }

                    //Get world position for click
                    if (Physics.Raycast(ray, out rayhit, float.MaxValue, layerMask, QueryTriggerInteraction.Ignore))
                    {
                        s_v3Position = rayhit.point + Vector3.up * .1f;

                        //Show confirmation window
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Play From Here"), false, OnConfirmPlayFromHere);

                        menu.ShowAsContext();

                        Event.current.Use();
                    }
                }
            }
        }
    }

    private static void OnConfirmPlayFromHere()
    {
        //Start PlayMode
        EditorApplication.isPlaying = true;

        //Unity reinitializes all scripts when starting PlayMode so we can't save any variables regularly
        //Create a GameObject with "DontSave"-Flag to "save" Position for start of PlayMode
        GameObject goPlayFromHere = new GameObject(c_strHackName);
        goPlayFromHere.transform.position = s_v3Position;
        goPlayFromHere.hideFlags = HideFlags.DontSave;
    }
}