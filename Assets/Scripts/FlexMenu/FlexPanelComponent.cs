using UnityEngine;
using System.Collections.Generic;

abstract public class FlexPanelComponent : MonoBehaviour
{
    private FlexRootMenuManager manager;
    private List<FlexActionableComponent> actions = new List<FlexActionableComponent>();
    public List<FlexActionableComponent> Actions
    {
        get
        {
            return actions;
        }
    }

    public void RegisterWith(FlexRootMenuManager manager) 
    {
        this.manager = manager;
    }

    public void UpdateActions()
    {
        actions.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            FlexActionableComponent action = transform.GetChild(i).GetComponent<FlexActionableComponent>();
            if (action != null)
            {
                action.SetupComponent(transform, OnCollisionEnter, OnCollisionExit);
                actions.Add(action);
            }
        }
    }

    private void OnCollisionEnter(FlexActionableComponent sender, GameObject collider)
    {
        manager.OnCollisionEnter(name, sender, collider);
    }

    private void OnCollisionExit(FlexActionableComponent sender, GameObject collider)
    {
        manager.OnCollisionExit(name, sender, collider);
    }

}
