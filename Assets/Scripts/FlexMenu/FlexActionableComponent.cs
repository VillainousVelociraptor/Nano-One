using UnityEngine;
using System.Collections;
using System;

abstract public class FlexActionableComponent : MonoBehaviour {

    private Action<FlexActionableComponent, GameObject> exitCallback;
    private Action<FlexActionableComponent, GameObject> stayCallback;
    private Action<FlexActionableComponent, GameObject> enterCallback;

    #region Properties
    private uint state;
    public uint State
    {
        get
        {
            return state;
        }
        set
        {
            uint _old = state;
            state = value;
            StateChanged(_old, value);
        }
    }

    private FlexPanelComponent parentPanel;
    public FlexPanelComponent ParentPanel { 
        get
        {
            return parentPanel;
        }
    }
    #endregion

    #region Classes
    class TriggerForwarder : MonoBehaviour {
        public MonoBehaviour target;

        public TriggerForwarder WithTarget(MonoBehaviour target)
        {
            this.target = target;
            return this;
        }

        public void OnTriggerExit(Collider c)
        {
            target.SendMessage("OnTriggerExit", c);
        }

        public void OnTriggerStay(Collider c)
        {
            target.SendMessage("OnTriggerStay", c);
        }

        public void OnTriggerEnter(Collider c)
        {
            target.SendMessage("OnTriggerEnter", c);
        }
    }
    #endregion

    // This needs to be implemented by a subclass
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
	protected abstract void AssembleComponent();
    protected abstract void StateChanged(uint _old, uint _new);
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 

    public void SetupComponent(Transform parentTransform,
                       Action<FlexActionableComponent, GameObject> OnCollisionEnter,
                       Action<FlexActionableComponent, GameObject> OnCollisionExit)
    {
        this.name = name;
        this.transform.SetParent(parentTransform);
        IdentifyParentPanel();

        SetEnterCallback(OnCollisionEnter);
        SetStayCallback(OnCollisionEnter);
        SetExitCallback(OnCollisionExit);

		AssembleComponent();
    }

    protected FlexActionableComponent SetupTriggerForwardingFor(GameObject o)
    {
        o.AddComponent<TriggerForwarder>().WithTarget(this);
        return this;
    }

    #region Setters
    public void SetState(uint _new)
    {
        State = _new;
    }

    public void SetExitCallback(System.Action<FlexActionableComponent, GameObject> OnCollisionEnter)
    {
        this.exitCallback = OnCollisionEnter;
    }

    public void SetStayCallback(System.Action<FlexActionableComponent, GameObject> OnCollisionEnter)
    {
        this.stayCallback = OnCollisionEnter;
    }

    public void SetEnterCallback(System.Action<FlexActionableComponent, GameObject> OnCollisionEnter)
    {
        this.enterCallback = OnCollisionEnter;
    }
    #endregion

    private void IdentifyParentPanel()
    {
        // Walk up the hierarachy tree till we find a panel
        // This is important for building freeform panels
        Transform parent = this.transform;
        while (parent != null && parent.GetComponent<FlexPanelComponent>() == null)
        {
            parent = parent.parent;
        }

        if (parent != null)
        {
            parentPanel = parent.GetComponent<FlexPanelComponent>();
        }
        else
        {
            throw new System.Exception("Actionable Component initialised outside of a panel");
        }
    }

    #region CollisionInterface
    public void OnTriggerExit(Collider c)
    {
        if (exitCallback != null)
        {
            exitCallback(this, c.gameObject);
        }
    }

    public void OnTriggerStay(Collider c)
    {
        if (stayCallback != null)
        {
            stayCallback(this, c.gameObject);
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (enterCallback != null)
        {
            enterCallback(this, c.gameObject);
        }
    }
    #endregion
}
