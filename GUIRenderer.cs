using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CybersecurityAwarenessBot
{
    public class GUIRenderer : IResponseRenderer
    {
        private readonly StackPanel _chatStack;

        public GUIRenderer(StackPanel chatStack)
        {
            _chatStack = chatStack;
        }

        public void DisplayColoredMessage(string message, ConsoleColor color)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var textBlock = new TextBlock
                {
                    Text = message,
                    Margin = new Thickness(5),
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    FontFamily = new FontFamily("Segoe UI")
                };

                // Traditional switch statement instead of switch expression
                switch (color)
                {
                    case ConsoleColor.Red:
                        textBlock.Foreground = Brushes.Red;
                        break;
                    case ConsoleColor.Green:
                        textBlock.Foreground = Brushes.Green;
                        break;
                    case ConsoleColor.Yellow:
                        textBlock.Foreground = Brushes.Goldenrod;
                        break;
                    case ConsoleColor.Cyan:
                        textBlock.Foreground = Brushes.CadetBlue;
                        break;
                    case ConsoleColor.Magenta:
                        textBlock.Foreground = Brushes.Purple;
                        break;
                    default:
                        textBlock.Foreground = Brushes.Black;
                        break;
                }

                _chatStack.Children.Add(textBlock);
                if (_chatStack.Parent is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToEnd();
                }
            });
        }
    }
}