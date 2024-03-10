using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractionInterface : MonoBehaviour
{
    public interface IMouseInteraction//보류,. NEED DEL
    {
        void LeftClick();
        void DoubleLeftClick();
        void RightClick();
        void DoubleRightClick();
    }
}
