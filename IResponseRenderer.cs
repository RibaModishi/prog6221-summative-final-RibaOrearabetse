using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    // Interface defining how messages should be displayed to the user
    // Implementations of this interface handle rendering messages with specified colors
    public interface IResponseRenderer
    {
        // Method to display a message string in a specific console color
        void DisplayColoredMessage(string message, ConsoleColor color);
    }
}
