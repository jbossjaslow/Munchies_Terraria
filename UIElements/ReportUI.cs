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
using Terraria.ModLoader.UI;

namespace Munchies.UIElements {
	class ReportUI : UIState {
		#region Properties
		DraggablePanel mainPanel = new();
		public UIPanel reportPanel;
		public UIList reportList = [];
		public UIText titleText = new(Munchies.instance.GetLocalization("UI.Report.Consumables"));
		public UIImage completionStar;

		public LocalizedText completedText = Munchies.instance.GetLocalization("UI.Report.Completed");

		readonly float spacing = 10f;
		readonly float panelWidth = 300f;
		readonly float panelHeight = 500f;
		public static readonly float tabSize = 36f;

		public bool HasBeenInitialized = false;
		private bool mainPanelPosSetFromSavedPos = false;

		public static readonly Color BackgroundColor = new(73, 94, 171);

		public static Asset<Texture2D> closeButtonTexture;
		public static Asset<Texture2D> completionTexture;

		public static Asset<Texture2D> classicDifficultyTexture;
		public static Asset<Texture2D> expertDifficultyTexture;
		public static Asset<Texture2D> masterDifficultyTexture;
		public static Asset<Texture2D> customModDifficultyTexture;

		public List<ConsumableMod> completedTabs = [];

		private ConsumableMod _currentTab;
		public ConsumableMod CurrentTab {
			get { return _currentTab; }
			set {
				if (_currentTab == value) return;

				_currentTab = value;
				UpdateCurrentConsumables(_currentTab.ModTabName);
				//RedrawConsumablesList();
			}
		}
		public List<ReportTab> tabs = [];
		public List<Consumable> CurrentConsumables = [];

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
			if (completionStar?.IsMouseHovering ?? false) {
				UICommon.TooltipMouseText(completedText.Format(CurrentTab.ModTabName));
			}
		}

		public void PresentUI() {
			if (!HasBeenInitialized) {
				HasBeenInitialized = true;
				InitializeUI();
				RedrawConsumablesList();
				UpdateSelectedTab();
			}

			CheckForCompletion();
			Main.playerInventory = false;
		}

		#region Initialize UI
		private void InitializeUI() {
			CurrentTab = Report.VanillaConsumableMod;

			InitializePanel();

			InitializeHeaderUI();

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

		private void InitializeHeaderUI() {
			string text = Report.ConsumablesList.Count > 1 ? Report.VanillaConsumableMod.ModTabName : Munchies.DefaultTitle.Value;
			titleText = new(text: text, textScale: 1.5f) {
				TextColor = Color.White,
				ShadowColor = Color.Black,
				HAlign = 0.5f
			};
			titleText.Left.Set(0f, 0f);
			titleText.SetPadding(0f);
			titleText.Top.Set(spacing, 0f);
			reportPanel.Append(titleText);
			UpdateTitleTextSize();

			string closeTextLocalized = Language.GetTextValue("LegacyInterface.52"); // Localized text for "Close"
			ReportCloseButton closeButton = new(closeButtonTexture, closeTextLocalized, Color.Red);
			closeButton.Left.Set(-closeButtonTexture.Width() - 10, 1f);
			closeButton.SetPadding(0);
			closeButton.Top.Set(spacing, 0f);
			closeButton.Width.Set(closeButtonTexture.Width(), 0f);
			closeButton.Height.Set(closeButtonTexture.Height(), 0f);
			closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
			reportPanel.Append(closeButton);

			completionStar = new(completionTexture);
			completionStar.Left.Set(10, 0);
			completionStar.Top.Set(10, 0f);
			completionStar.Width.Set(completionTexture.Width(), 0f);
			completionStar.Height.Set(completionTexture.Height(), 0f);
			completionStar.SetPadding(0);
		}

		private void InitializeListAndScrollBar() {
			reportList = [];
			reportList.Top.Pixels = 42f + spacing;
			reportList.Width.Set(-25f, 1f);
			reportList.HAlign = 0f;
			reportList.Height.Set(-42f - spacing * 2, 1f);
			reportList.ListPadding = spacing;
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
				tab.panel.BackgroundColor = (tab.mod.SameAs(CurrentTab)) ? Color.ForestGreen : BackgroundColor;
				mainPanel.Append(tab);
				tabs.Add(tab);
			}
		}

		#endregion

		#region Update UI
		private void UpdateSelectedTab() {
			if (CurrentTab == null) return;

			foreach(ReportTab tab in tabs) {
				tab.panel.BackgroundColor = (tab.mod.SameAs(CurrentTab)) ? Color.ForestGreen : BackgroundColor;
			}
		}

		private void UpdateTitleTextSize() {
			Recalculate();
			CalculatedStyle textDimensions = titleText.GetInnerDimensions();
			float maxTextWidth = panelWidth - 100;
			if (textDimensions.Width > maxTextWidth) {
				float newScale = maxTextWidth / textDimensions.Width;
				titleText.SetText(text: CurrentTab.ModTabName, textScale: newScale * 1.5f, large: false);
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
			if (Report.ConsumablesList.Count > 1) titleText.SetText(text: CurrentTab.ModTabName, textScale: 1.5f, large: false);
			UpdateTitleTextSize();

			RedrawConsumablesList();
			UpdateSelectedTab();
			CheckForCompletion();
			//SoundEngine.PlaySound(SoundID.Tink);
		}

		internal void RedrawConsumablesList() {
			reportList.Clear();

			foreach (Consumable consumable in CurrentConsumables) {
				// If config is false for showing multi-use consumables, jump to next loop if consumable is multi-use
				if (!Config.instance.ShowMultiUseConsumables && consumable.IsMultiUse) continue;

				ReportListItem item = new(consumable);
				item.Width.Set(-spacing, 1f);
				item.Height.Set(50, 0);
				item.Left.Set(spacing, 0f);
				reportList.Add(item);
			}
			reportList.Activate();
		}

		public void UpdateConsumablesList(Item usedItem) {
			// This method is only called when the used item is already found in the UpdateCurrentConsumables list, so this extra forEach and Cast shouldn't have much of a performance impact
			foreach (ReportListItem item in reportList.Cast<ReportListItem>()) {
				if (usedItem.type == item.Consumable.ID) {
					item.AddCheckMarkOrCount();
					item.UpdateTextScale();
					CheckForCompletion();
					return;
				}
			}
		}

		#endregion

		#region Helper methods

		private void UpdateCurrentConsumables(string modName) {
			if (CurrentTab == null || Report.ConsumablesList == null) return;

			CurrentConsumables = Report.ConsumablesList.Find(entry => entry.Mod.ModTabName == modName).Consumables;
		}

		private void CheckForCompletion() {
			completionStar.Remove();
			if (completedTabs.Contains(CurrentTab)) {
				reportPanel.Append(completionStar);
				return;
			}

			// current tab has not been evaluated
			foreach(Consumable consumable in CurrentConsumables) {
				if (consumable.Available() && !consumable.HasBeenConsumed) {
					return;
				}
			}
			
			// at this point, all consumables have been consumed. Add to completed tabs list
			completedTabs.Add(CurrentTab);
			reportPanel.Append(completionStar);
		}

		#endregion
	}
}
