﻿using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace CalcItUWP {
	public sealed partial class CalculationResult: UserControl {
		private MainPage mainPage;

		public CalculationResult() {
			this.InitializeComponent();
		}

		public CalculationResult(string expression, string result, string error, MainPage main) {
			this.InitializeComponent();

			controlExpression.Text = expression;
			if (result != null) {
				controlResult.Visibility = Visibility.Visible;
				controlResult.Text = result;
			}
			if (error != null) {
				controlError.Visibility = Visibility.Visible;
				controlError.Text = error;
			}
			mainPage = main;
		}

		private void copyToClipboard(string text) {
			DataPackage clipboardPackage = new DataPackage();
			clipboardPackage.SetText(text);
			Clipboard.SetContent(clipboardPackage);
		}

		private void onCopyAll(object sender, RoutedEventArgs e) {
			string toReturn = controlExpression.Text + "\n";
			toReturn += controlResult.Visibility == Visibility.Visible ? "= " + controlResult.Text : controlError.Text;
			copyToClipboard(toReturn);
		}

		private void onCopyExpression(object sender, RoutedEventArgs e) {
			copyToClipboard(controlExpression.Text);
		}

		private void onCopyResult(object sender, RoutedEventArgs e) {
			copyToClipboard(controlResult.Visibility == Visibility.Visible ? controlResult.Text : controlError.Text);
		}

		private void onRightClick(object sender, RightTappedRoutedEventArgs e) {
			((MenuFlyout)FlyoutBase.GetAttachedFlyout(this)).ShowAt(this, e.GetPosition(this));
		}

		private void onPointerOver(object sender, PointerRoutedEventArgs e) {
			VisualStateManager.GoToState(this, "pointerOver", false);
		}

		private void onPointerExit(object sender, PointerRoutedEventArgs e) {
			VisualStateManager.GoToState(this, "normal", false);
		}

		private void onPointerDown(object sender, PointerRoutedEventArgs e) {
			// There is a weird behavior that PointerDownThemeAnimation throws exception every second call.
			int trials = 0;
			while (++trials < 6) try {
				mouseDown.Begin();
				break;
			} catch {
				continue;
			}
		}

		private void onPointerUp(object sender, PointerRoutedEventArgs e) {
			mouseUp.Begin();
		}

		private void onLeftClick(object sender, TappedRoutedEventArgs e) {
			if (mainPage == null) return;
			mainPage.pasteTextToInput(controlExpression.Text);
		}

		public void setResultOutOfRange() {
			controlResult.Text = "?";
			controlResult.Visibility = Visibility.Visible;
			ToolTipService.SetToolTip(controlResult, Utils.getString("text/newOutputNumberOutOfRange"));
		}
	}
}