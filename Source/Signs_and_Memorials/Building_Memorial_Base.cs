using System;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace SaM;

public class Building_Memorial_Base : Building
{
    //[ThisOneIsSavedTho]
    private string savedata;

    //
    // Static Fields
    //
    // this value is only correct when the object is placed.
    // in minified form this should be 5, but sadly we can't find that out easily
    // so there will be a stupid scrollbar while in minified form.
    // you can also not access the full text, but I believe that's not a bad thing
    //private static int MAX_LINES = 6;
    //private static int MAX_LENGTH = 408; // this is valid ONLY for GameFont.Small
    //private static GameFont TEXT_FONT = GameFont.Small;

    //
    // Fields
    //
    [Unsaved] private string text;

    //
    // Constructors
    //

    //
    // Properties
    //
    public string Text
    {
        get => text;
        set
        {
            if (text == value)
            {
                return;
            }

            text = value;
            SaveText();
        }
    }

    //
    // Methods
    //
    public void SaveText()
    {
        if (string.IsNullOrEmpty(text))
        {
            savedata = string.Empty;
            return;
        }

        try
        {
            var encoding = Encoding.Unicode;
            savedata = Convert.ToBase64String(
                CompressUtility.Compress(encoding.GetBytes(text))
            );
        }
        catch (Exception E)
        {
            Log.ErrorOnce($"Unable to encode memorial data for {ThingID}: {E}", ThingID.GetHashCode());
        }
    }

    private void loadText()
    {
        if (string.IsNullOrEmpty(savedata))
        {
            text = string.Empty;
            return;
        }

        try
        {
            var encoding = Encoding.Unicode;
            var base64 = encoding.GetChars(
                CompressUtility.Decompress(Convert.FromBase64String(savedata))
            );
            text = new string(base64);
        }
        catch (Exception E)
        {
            Log.ErrorOnce($"Unable to decode memorial data for {ThingID}: {E}", ThingID.GetHashCode());
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            SaveText();
        }

        Scribe_Values.Look(ref savedata, "text");

        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            loadText();
        }
    }

    // THIS METHOD HANDLES THE STRING IN THE BOTTOM LEFT
    public override string GetInspectString()
    {
        return text;
    }

    public override TipSignal GetTooltip()
    {
        return new TipSignal(text, thingIDNumber * 152317 /*251235*/, TooltipPriority.Default);
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);

        if (!(SaM_Mod.Settings?.EditOnBuild ?? false))
        {
            return;
        }

        CameraJumper.TryJumpAndSelect(this);
        Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect);
        ((MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow).OpenTabType =
            GetInspectTabs().OfType<ITab_View>().First().GetType();
    }
}