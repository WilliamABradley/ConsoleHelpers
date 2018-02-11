using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace WAConsoleHelpers
{
    /// <summary>
    /// A bunch of Helpful helpers for interacting with Console Applications.
    /// </summary>
    public static class ConsoleHelpers
    {
        private static TaskCompletionSource<int> CloseWaiter = new TaskCompletionSource<int>();

        /// <summary>
        /// The SystemColor, modify this to change what the System Prompt color is.
        /// </summary>
        public static ConsoleColor SystemColor = ConsoleColor.DarkCyan;

        /// <summary>
        /// Prevents continuation until a key is pressed, in a non-async workload.
        /// </summary>
        public static void AnyKeyContinue()
        {
            SystemWriteLine("Press Any Key to Continue");
            Console.ReadKey();
        }

        /// <summary>
        /// Prompts the User for a Yes/No response, Prompt message is appended with (y/n).
        /// </summary>
        /// <param name="Prompt">Prompt message.</param>
        /// <returns>State</returns>
        public static bool PromptYesNo(string Prompt)
        {
            SystemWriteLine(Prompt + " (y/n)");
            while (true)
            {
                var res = Console.ReadKey();
                if (res.Key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    return true;
                }
                else if (res.Key == ConsoleKey.N)
                {
                    Console.WriteLine();
                    return false;
                }
            }
        }

        /// <summary>
        /// Writes a message in the System Color.
        /// </summary>
        /// <param name="SystemText">Text to Print</param>
        public static void SystemWrite(string SystemText)
        {
            Console.ForegroundColor = SystemColor;
            Console.Write(SystemText);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a message line in the System Color.
        /// </summary>
        /// <param name="SystemText">Text to Print</param>
        public static void SystemWriteLine(string SystemText)
        {
            SystemWrite(SystemText + Environment.NewLine);
        }

        /// <summary>
        /// Prevents the Console Window from closing, requires calling <see cref="Close(int)"/> from a separate thread, as this will halt the thread to prevent close.
        /// </summary>
        public static void PreventClose()
        {
            CloseWaiter.Task.Wait();
        }

        /// <summary>
        /// This will close the Console window after <see cref="PreventClose"/> is used, returning an optional Program Code.
        /// </summary>
        /// <param name="Code">Program response code</param>
        public static void Close(int Code = 0)
        {
            CloseWaiter.TrySetResult(Code);
        }

        /// <summary>
        /// Prints a Property to the Screen.
        /// </summary>
        /// <param name="PropertyName">Property Name</param>
        /// <param name="Value">Property to Print</param>
        /// <param name="PreventSerialize">Prevents JSON Serialization of Classes.</param>
        public static void PrintProperty(string PropertyName, object Value, bool PreventSerialize = false)
        {
            SystemWrite($"{PropertyName}: ");
            string propertyvalue = Value?.ToString() ?? "null";

            if (Value?.GetType().IsClass == true && !PreventSerialize)
            {
                propertyvalue = Environment.NewLine + JsonConvert.SerializeObject(Value, Formatting.Indented);
            }

            Console.WriteLine(propertyvalue);
        }

        /// <summary>
        /// Hides password Entry by replacing characters with a HideChar to conceal input.
        /// </summary>
        /// <returns>Raw Password Text</returns>
        public static string EnterPassword(char HideChar = '*')
        {
            string pass = string.Empty;
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write(HideChar);
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            return pass;
        }
    }
}