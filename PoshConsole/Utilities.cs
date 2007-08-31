using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Markup;
using System.Xml;
using System.IO;

namespace PoshConsole
{
    public class Utilities
    {
        
		#region [rgn] Fields (1)

		public static Regex chunker = new Regex(@"[^ ""']+|([""'])[^\1]*?\1[^ ""']*|([""'])[^\1]*$", RegexOptions.Compiled);

		#endregion [rgn]

		#region [rgn] Methods (2)

		// [rgn] Public Methods (2)

		public static string GetLastWord(string cmdline)
        {
            string lastWord = null;
            MatchCollection words = chunker.Matches(cmdline);
            if (words.Count >= 1)
            {
                Match lw = words[words.Count - 1];
                lastWord = lw.Value;
                if (lastWord[0] == '"')
                {
                    lastWord = lastWord.Replace("\"", string.Empty);
                }
                else if (lastWord[0] == '\'')
                {
                    lastWord = lastWord.Replace("'", string.Empty);
                }
            }
            return lastWord;
        }
		
		public static bool IsModifierOn(KeyEventArgs e, ModifierKeys modifier)
        {
            return (e.KeyboardDevice.Modifiers & modifier) == modifier;
        }
		
		#endregion [rgn]

    }

    public static class EnumHelper
    {
        
		#region [rgn] Methods (7)

		// [rgn] Public Methods (7)

		public static IEnumerable<TOutput> ConvertAll<TInput, TOutput>(IEnumerable<TInput> input, Converter<TInput, TOutput> converter)
        {
            if (input != null)
            {
                foreach (TInput item in input)
                {
                    yield return converter(item);
                }
            }
        }
		
		public static T First<T>(System.Collections.ObjectModel.Collection<T> collection)
        {
            return (T)collection[0];
        }
		
		public static void ForEach<T>(IEnumerable<T> input, Action<T> action)
        {
            if (input != null)
            {
                foreach (T item in input)
                {
                    action(item);
                }
            }
        }
		
		public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }
		
		public static TEnum? Parse<TEnum>(string str) where TEnum : struct
        {
            return Parse<TEnum>(str, false);
        }
		
		public static TEnum? Parse<TEnum>(string str, bool ignoreCase) where TEnum : struct
        {
            TEnum value = (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
            if (IsDefined<TEnum>(value)) return value;
            return null;
        }
		
		public static void VerifyIsDefined<TEnum>(TEnum value, string argumentName) where TEnum : struct
        {
            if (!IsDefined<TEnum>(value))
            {
                throw new InvalidEnumArgumentException(argumentName, Convert.ToInt32(value), typeof(TEnum));
            }
        }
		
		#endregion [rgn]

    }

    internal static class PipelineHelper
    {
        
		#region [rgn] Methods (2)

		// [rgn] Public Methods (2)

		public static bool IsDone(System.Management.Automation.Runspaces.PipelineStateInfo psi)
        {
            return
                psi.State == System.Management.Automation.Runspaces.PipelineState.Completed ||
                psi.State == System.Management.Automation.Runspaces.PipelineState.Stopped ||
                psi.State == System.Management.Automation.Runspaces.PipelineState.Failed;
        }
		
		public static bool IsFailed(System.Management.Automation.Runspaces.PipelineStateInfo info)
        {
            return info.State == System.Management.Automation.Runspaces.PipelineState.Failed;
        }
		
		#endregion [rgn]

    }


    internal static class RectHelper
    {
        
		#region [rgn] Methods (2)

		// [rgn] Public Methods (2)

		public static int Height(System.Management.Automation.Host.Rectangle rect)
        {
            return rect.Bottom - rect.Top;
        }
		
		public static int Width(System.Management.Automation.Host.Rectangle rect)
        {
            return rect.Right - rect.Left;
        }
		
		#endregion [rgn]

    }

    internal static class ThicknessHelper
    {
        
		#region [rgn] Methods (2)

		// [rgn] Public Methods (2)

		public static double Height(System.Windows.Thickness t)
        {
            return t.Top + t.Bottom;
        }
		
		public static double Width(System.Windows.Thickness t)
        {
            return t.Left + t.Right;
        }
		
		#endregion [rgn]

    }

}