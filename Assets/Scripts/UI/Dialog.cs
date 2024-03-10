using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dialog
{
    public int id;
    public string eventName;
    public string script;
    public string leftImage;
    public string rightImage;
    public string speaker;
    public string spotLight;
    public string callFunction;
    public int jumpLine;
    public float speedMultifly = 1;
    public float endDelay;
    // int or float's null is Zero(0)
    public Dialog()
    {
        
    }
}
