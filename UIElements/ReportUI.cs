using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.Models;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using System.Collections.Generic;
using System.Linq;
using Munchies.Configuration;
using Terraria.GameInput;

namespace Munchies.UIElements {
	class ReportUI : UIState {
		#region Properties
		DraggablePanel mainPanel = new();
		public UIPanel reportPanel;
		public UIList reportList = [];
		public UIText titleText = new("Consumables");
		//public UIMessageBox messageBox; // UIMessageBox from BossChecklist

		readonly float spacing = 8f;
		readonly float panelWidth = 300f;
		readonly float panelHeight = 500f;
		public static readonly float tabSize = 36f;

		private bool HasBeenInitialized = false;
		private bool mainPanelPosSetFromSavedPos = false;

		public static readonly Color BackgroundColor = new(73, 94, 171);

		public static Asset<Texture2D> buttonDeleteTexture;

		private ConsumableMod _currentTab;
		public ConsumableMod CurrentTab {
			get { return _currentTab; }
			set {
				if (_currentTab == value) return;

				_currentTab = value;
				//RedrawConsumablesList();
			}
		}
		public List<ReportTab> tabs = [];

		public static bool Visible => ReportUISystem._reportUI.CurrentState == ReportUISystem.Instance.ReportUI;
		public static void SetVisible(bool newValue, bool playCloseSound = true) {
			if (newValue == Visible) return;

			if (newValue) SoundEngine.PlaySound(SoundID.MenuOpen);
			else if (!newValue && playCloseSound) SoundEngine.PlaySound(SoundID.MenuClose);

			if (newValue) ReportUISystem.Instance.ReportUI.PresentUI(); // get out of the static context

			if (Config.instance.ResetDragCoords) {
				ReportUISystem.Instance.ReportUI.mainPanel.Left.Pixels = ReportUISystem.Instance.ReportUI.mainPanel.Top.Pixels = 0;
				Config.instance.ResetDragCoords = false;
				ReportUISystem.ChecklistLeftPos = 0;
				ReportUISystem.ChecklistTopPos = 0;
			}

			ReportUISystem._reportUI.SetState(newValue ? ReportUISystem.Instance.ReportUI : null);

			ReportUISystem.Instance.ReportUI.SetMainPanelPosFromSavedPos();
		}

		#endregion

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			Vector2 MousePosition = new(Main.mouseX, Main.mouseY);
			if(mainPanel.ContainsPoint(MousePosition)) {
				Main.player[Main.myPlayer].mouseInterface = true;
			}
		}

		public void PresentUI() {
			if (!HasBeenInitialized) {
				HasBeenInitialized = true;
				InitializeUI();
				RedrawConsumablesList();
				UpdateSelectedTab();
			}

			Main.playerInventory = false;
		}

		#region Initialize UI
		private void InitializeUI() {
			CurrentTab = Report.VanillaConsumableMod;

			InitializePanel();

			InitializeTitleTextAndCloseButton();

			InitializeListAndScrollBar();

			InitializeTabs();
		}

		private void InitializePanel() {
			//mainPanel = new();
			mainPanel.SetPadding(0);
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(panelWidth, 0f);
			mainPanel.Height.Set(panelHeight + (tabSize * 0.75f), 0f);
			mainPanel.BackgroundColor = Color.Transparent;
			mainPanel.BorderColor = Color.Transparent;
			Append(mainPanel);

			reportPanel = new();
			reportPanel.SetPadding(0);
			reportPanel.HAlign = 0.5f;
			reportPanel.Top.Set(tabSize * 0.75f, 0);
			reportPanel.Width.Set(panelWidth, 0f);
			reportPanel.Height.Set(panelHeight, 0f);
			reportPanel.BackgroundColor = BackgroundColor;
			mainPanel.Append(reportPanel);
		}

		private void SetMainPanelPosFromSavedPos() {
			if (mainPanelPosSetFromSavedPos) return;
			mainPanelPosSetFromSavedPos = true;
			mainPanel.Left.Pixels = ReportUISystem.ChecklistLeftPos;
			mainPanel.Top.Pixels = ReportUISystem.ChecklistTopPos;
			mainPanel.Recalculate();
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

			string closeTextLocalized = Language.GetTextValue("LegacyInterface.52"); // Localized text for "Close"
			ReportCloseButton closeButton = new(buttonDeleteTexture, closeTextLocalized, Color.Red);
			closeButton.Left.Set(-32f, 1f);
			closeButton.SetPadding(0);
			closeButton.Top.Set(10f, 0f);
			closeButton.Width.Set(20f, 0f);
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
				tab.Top.Set(0, 0);
				tab.Left.Pixels = spacing + (tabSize * i);
				tab.panel.BackgroundColor = (tab.mod.ModTabName == CurrentTab.ModTabName) ? Color.ForestGreen : BackgroundColor;
				mainPanel.Append(tab);
				tabs.Add(tab);
			}
		}

		#endregion

		#region Update UI
		private void UpdateSelectedTab() {
			if (CurrentTab == null) return;

			foreach(ReportTab tab in tabs) {
				tab.panel.BackgroundColor = (tab.mod.ModTabName == CurrentTab.ModTabName) ? Color.ForestGreen : BackgroundColor;
			}
		}

		#endregion

		#region User Interaction

		private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
			SetVisible(false);
		}

		private void OnSelectTab(ConsumableMod mod) {
			if (mod == null || CurrentTab == mod) return;

			CurrentTab = mod;
			if (Report.ConsumablesList.Count > 1) titleText.SetText(CurrentTab.ModTabName);
			RedrawConsumablesList();
			UpdateSelectedTab();
			//SoundEngine.PlaySound(SoundID.Tink);
		}

		internal void RedrawConsumablesList() {
			reportList.Clear();

			foreach (Consumable consumable in CurrentConsumables) {
				// If config is false for showing multi-use consumables, jump to next loop if consumable is multi-use
				if (!Config.instance.ShowMultiUseConsumables && consumable.IsMultiUse) continue;

				ReportListItem item = new(consumable);
				item.Width.Set(-10f, 1f);
				item.Height.Set(50, 0);
				item.Left.Set(10f, 0f);
				reportList.Add(item);
			}
			reportList.Activate();
		}

		public void UpdateConsumablesList(Item usedItem) {
			// This method is only called when the used item is already found in the CurrentConsumables list, so this extra forEach and Cast shouldn't have much of a performance impact
			foreach (ReportListItem item in reportList.Cast<ReportListItem>()) {
				if (usedItem.type == item.Consumable.ID) {
					item.AddCheckMarkOrCount();
					item.UpdateTextScale();
				}
			}
		}

		#endregion

		#region Helper methods

		public List<Consumable> CurrentConsumables => Report.ConsumablesList.Find(entry => entry.Mod.ModTabName == CurrentTab.ModTabName).Consumables;

		#endregion
	}
}
