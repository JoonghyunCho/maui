﻿using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.Editor)]
	public partial class EditorHandlerTests : HandlerTestBase<EditorHandler, EditorStub>
	{
		public EditorHandlerTests(HandlerTestFixture fixture) : base(fixture)
		{
		}

		[Fact(DisplayName = "Text Initializes Correctly")]
		public async Task TextInitializesCorrectly()
		{
			var editor = new EditorStub()
			{
				Text = "Test"
			};

			await ValidatePropertyInitValue(editor, () => editor.Text, GetNativeText, editor.Text);
		}

		[Theory(DisplayName = "Text Updates Correctly")]
		[InlineData(null, null)]
		[InlineData(null, "Hello")]
		[InlineData("Hello", null)]
		[InlineData("Hello", "Goodbye")]
		public async Task TextUpdatesCorrectly(string setValue, string unsetValue)
		{
			var editor = new EditorStub();

			await ValidatePropertyUpdatesValue(
				editor,
				nameof(IEditor.Text),
				h =>
				{
					var n = GetNativeText(h);
					if (string.IsNullOrEmpty(n))
						n = null; // native platforms may not upport null text
					return n;
				},
				setValue,
				unsetValue);
		}

		[Theory(DisplayName = "Is Read Only Initializes Correctly")]
		[InlineData(true)]
		[InlineData(false)]
		public async Task IsReadOnlyInitializesCorrectly(bool isReadOnly)
		{
			var editor = new EditorStub()
			{
				IsReadOnly = isReadOnly,
				Text = "Test"
			};

			await ValidatePropertyInitValue(editor, () => editor.IsReadOnly, GetNativeIsReadOnly, editor.IsReadOnly);
		}

		[Theory(DisplayName = "Is Text Prediction Enabled")]
		[InlineData(true)]
		[InlineData(false)]
		public async Task IsTextPredictionEnabledCorrectly(bool isEnabled)
		{
			var editor = new EditorStub()
			{
				IsTextPredictionEnabled = isEnabled
			};

			await ValidatePropertyInitValue(editor, () => editor.IsTextPredictionEnabled, GetNativeIsTextPredictionEnabled, isEnabled);
		}
	}
}