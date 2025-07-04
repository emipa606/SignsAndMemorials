﻿using Mlie;
using UnityEngine;
using Verse;

namespace SaM;

public class SaM_Mod : Mod
{
    public static SaM_ModSettings Settings;
    private static string currentVersion;

    //
    // Constructors
    //
    public SaM_Mod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<SaM_ModSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    //
    // Methods
    //
    public override string SettingsCategory()
    {
        return "SaM_Settings_category".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Text.Font = GameFont.Small;

        const float margin = 10;
        var xPos = inRect.x + margin;
        var yOff = inRect.y + margin;
        var posW = inRect.width - (2 * margin);

        var editCheckbox = new Rect(xPos, yOff, posW, 24);
        yOff += editCheckbox.height + 1;
        var editDescription = new Rect(xPos, yOff, posW, 24);

        Widgets.CheckboxLabeled(editCheckbox, "SaM_Settings_editOnBuild_label".Translate(),
            ref Settings.EditOnBuild);
        Widgets.Label(editDescription, "SaM_Settings_editOnBuild_description".Translate());

        yOff += editDescription.height + 9;

        var pauseCheckbox = new Rect(xPos, yOff, posW, 24);
        yOff += pauseCheckbox.height + 1;
        var pauseDescription = new Rect(xPos, yOff, posW, 24);

        Widgets.CheckboxLabeled(pauseCheckbox, "SaM_Settings_pauseOnEdit_label".Translate(),
            ref Settings.PauseGameOnEdit);
        Widgets.Label(pauseDescription, "SaM_Settings_pauseOnEdit_description".Translate());
        if (currentVersion == null)
        {
            return;
        }

        yOff += pauseCheckbox.height + 1;
        var versionDescription = new Rect(xPos, yOff, posW, 24);
        GUI.contentColor = Color.gray;
        Widgets.Label(versionDescription, "SaM_Settings_modVersion_label".Translate(currentVersion));
        GUI.contentColor = Color.white;
    }
}