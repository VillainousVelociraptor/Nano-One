using UnityEngine;
using System.Collections.Generic;
using System;

public class FlexRootMenuManager : MonoBehaviour
{
    public FlexPanelComponent mainMenuPanel;

    private Dictionary<string, FlexPanelComponent> panels = new Dictionary<string, FlexPanelComponent>();

    private FlexPanelComponent activePanel;
    public FlexPanelComponent ActivePanel
    {
        get
        {
            return activePanel;
        }
    }

    public interface FlexMenuResponder
    {
        void Flex_ActionStart(string name, FlexActionableComponent sender, GameObject collider);
        void Flex_ActionEnd(string name, FlexActionableComponent sender, GameObject collider);
    }
    public FlexMenuResponder responder;

    void Start()
    {
        UpdatePanels();
        TransitionToPanelNamed(mainMenuPanel.name);
    }

    public void UpdatePanels()
    {
        panels.Clear();

        // Walk through all of the children of this menu and look for ones that have FlexPanelComponent
        for(int i = 0; i < transform.childCount; i++)
        {
            FlexPanelComponent child = transform.GetChild(i).gameObject.GetComponent<FlexPanelComponent>();
            if (child != null)
            {
                panels[child.gameObject.name] = child;
                child.RegisterWith(this);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }

    public void RegisterResponder(FlexMenuResponder responder)
    {
        this.responder = responder;
    }

    public void TransitionToPanelNamed(string name)
    {
        if (name == null || panels.ContainsKey(name))
        {
            foreach (string panelName in panels.Keys)
            {
                Debug.Log(panelName);
                panels[panelName].gameObject.SetActive(panelName == name);
            }
        }
        else
        {
            throw new System.Exception("Attempting to switch to a panel that does not exist");
        }
    }

    public void CloseMenu()
    {
        TransitionToPanelNamed(null);
    }

    public void OpenMenu(string name)
    {
        TransitionToPanelNamed(name);
    }

    public void OnCollisionExit(string name, FlexActionableComponent sender, GameObject collider)
    {
        if (responder != null) responder.Flex_ActionEnd(name, sender, collider);
    }

    public void OnCollisionEnter(string name, FlexActionableComponent sender, GameObject collider)
    {
        if (responder != null) responder.Flex_ActionStart(name, sender, collider);
    }
}