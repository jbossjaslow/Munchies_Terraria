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
using Terraria.GameInput;
using System.Collections.Generic;
using System.Linq;

namespace Munchies.UIElements {
	class ReportUI : UIState {
		public UIPanel reportPanel;
		public UIList reportList = [];
		public UIText titleText = new("Consumables");
		//public UIMessageBox messageBox; // UIMessageBox from BossChecklist

		readonly float spacing = 8f;
		readonly float panelWidth = 300f;
		readonly float panelHeight = 500f;
		public static readonly float tabSize = 36f;

		public static readonly Color BackgroundColor = new(73, 94, 171);

		private ConsumableMod _currentTab;
		public ConsumableMod CurrentTab {
			get { return _currentTab; }
			set {
				if (_currentTab == value) return;

				_currentTab = value;
				//UpdateConsumablesList();
			}
		}
		public List<ReportTab> tabs = [];

		public static bool Visible => ReportUISystem._reportUI.CurrentState == ReportUISystem.Instance.ReportUI;
		public static void SetVisible(bool newValue, bool playCloseSound = true) {
			if (newValue == Visible) return;

			if (newValue) SoundEngine.PlaySound(SoundID.MenuOpen);
			else if (!newValue && playCloseSound) SoundEngine.PlaySound(SoundID.MenuClose);

			if (newValue) ReportUISystem.Instance.ReportUI.PresentUI(); // get out of the static context

			ReportUISystem._reportUI.SetState(newValue ? ReportUISystem.Instance.ReportUI : null);
		}

		//public override void Update(GameTime gameTime) {
		//	base.Update(gameTime);

		//	// do something on update
		//}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			Vector2 MousePosition = new(Main.mouseX, Main.mouseY);
			if (ContainsPoint(MousePosition)) {
				Main.player[Main.myPlayer].mouseInterface = true;
				PlayerInput.LockVanillaMouseScroll("Munchies");
			}
		}

		public void PresentUI() {
			InitializeUI();
			//InitializeTabs();
			UpdateConsumablesList();
			UpdateSelectedTab();
			Main.playerInventory = false;
		}

		//public override void OnInitialize() {
			////Munchies.report ??= new(); // initialize the report if it is null
			//CurrentTab = Report.VanillaConsumableMod;
			////CurrentTab = new TestConsumableMod();

			//InitializePanel();

			//InitializeTitleTextAndCloseButton();

			//InitializeListAndScrollBar();

			////InitializeTabs();

			//Append(reportPanel);
		//}

		private bool HasBeenInitialized = false;
		private void InitializeUI() {
			if (HasBeenInitialized) return;
			HasBeenInitialized = true;
			//Munchies.report ??= new(); // initialize the report if it is null
			CurrentTab = Report.VanillaConsumableMod;

			InitializePanel();

			InitializeTitleTextAndCloseButton();

			InitializeListAndScrollBar();

			InitializeTabs();
		}

		private void InitializePanel() {
			reportPanel = new();
			reportPanel.SetPadding(0);
			reportPanel.HAlign = 0.5f;
			reportPanel.VAlign = 0.5f;
			reportPanel.Width.Set(panelWidth, 0f);
			reportPanel.Height.Set(panelHeight, 0f);
			reportPanel.BackgroundColor = BackgroundColor;
			Append(reportPanel);
		}

		private void InitializeTitleTextAndCloseButton() {
			string text = Report.ConsumablesList.Count > 1 ? "Terraria" : "Consumables";
			titleText = new(text: text, textScale: 1.5f) {
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

		private void InitializeTabs() {
			if (Report.ConsumablesList.Count <= 1) return; // don't show tabs if only vanilla

			for (int i = 0; i < Report.ConsumablesList.Count; i++) {
				ReportTab tab = new(mod: Report.ConsumablesList.ElementAt(i).Mod, index: i, OnSelectTab: OnSelectTab);
				// All of these have to be set outside, otherwise nothing works
				tab.Width.Set(tabSize, 0);
				tab.Height.Set(tabSize, 0);
				tab.HAlign = 0.5f;
				tab.VAlign = 0.5f;
				tab.Top.Set((-panelHeight / 2) - (tabSize * 0.275f), 0);
				tab.Left.Set((-panelWidth / 2) + (tabSize * 0.5f) + 10 + (tabSize * i), 0);
				tab.panel.BackgroundColor = (tab.mod.ModTabName == CurrentTab.ModTabName) ? Color.ForestGreen : BackgroundColor;
				Append(tab);
				tabs.Add(tab);
			}
		}

		private void UpdateSelectedTab() {
			if (CurrentTab == null) return;

			foreach(ReportTab tab in tabs) {
				tab.panel.BackgroundColor = (tab.mod.ModTabName == CurrentTab.ModTabName) ? Color.ForestGreen : BackgroundColor;
			}
		}

		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			SetVisible(false);
		}

		private void OnSelectTab(ConsumableMod mod) {
			if (mod == null || CurrentTab == mod) return;

			CurrentTab = mod;
			if (Report.ConsumablesList.Count > 1) titleText.SetText(CurrentTab.ModTabName);
			UpdateConsumablesList();
			UpdateSelectedTab();
			SoundEngine.PlaySound(SoundID.Tink);
		}

		internal void UpdateConsumablesList() {
			reportList.Clear();

			foreach (Consumable consumable in CurrentConsumables()) {
				ReportListItem item = new(consumable);
				item.Width.Set(-10f, 1f);
				item.Height.Set(50, 0);
				item.Left.Set(10f, 0f);
				reportList.Add(item);
			}
			reportList.Activate();
		}

		private List<Consumable> CurrentConsumables() {
			return Report.ConsumablesList.Find(entry => entry.Mod.ModTabName == CurrentTab.ModTabName).Consumables;
		}
	}
}
