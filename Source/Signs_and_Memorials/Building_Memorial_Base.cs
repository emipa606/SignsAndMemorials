using System;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace SaM
{
    public class Building_Memorial_Base : Building
    {
        //[ThisOneIsSavedTho]
        private string savedata;

        //
        // Static Fields
        //
        // this value is only correct when the object is placed.
        // in minified form this should be 5 but sadly we can't find that out easily
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
            try
            {
                var encoding = Encoding.Unicode;
                savedata = Convert.ToBase64String(
                    CompressUtility.Compress(encoding.GetBytes(text))
                );
            }
            catch (Exception E)
            {
                Log.ErrorOnce("Unable to encode memorial data for " + ThingID + ": " + E, ThingID.GetHashCode());
            }
        }

        private void LoadText()
        {
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
                Log.ErrorOnce("Unable to decode memorial data for " + ThingID + ": " + E, ThingID.GetHashCode());
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            SaveText();
            Scribe_Values.Look(ref savedata, "text");
        }

        public override void PostMapInit()
        {
            base.PostMapInit();
            LoadText();
        }

        // THIS METHOD HANDLES THE STRING IN THE BOTTOM LEFT
        public override string GetInspectString()
        {
            /*
			// REQUIRED FOR Text.CalcSize TO WORK PROPERLY
			Text.Font = TEXT_FONT;

			List<string> lines = new List<string>();

			foreach(string line in this.text.Split('\n')) {
				string short_line = "";
                if (string.IsNullOrEmpty(line))
                {
                    // lazy fix for an error that occurs when the lower display contains empty lines.
                    // usually that error is most likely valid, but here I believe it's best
                    // to preserve those empty lines to allow for greater stylistic freedom.
                    lines.Add(" ");
					// another valid alternative would be to skip those empty lines in the small summary,
					// but I feel it's better to show the "correct" representation of the text in question
                    continue;
                }
                if (Text.CalcSize(line).x > MAX_LENGTH) {
					foreach(string word in line.Split(' ')) {
						if(Text.CalcSize(short_line + word).x < MAX_LENGTH) {
							short_line += word + ' ';
						} else if(Text.CalcSize(short_line + word).x > MAX_LENGTH) {
							if(short_line.Length > 0) {
								lines.Add(short_line);
								short_line = "";
							}
							Stack<char> _word = new Stack<char>(word.Reverse());
							while(Text.CalcSize(new String(_word.ToArray())).x >= MAX_LENGTH) {
								string short_word = "";
								while(Text.CalcSize(short_word + _word.Peek()).x < MAX_LENGTH) {
									short_word += _word.Pop();
								}
								lines.Add(short_word);
							}
							short_line = new String(_word.ToArray()) + " ";
						} else {
							lines.Add(short_line + word);
							short_line = "";
						}
					}
				} else {
					short_line = line;
				}
				lines.Add(short_line.Trim());
			}

			if(lines.Count > MAX_LINES) {
				lines = lines.GetRange(0, MAX_LINES - 1);
				lines.Add("(...)");
			}
			return String.Join("\n", lines.ToArray());
			*/
            return text;
        }

        public override TipSignal GetTooltip()
        {
            return new TipSignal(text, thingIDNumber * 152317 /*251235*/, TooltipPriority.Default);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!(SaM_Mod.settings?.editOnBuild ?? false))
            {
                return;
            }

            CameraJumper.TryJumpAndSelect(this);
            Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect);
            ((MainTabWindow_Inspect) MainButtonDefOf.Inspect.TabWindow).OpenTabType =
                GetInspectTabs().OfType<ITab_View>().First().GetType();
        }
    }
}