using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Inking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.InkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch | CoreInputDeviceTypes.Mouse;
        }

        private void HighlighterButtoner_Click(object sender, RoutedEventArgs e)
        {
            var drawingAttributes = new InkDrawingAttributes
            {
                DrawAsHighlighter = true,
                PenTip = PenTipShape.Rectangle,
                Size = new Size(4, 10),
                Color = Colors.Yellow
            };

            this.InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void PenButton_Click(object sender, RoutedEventArgs e)
        {
            var drawingAttributes = new InkDrawingAttributes
            {
                DrawAsHighlighter = false
            };

            this.InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            this.InkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Erasing;
        }

        private async void RecognizeButton_Click(object sender, RoutedEventArgs e)
        {
            var inkRecognizer = new InkRecognizerContainer();
            if (null != inkRecognizer)
            {
                var recognitionResults = await inkRecognizer.RecognizeAsync(this.InkCanvas.InkPresenter.StrokeContainer, InkRecognitionTarget.All);
                string recognizedText = string.Join(" ", recognitionResults.Select(i => i.GetTextCandidates()[0]));

                var messageDialog = new MessageDialog(recognizedText);
                await messageDialog.ShowAsync();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var fileSave = new FileSavePicker();
            fileSave.FileTypeChoices.Add("Gif with embedded ISF", new List<string> { ".gif" });

            var storageFile = await fileSave.PickSaveFileAsync();

            if (storageFile != null)
            {
                using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await this.InkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
                }
            }
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var fileOpen = new FileOpenPicker();
            fileOpen.FileTypeFilter.Add(".gif");

            var storageFile = await fileOpen.PickSingleFileAsync();

            if (storageFile != null)
            {
                using (var stream = await storageFile.OpenReadAsync())
                {
                    this.InkCanvas.InkPresenter.StrokeContainer.Clear();
                    await this.InkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                }
            }
        }
    }
}
