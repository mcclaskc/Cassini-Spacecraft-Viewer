using UnityEngine;

class NGUIEventPasser : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////
    //Handle tooltips

    public string tooltipText = "";

    void Start()
    {
        tooltipText = tooltipText.Replace("\\n", "\n"); //properly unescape - e.g. turn \\n into \n
    }

    void OnTooltip(bool showTooltip)
    {
        if (!string.IsNullOrEmpty(tooltipText))
            UITooltip.ShowText(showTooltip ? tooltipText : null);
    }

    //////////////////////////////////////////////////////////////////////////////
    //Define the receiver for events

    public GameObject eventHandler;

    private GameObject EventHandler
    {
        get
        {
            if (eventHandler == null)
                Debug.LogError("There is no event handler connected to the '" + this.GetType().ToString() + "' script on '" + this.name + "'");
            
            return eventHandler;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    //Pass Events

    void OnHover(bool isOver)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.isOver = isOver;

        eventHandler.SendMessage(gameObject.name + "Hover", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnPress(bool isDown)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.isDown = isDown;

        eventHandler.SendMessage(gameObject.name + "Press", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnClick()
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);

        eventHandler.SendMessage(gameObject.name + "Click", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnDoubleClick()
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);

        eventHandler.SendMessage(gameObject.name + "DoubleClick", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnSelect(bool selected)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.isSelected = selected;

        eventHandler.SendMessage(gameObject.name + "Select", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnDrag(Vector2 delta)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.dragDelta = delta;

        eventHandler.SendMessage(gameObject.name + "Drag", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnDrop(GameObject drag)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.droppedObject = drag;

        eventHandler.SendMessage(gameObject.name + "Drop", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnMove(Vector2 delta)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.moveDelta = delta;

        eventHandler.SendMessage(gameObject.name + "Move", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnInput(string input)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.input = input;

        eventHandler.SendMessage(gameObject.name + "Input", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnScroll(float delta)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.scrollDelta = delta;

        eventHandler.SendMessage(gameObject.name + "Scroll", evt, SendMessageOptions.DontRequireReceiver);
    }

    void OnKey(KeyCode key)
    {
        NGUIEvent evt = new NGUIEvent(this.gameObject);
        evt.key = key;

        eventHandler.SendMessage(gameObject.name + "Key", evt, SendMessageOptions.DontRequireReceiver);
    }
}

public class NGUIEvent
{
    public NGUIEvent(GameObject originator)
    {
        this.originator = originator;
    }

    //Event parameters
    public GameObject originator;
    
    public bool isOver;
    public bool isDown;
    public bool isSelected;
    public float scrollDelta;
    public Vector2 moveDelta;
    public Vector2 dragDelta;
    public string input;
    public KeyCode key;
    public GameObject droppedObject;

    //convenience properties
    public UIWidget UIWidget
    {
        get
        {
            return originator.GetComponent<UIWidget>();
        }
    }


    public UIButton UIButton
    {
        get
        {
            return originator.GetComponent<UIButton>();
        }
    }

    public UICheckbox UICheckbox
    {
        get
        {
            return originator.GetComponent<UICheckbox>();
        }
    }

    public UISprite UISprite
    {
        get
        {
            return originator.GetComponent<UISprite>();
        }
    }

    public UILabel UILabel
    {
        get
        {
            return originator.GetComponent<UILabel>();
        }
    }
}