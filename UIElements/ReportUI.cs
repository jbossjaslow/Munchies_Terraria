using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.Models;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Munchies.ModSupport;
using Terraria.GameInput;
using Newtonsoft.Json.Linq;

namespace Munchies.UIElements {
	class ReportUI : UIState {
		public UIPanel reportPanel;
		public UIList reportList;
		//public UIMessageBox messageBox; // UIMessageBox from BossChecklist

		readonly float spacing = 8f;
		readonly float panelWidth = 300f;
		readonly float panelHeight = 500f;
		readonly float panelHeightFromScreenHeight = -400f;

		public int totalTabs => Report.ModConsumables.Count > 0 ? Report.ModConsumables.Count + 1 : 1;

		//public static bool Visible {
		//	get { return ReportUISystem._reportUI.CurrentState == ReportUISystem.Instance.ReportUI; }
		//	set {
		//		if (value == Visible) return; // ensure we're only running code if the value is changing

		//		if (value) SoundEngine.PlaySound(SoundID.MenuOpen);
		//		else if(!value && Main.playerInventory) SoundEngine.PlaySound(SoundID.MenuClose);

		//		ReportUISystem._reportUI.SetState(value ? ReportUISystem.Instance.ReportUI : null);
		//	}
		//}

		public static bool Visible => ReportUISystem._reportUI.CurrentState == ReportUISystem.Instance.ReportUI;
		public static void SetVisible(bool newValue, bool playCloseSound = true) {
			if (newValue == Visible) return;

			if (newValue) SoundEngine.PlaySound(SoundID.MenuOpen);
			else if(!newValue && playCloseSound) SoundEngine.PlaySound(SoundID.MenuClose);

			if (newValue) ReportUISystem.Instance.ReportUI.PresentUI(); // get out of the static context

			ReportUISystem._reportUI.SetState(newValue ? ReportUISystem.Instance.ReportUI : null);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// do something on update
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			Vector2 MousePosition = new(Main.mouseX, Main.mouseY);
			if (ContainsPoint(MousePosition)) {
				Main.player[Main.myPlayer].mouseInterface = true;
				PlayerInput.LockVanillaMouseScroll("Munchies");

				// Doesn't fully fix problem. Clicks still happen in back to front manner.
				//Main.HoverItem = new Item();
				//Main.hoverItemName = "";
			}
		}

		public override void OnInitialize() {
			InitializePanel();

			InitializeTitleTextAndCloseButton();

			InitializeListAndScrollBar();

			Append(reportPanel);
		}

		private void InitializePanel() {
			reportPanel = new();
			reportPanel.SetPadding(0);
			reportPanel.HAlign = 0.5f;
			reportPanel.VAlign = 0.5f;
			reportPanel.Width.Set(panelWidth, 0f);
			reportPanel.Height.Set(panelHeight, 0f);
			reportPanel.BackgroundColor = new Color(73, 94, 171);
			//reportPanel.OverflowHidden = false;
		}

		private void InitializeTitleTextAndCloseButton() {
			UIText titleText = new(text: "Consumables", textScale: 1.5f) {
				TextColor = Color.White,
				ShadowColor = Color.Black,
				HAlign = 0.5f
			};
			titleText.Left.Set(0f, 0f);
			titleText.SetPadding(0f);
			titleText.Top.Set(10f, 0f);
			reportPanel.Append(titleText);

			Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonDelete");
			string closeTextLocalized = Language.GetTextValue("LegacyInterface.52"); // Localized text for "Close"
			ReportCloseButton closeButton = new(buttonDeleteTexture, closeTextLocalized);
			closeButton.Left.Set(-32f, 1f);
			closeButton.SetPadding(0);
			closeButton.Top.Set(10f, 0f);
			closeButton.Width.Set(22f, 0f);
			closeButton.Height.Set(22f, 0f);
			closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
			reportPanel.Append(closeButton);
		}

		private void InitializeListAndScrollBar() {
			reportList = [];
			reportList.Top.Pixels = 42f + spacing;
			reportList.Width.Set(-25f, 1f);
			reportList.HAlign = 0f;
			reportList.Height.Set(-42f - spacing * 2, 1f);
			reportList.ListPadding = 10f;
			reportList.SetPadding(0);
			reportPanel.Append(reportList);

			MunchiesUIScrollbar reportListScrollbar = new();
			reportListScrollbar.SetView(100f, 1000f);
			reportListScrollbar.Top.Pixels = 42f + spacing;
			reportListScrollbar.Height.Set(-42f - spacing * 2, 1f);
			reportListScrollbar.HAlign = 1f;
			reportPanel.Append(reportListScrollbar);
			reportList.SetScrollbar(reportListScrollbar);
		}

		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			SetVisible(false);
		}

		internal void AddConsumablesToList() {
			//Report.UpdateInfo();
			reportList.Clear();
			foreach (VanillaConsumable consumable in Report.VanillaConsumables) {
				ReportListItem item = new(consumable);
				item.Width.Set(-10f, 1f);
				item.Height.Set(50, 0);
				item.Left.Set(10f, 0f);
				reportList.Add(item);
			}
		}

		internal void PresentTabs() {

		}

		public void PresentUI() {
			AddConsumablesToList();
			Main.playerInventory = false;
		}
	}
}
