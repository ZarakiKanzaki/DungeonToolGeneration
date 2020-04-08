
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TabNavigator : MonoBehaviour
{
    public int startIndex = 0;
    private int targetIndex;

    public List<ObjectTab> objectTabs = new List<ObjectTab>();
    private EventSystem myEventSystem;



    void Start()
    {
        myEventSystem = EventSystem.current;
        targetIndex = startIndex - 1;
        SetCurrentTabObject();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SetCurrentTabObject();
    }

    void SetCurrentTabObject()
    {

        targetIndex++;
        if (targetIndex >= objectTabs.Count)
            targetIndex = 0;



        if (!objectTabs[targetIndex].tabStop || !objectTabs[targetIndex].tabObject.activeSelf)
        {
            SetCurrentTabObject();
            return;
        }
        myEventSystem.SetSelectedGameObject(objectTabs[targetIndex].tabObject);
    }
}

[System.Serializable]
public class ObjectTab
{
    public bool tabStop;
    public GameObject tabObject;
}
