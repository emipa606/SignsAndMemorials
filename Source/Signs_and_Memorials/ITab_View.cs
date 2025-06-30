using RimWorld;
using UnityEngine;
using Verse;

namespace SaM;

public class ITab_View : ITab
{
    [Unsaved] private bool alreadyPaused;

    //
    // Fields
    //
    private bool editing;
    private string tabThingID;
    private string text = "";

    //
    // Constructors
    //
    public ITab_View()
    {
        size = new Vector2(570, 470);
        labelKey = "SaM_TabView";
        tutorTag = "View";
    }

    //
    // Properties
    //
    public override bool IsVisible => true;

    //
    // Methods
    //
    public override void TabUpdate()
    {
        base.TabUpdate();
        if (tabThingID == SelThing.GetUniqueLoadID())
        {
            return;
        }

        text = ((Building_Memorial_Base)SelThing).Text;
        tabThingID = SelThing.GetUniqueLoadID();
    }

    protected override void FillTab()
    {
        Text.Font = GameFont.Small;

        var rectText = new Rect(20, 20, size.x - 40, size.y - 75);
        var rectButton = new Rect(20, size.y - 50, size.x - 40, 30);
        var rectCancel = new Rect(rectButton.x, rectButton.y, (rectButton.width - 10) / 2, rectButton.height);
        var rectSave = new Rect((size.x + 10) / 2, rectButton.y, (rectButton.width - 10) / 2, rectButton.height);

        if (editing)
        {
            var memorial = (Building_Memorial_Base)SelThing;

            text = Widgets.TextArea(rectText, text);

            if (Widgets.ButtonText(rectCancel, "SaM_TabView_Cancel".Translate()))
            {
                editing = false;
                text = memorial.Text;
            }

            if (Widgets.ButtonText(rectSave, "SaM_TabView_Save".Translate()))
            {
                editing = false;
                memorial.Text = text;
            }

            if (editing)
            {
                return;
            }

            //state changed
            if (SaM_Mod.Settings.PauseGameOnEdit && Find.TickManager.Paused && !alreadyPaused)
            {
                Find.TickManager.TogglePaused();
            }

            memorial.SaveText();
        }
        else
        {
            Widgets.Label(rectText, text);

            if (!Widgets.ButtonText(rectButton, "SaM_TabView_Edit".Translate()))
            {
                return;
            }

            editing = true;
            if (!SaM_Mod.Settings.PauseGameOnEdit)
            {
                return;
            }

            if (Find.TickManager.Paused)
            {
                alreadyPaused = true;
            }
            else
            {
                alreadyPaused = false;
                Find.TickManager.TogglePaused();
            }
        }
    }
}