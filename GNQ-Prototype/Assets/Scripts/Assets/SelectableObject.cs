using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    protected enum HighlightState { NORMAL, DEFOCUSED, SELECTED, HIGHLIGHTED, PATHED, RANGED }
    protected HighlightState state = HighlightState.NORMAL;

    protected Renderer rend;
    private void Awake() {
        rend = GetComponent<Renderer>();
    }

    public bool inRange, hover;
    public Color normal, defocus, selected, highlighted, pathed, ranged, notRanged;

    public abstract void GrabHighlightColors(Player player);

    public void SetColors(Color n, Color d, Color s, Color h, Color p, Color r, Color nR) {
        normal = n;
        defocus = d;
        selected = s;
        highlighted = h;
        pathed = p;
        ranged = r;
        notRanged = nR;
    }

    private void SetColor(Color color) { rend.material.color = color; }

    // SETTING STATE and COLOR
    protected void SetNormal() {
        state = HighlightState.NORMAL;
        SetColor(normal);
        OnNormal();
    }
    protected void SetDefocused() {
        state = HighlightState.DEFOCUSED;
        SetColor(defocus);
        OnDefocused();
    }
    protected void SetSelected() {
        state = HighlightState.SELECTED;
        SetColor(selected);
        OnSelected();
    }
    protected void SetHighlighted() {
        state = HighlightState.HIGHLIGHTED;
        SetColor(highlighted);
        OnHighlighted();
    }
    protected void SetPathed() {
        state = HighlightState.PATHED;
        SetColor(pathed);
        OnPathed();
    }
    protected void SetRanged() {
        state = HighlightState.RANGED;
        if (inRange) { SetColor(ranged); }
        else { SetColor(notRanged); }
        OnRanged();
    }


    // OVERRIDEABLES
    public virtual void OnNormal() { }
    public virtual void OnDefocused() { }
    public virtual void OnSelected() { }
    public virtual void OnHighlighted() { }
    public virtual void OnPathed() { }
    public virtual void OnRanged() { }


    // ~~~~~ STATE MACHINE LOGIC ~~~~~~ //

    public void DoFocus(bool enabled) {
        if (enabled) {            
            if (state == HighlightState.DEFOCUSED) { SetNormal(); }
        }
        else {
            SetDefocused();
        }
    }

    public void DoHover(bool enter) {
        hover = enter;

        if (enter) {
            if (state == HighlightState.NORMAL) { SetHighlighted(); ; }
            if (state == HighlightState.PATHED) { SetRanged(); }
        }
        else {
            if (state == HighlightState.HIGHLIGHTED) { SetNormal(); }
            if (state == HighlightState.RANGED) { SetPathed(); }
        }
    }

    public void DoSelect(bool selected) {
        if (selected) {
            if (state == HighlightState.HIGHLIGHTED || state == HighlightState.RANGED) { SetSelected(); }
        }
        else {
            if (hover) { SetHighlighted(); }
            else { SetNormal(); }
        }
    }

    public void DoPath(bool enabled) {
        if (enabled) {
            if (state == HighlightState.NORMAL) { SetPathed(); }
            if (state == HighlightState.HIGHLIGHTED) { SetRanged(); }
        }
        else {
            if (state == HighlightState.PATHED) { SetNormal(); }
            if (state == HighlightState.RANGED) { SetHighlighted(); }
        }
    }

}
