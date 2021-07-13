using Verse;

namespace SaM
{
    public class SaM_ModSettings : ModSettings
    {
        //
        // Fields
        //
        public bool editOnBuild;
        public bool pauseGameOnEdit;

        //
        // Constructors
        //

        //
        // Methods
        //
        public override void ExposeData()
        {
            Scribe_Values.Look(ref editOnBuild, "edit_on_build");
            Scribe_Values.Look(ref pauseGameOnEdit, "pause_game_on_edit");
        }
    }
}