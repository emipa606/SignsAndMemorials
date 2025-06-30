using Verse;

namespace SaM;

public class SaM_ModSettings : ModSettings
{
    //
    // Fields
    //
    public bool EditOnBuild;
    public bool PauseGameOnEdit;

    //
    // Constructors
    //

    //
    // Methods
    //
    public override void ExposeData()
    {
        Scribe_Values.Look(ref EditOnBuild, "edit_on_build");
        Scribe_Values.Look(ref PauseGameOnEdit, "pause_game_on_edit");
    }
}