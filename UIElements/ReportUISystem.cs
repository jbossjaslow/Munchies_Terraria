using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.Configuration;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Munchies.UIElements {

	[Autoload(Side = ModSide.Client)]
	class ReportUISystem : ModSystem {
		internal static ReportUISystem Instance { get; private set; }

		internal ReportUI ReportUI;
		internal static UserInterface _reportUI;

		public static float ChecklistLeftPos;
		public static float ChecklistTopPos;

		public override void Load() {
			if (!Main.dedServ) {
				Instance = this;

				ReportUI = new ReportUI();
				ReportUI.Activate();
				_reportUI = new UserInterface();
				//_reportUI.SetState(ReportUI);

				ReportListItem.CheckMarkTexture = Mod.Assets.Request<Texture2D>("UIElements/checkMark");
				ReportUI.buttonDeleteTexture = Mod.Assets.Request<Texture2D>("UIElements/xMark");
			}
		}

		public override void Unload() {
			ReportUI = null;
		}

		public override void LoadWorldData(TagCompound tag) {
			ChecklistLeftPos = tag.GetFloat(SaveData.CheckListLeftPosKey);
			ChecklistTopPos = tag.GetFloat(SaveData.CheckListTopPosKey);
		}

		public override void SaveWorldData(TagCompound tag) {
			tag[SaveData.CheckListLeftPosKey] = ChecklistLeftPos;
			tag[SaveData.CheckListTopPosKey] = ChecklistTopPos;
		}

		public override void UpdateUI(GameTime gameTime) {
			_reportUI?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"YourMod: A Description",
					delegate {
						_reportUI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
