
using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    [HideInInspector]
    public string toolTip = "Power Up";

    public bool displayIcon = true;
    public Texture2D icon;
    public Vector2 iconPosition = Vector3.zero;
    public int uses = 5;

    [HideInInspector]
    public bool isActive = false;
    [HideInInspector]
    public bool isPassive = false;
    public float passiveLifetime = 20;
    [HideInInspector]
    public float dieAt = 0;


    public virtual void Init()
    {
        if (isPassive)
        {
            Destroy(this, passiveLifetime);
            dieAt = Time.time + passiveLifetime;
        }
        else
        {
            isActive = true;
        }
    }
    public void Dectivate() { isActive = false; }

    void OnGUI()
    {
        if (!displayIcon) return;
        if (!icon) return;
        if (!GetComponent<Hitpoints>()) return;

        Color c = Color.white;
        if (!isActive || isPassive) c.a = 0.5f;
        GUI.color = c;

        string s = "";
        if (uses > 1) s = uses.ToString();
        GUIContent content = new GUIContent(icon, toolTip);


        Rect rect = new Rect(iconPosition.x, iconPosition.y, icon.width, icon.height);
        GUI.Box(rect, content);
        s = ""; // uses or seconds remaining
        if (uses > 1) s = uses.ToString();
        if (isPassive)
        {
            float t = dieAt - Time.time;
            s = (Mathf.Round((dieAt - Time.time) * 10.0f) / 10.0f).ToString();
            if (t > 3) s = (Mathf.Floor(dieAt - Time.time)).ToString();
        }
        if (s != "")
        {
            GUI.color = Color.white;
            GUI.Label(rect, s);
        }
    }
}