//////////////////////////////////////////////////////////////////////////////
//    Command Line Argument Parser
//    ----------------------------
//
//    Author: peterhal@microsoft.com
//
//    Shared Source License for Command Line Parser Library
//
//    This license governs use of the accompanying software ('Software'), and your
//    use of the Software constitutes acceptance of this license.
//
//    You may use the Software for any commercial or noncommercial purpose,
//    including distributing derivative works.
//
//    In return, we simply require that you agree:
//
//    1. Not to remove any copyright or other notices from the Software. 
//    2. That if you distribute the Software in source code form you do so only
//    under this license (i.e. you must include a complete copy of this license
//    with your distribution), and if you distribute the Software solely in
//    object form you only do so under a license that complies with this
//    license.
//    3. That the Software comes "as is", with no warranties. None whatsoever.
//    This means no express, implied or statutory warranty, including without
//    limitation, warranties of merchantability or fitness for a particular
//    purpose or any warranty of title or non-infringement. Also, you must pass
//    this disclaimer on whenever you distribute the Software or derivative
//    works.
//    4. That no contributor to the Software will be liable for any of those types
//    of damages known as indirect, special, consequential, or incidental
//    related to the Software or this license, to the maximum extent the law
//    permits, no matter what legal theory it�s based on. Also, you must pass
//    this limitation of liability on whenever you distribute the Software or
//    derivative works.
//    5. That if you sue anyone over patents that you think may apply to the
//    Software for a person's use of the Software, your license to the Software
//    ends automatically.
//    6. That the patent rights, if any, granted in this license only apply to the
//    Software, not to any derivative works you make.
//    7. That the Software is subject to U.S. export jurisdiction at the time it
//    is licensed to you, and it may be subject to additional export or import
//    laws in other places.  You agree to comply with all such laws and
//    regulations that may apply to the Software after delivery of the software
//    to you.
//    8. That if you are an agency of the U.S. Government, (i) Software provided
//    pursuant to a solicitation issued on or after December 1, 1995, is
//    provided with the commercial license rights set forth in this license,
//    and (ii) Software provided pursuant to a solicitation issued prior to
//    December 1, 1995, is provided with �Restricted Rights� as set forth in
//    FAR, 48 C.F.R. 52.227-14 (June 1987) or DFAR, 48 C.F.R. 252.227-7013
//    (Oct 1988), as applicable.
//    9. That your rights under this License end automatically if you breach it in
//    any way.
//    10.That all rights not expressly granted to you in this license are reserved.
//
//    Usage
//    -----
//
//    Parsing command line arguments to a console application is a common problem. 
//    This library handles the common task of reading arguments from a command line 
//    and filling in the values in a type.
//
//    To use this library, define a class whose fields represent the data that your 
//    application wants to receive from arguments on the command line. Then call 
//    CommandLine.ParseArguments() to fill the object with the data 
//    from the command line. Each field in the class defines a command line argument. 
//    The type of the field is used to validate the data read from the command line. 
//    The name of the field defines the name of the command line option.
//
//    The parser can handle fields of the following types:
//
//    - string
//    - int
//    - uint
//    - bool
//    - enum
//    - array of the above type
//
//    For example, suppose you want to read in the argument list for wc (word count). 
//    wc takes three optional boolean arguments: -l, -w, and -c and a list of files.
//
//    You could parse these arguments using the following code:
//
//    class WCArguments
//    {
//        public bool lines;
//        public bool words;
//        public bool chars;
//        public string[] files;
//    }
//
//    class WC
//    {
//        static void Main(string[] args)
//        {
//            if (CommandLine.ParseArgumentsWithUsage(args, parsedArgs))
//            {
//            //     insert application code here
//            }
//        }
//    }
//
//    So you could call this aplication with the following command line to count 
//    lines in the foo and bar files:
//
//        wc.exe /lines /files:foo /files:bar
//
//    The program will display the following usage message when bad command line 
//    arguments are used:
//
//        wc.exe -x
//
//    Unrecognized command line argument '-x'
//        /lines[+|-]                         short form /l
//        /words[+|-]                         short form /w
//        /chars[+|-]                         short form /c
//        /files:<string>                     short form /f
//        @<file>                             Read response file for more options
//
//    That was pretty easy. However, you realy want to omit the "/files:" for the 
//    list of files. The details of field parsing can be controled using custom 
//    attributes. The attributes which control parsing behaviour are:
//
//    ArgumentAttribute 
//        - controls short name, long name, required, allow duplicates,      value
//        and help text
//    DefaultArgumentAttribute 
//        - allows omition of the "/name".
//        - This attribute is allowed on only one field in the argument class.
//
//    So for the wc.exe program we want this:
//
//    using System;
//    using Utilities;
//
//    class WCArguments
//    {
//        [Argument(ArgumentType.AtMostOnce, HelpText="Count number of lines in the input text.")]
//        public bool lines;
//        [Argument(ArgumentType.AtMostOnce, HelpText="Count number of words in the input text.")]
//        public bool words;
//        [Argument(ArgumentType.AtMostOnce, HelpText="Count number of chars in the input text.")]
//        public bool chars;
//        [DefaultArgument(ArgumentType.MultipleUnique, HelpText="Input files to count.")]
//        public string[] files;
//    }
//
//    class WC
//    {
//        static void Main(string[] args)
//        {
//            WCArguments parsedArgs = new WCArguments();
//            if (CommandLine.ParseArgumentsWithUsage(args, parsedArgs))
//            {
//            //     insert application code here
//            }
//        }
//    }
//
//
//
//    So now we have the command line we want:
//
//        wc.exe /lines foo bar
//
//    This will set lines to true and will set files to an array containing the 
//    strings "foo" and "bar".
//
//    The new usage message becomes:
//
//        wc.exe -x
//
//    Unrecognized command line argument '-x'
//    /lines[+|-]  Count number of lines in the input text. (short form /l)
//    /words[+|-]  Count number of words in the input text. (short form /w)
//    /chars[+|-]  Count number of chars in the input text. (short form /c)
//    @<file>      Read response file for more options
//    <files>      Input files to count. (short form /f)
//
//    If you want more control over how error messages are reported, how /help is 
//    dealt with, etc you can instantiate the CommandLine.Parser class.
//
//
//
//    Cheers,
//    Peter Hallam
//    C# Compiler Developer
//    Microsoft Corp.
//
//
//
//
//    Release Notes
//    -------------
//
//    10/02/2002 Initial Release
//    10/14/2002 Bug Fix
//    01/08/2003 Bug Fix in @ include files
//    10/23/2004 Added user specified help text, formatting of help text to 
//            screen width. Added ParseHelp for /?.
//    11/23/2004 Added support for default values.
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace CommandLine
{
    /// <summary>
    /// Used to control parsing of command line arguments.
    /// </summary>
    [Flags]
    public enum ArgumentTypes
    {
        /// <summary>
        /// Indicates that this field is required. An error will be displayed
        /// if it is not present when parsing arguments.
        /// </summary>
        Required = 0x01,
        /// <summary>
        /// Only valid in conjunction with Multiple.
        /// Duplicate values will result in an error.
        /// </summary>
        Unique = 0x02,
        /// <summary>
        /// Inidicates that the argument may be specified more than once.
        /// Only valid if the argument is a collection
        /// </summary>
        Multiple = 0x04,

        /// <summary>
        /// The default type for non-collection arguments.
        /// The argument is not required, but an error will be reported if it is specified more than once.
        /// </summary>
        AtMostOnce = 0x00,

        /// <summary>
        /// For non-collection arguments, when the argument is specified more than
        /// once no error is reported and the value of the argument is the last
        /// value which occurs in the argument list.
        /// </summary>
        LastOccurenceWins = Multiple,

        /// <summary>
        /// The default type for collection arguments.
        /// The argument is permitted to occur multiple times, but duplicate 
        /// values will cause an error to be reported.
        /// </summary>
        MultipleUnique = Multiple | Unique,
    }

    /// <summary>
    /// Allows control of command line parsing.
    /// Attach this attribute to instance fields of types used
    /// as the destination of command line argument parsing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ArgumentAttribute: Attribute
    {
        /// <summary>
        /// Allows control of command line parsing.
        /// </summary>
        /// <param name="type"> Specifies the error checking to be done on the argument. </param>
        public ArgumentAttribute(ArgumentTypes type)
        {
            this.type = type;
        }

        /// <summary>
        /// The error checking to be done on the argument.
        /// </summary>
        public ArgumentTypes Type
        {
            get { return type; }
        }

        /// <summary>
        /// Returns true if the argument did not have an explicit short name specified.
        /// </summary>
        public bool DefaultShortName
        {
            get { return null == shortName; }
        }

        /// <summary>
        /// The short name of the argument.
        /// Set to null means use the default short name if it does not
        /// conflict with any other parameter name.
        /// Set to String.Empty for no short name.
        /// This property should not be set for DefaultArgumentAttributes.
        /// </summary>
        public string ShortName
        {
            get { return shortName; }
            set
            {
                Debug.Assert(value == null || !(this is DefaultArgumentAttribute));
                shortName = value;
            }
        }

        /// <summary>
        /// Returns true if the argument did not have an explicit long name specified.
        /// </summary>
        public bool DefaultLongName
        {
            get { return null == longName; }
        }

        /// <summary>
        /// The long name of the argument.
        /// Set to null means use the default long name.
        /// The long name for every argument must be unique.
        /// It is an error to specify a long name of String.Empty.
        /// </summary>
        public string LongName
        {
            get
            {
                Debug.Assert(!DefaultLongName);
                return longName;
            }
            set
            {
                Debug.Assert(value.Length > 0);
                longName = value;
            }
        }

        /// <summary>
        /// The default value of the argument.
        /// </summary>
        public object DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// Returns true if the argument has a default value.
        /// </summary>
        public bool HasDefaultValue
        {
            get { return null != defaultValue; }
        }

        /// <summary>
        /// Returns true if the argument has help text specified.
        /// </summary>
        public bool HasHelpText
        {
            get { return null != helpText; }
        }

        /// <summary>
        /// The help text for the argument.
        /// </summary>
        public string HelpText
        {
            get { return helpText; }
            set { helpText = value; }
        }

        private string shortName;
        private string longName;
        private string helpText;
        private object defaultValue;
        private readonly ArgumentTypes type;
    }

    /// <summary>
    /// Indicates that this argument is the default argument.
    /// '/' or '-' prefix only the argument value is specified.
    /// The ShortName property should not be set for DefaultArgumentAttribute
    /// instances. The LongName property is used for usage text only and
    /// does not affect the usage of the argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DefaultArgumentAttribute: ArgumentAttribute
    {
        /// <summary>
        /// Indicates that this argument is the default argument.
        /// </summary>
        /// <param name="type"> Specifies the error checking to be done on the argument. </param>
        public DefaultArgumentAttribute(ArgumentTypes type): base(type) {}
    }

    /// <summary>
    /// A delegate used in error reporting.
    /// </summary>
    public delegate void ReportError(string message);

    /// <summary>
    /// Parser for command line arguments.
    ///
    /// The parser specification is infered from the instance fields of the object
    /// specified as the destination of the parse.
    /// Valid argument types are: int, uint, string, bool, enums
    /// Also argument types of Array of the above types are also valid.
    /// 
    /// Error checking options can be controlled by adding a ArgumentAttribute
    /// to the instance fields of the destination object.
    ///
    /// At most one field may be marked with the DefaultArgumentAttribute
    /// indicating that arguments without a '-' or '/' prefix will be parsed as that argument.
    ///
    /// If not specified then the parser will infer default options for parsing each
    /// instance field. The default long name of the argument is the field name. The
    /// default short name is the first character of the long name. Long names and explicitly
    /// specified short names must be unique. Default short names will be used provided that
    /// the default short name does not conflict with a long name or an explicitly
    /// specified short name.
    ///
    /// Arguments which are array types are collection arguments. Collection
    /// arguments can be specified multiple times.
    /// </summary>
    public sealed class Parser
    {
        /// <summary>
        /// The System Defined new line string.
        /// </summary>
        public const string NewLine = "\r\n";

        /// <summary>
        /// Don't ever call this.
        /// </summary>
        private Parser() {}

        /// <summary>
        /// Parses Command Line Arguments. Displays usage message to Console.Out
        /// if /?, /help or invalid arguments are encounterd.
        /// Errors are output on Console.Error.
        /// Use ArgumentAttributes to control parsing behaviour.
        /// </summary>
        /// <param name="arguments"> The actual arguments. </param>
        /// <param name="destination"> The resulting parsed arguments. </param>
        /// <returns> true if no errors were detected. </returns>
        public static bool ParseArgumentsWithUsage(string[] arguments, object destination)
        {
            if (ParseHelp(arguments) || !ParseArguments(arguments, destination))
            {
                // error encountered in arguments. Display usage message
                Console.Write(ArgumentsUsage(destination.GetType()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parses Command Line Arguments. 
        /// Errors are output on Console.Error.
        /// Use ArgumentAttributes to control parsing behaviour.
        /// </summary>
        /// <param name="arguments"> The actual arguments. </param>
        /// <param name="destination"> The resulting parsed arguments. </param>
        /// <returns> true if no errors were detected. </returns>
        public static bool ParseArguments(string[] arguments, object destination)
        {
            return ParseArguments(arguments, destination, Console.Error.WriteLine);
        }

        /// <summary>
        /// Parses Command Line Arguments. 
        /// Use ArgumentAttributes to control parsing behaviour.
        /// </summary>
        /// <param name="arguments"> The actual arguments. </param>
        /// <param name="destination"> The resulting parsed arguments. </param>
        /// <param name="reporter"> The destination for parse errors. </param>
        /// <returns> true if no errors were detected. </returns>
        public static bool ParseArguments(string[] arguments,
                                          object destination,
                                          ReportError reporter)
        {
            Parser parser = new Parser(destination.GetType(), reporter);
            return parser.Parse(arguments, destination);
        }

        private static void NullReportError(string message) {}

        private class HelpArgument
        {
            [Argument(ArgumentTypes.AtMostOnce, ShortName = "?")]
            public bool help;
        }

        /// <summary>
        /// Checks if a set of arguments asks for help.
        /// </summary>
        /// <param name="args"> Args to check for help. </param>
        /// <returns> Returns true if args contains /? or /help. </returns>
        public static bool ParseHelp(string[] args)
        {
            Parser helpParser = new Parser(typeof (HelpArgument), NullReportError);
            HelpArgument helpArgument = new HelpArgument();
            helpParser.Parse(args, helpArgument);
            return helpArgument.help;
        }

        /// <summary>
        /// Returns a Usage string for command line argument parsing.
        /// Use ArgumentAttributes to control parsing behaviour.
        /// Formats the output to the width of the current console window.
        /// </summary>
        /// <param name="argumentType"> The type of the arguments to display usage for. </param>
        /// <returns> Printable string containing a user friendly description of command line arguments. </returns>
        public static string ArgumentsUsage(Type argumentType)
        {
            int screenWidth = GetConsoleWindowWidth();
            if (screenWidth == 0)
            {
                screenWidth = 80;
            }
            return ArgumentsUsage(argumentType, screenWidth);
        }

        /// <summary>
        /// Returns a Usage string for command line argument parsing.
        /// Use ArgumentAttributes to control parsing behaviour.
        /// </summary>
        /// <param name="argumentType"> The type of the arguments to display usage for. </param>
        /// <param name="columns"> The number of columns to format the output to. </param>
        /// <returns> Printable string containing a user friendly description of command line arguments. </returns>
        public static string ArgumentsUsage(Type argumentType, int columns)
        {
            return (new Parser(argumentType, null)).GetUsageString(columns);
        }

        private const int STD_OUTPUT_HANDLE = -11;

        private struct COORD
        {
            internal Int16 x;
            internal Int16 y;
        }

        private struct SMALL_RECT
        {
            internal Int16 Left;
            internal Int16 Top;
            internal Int16 Right;
            internal Int16 Bottom;
        }

        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal Int16 wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
        }

        private static class NativeMethods
        {
            [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true,
                    CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo",
                    SetLastError = true, CharSet = CharSet.Auto,
                    CallingConvention = CallingConvention.StdCall)]
            public static extern int GetConsoleScreenBufferInfo(int hConsoleOutput,
                                                                ref CONSOLE_SCREEN_BUFFER_INFO
                                                                        lpConsoleScreenBufferInfo);
        }

        /// <summary>
        /// Returns the number of columns in the current console window
        /// </summary>
        /// <returns>Returns the number of columns in the current console window</returns>
        public static int GetConsoleWindowWidth()
        {
            int screenWidth;
            CONSOLE_SCREEN_BUFFER_INFO csbi = new CONSOLE_SCREEN_BUFFER_INFO();

            int rc;
            rc =
                    NativeMethods.GetConsoleScreenBufferInfo(
                            NativeMethods.GetStdHandle(STD_OUTPUT_HANDLE), ref csbi);
            screenWidth = csbi.dwSize.x;
            return screenWidth;
        }

        /// <summary>
        /// Searches a StringBuilder for a character
        /// </summary>
        /// <param name="text"> The text to search. </param>
        /// <param name="value"> The character value to search for. </param>
        /// <param name="startIndex"> The index to stat searching at. </param>
        /// <returns> The index of the first occurence of value or -1 if it is not found. </returns>
        public static int IndexOf(StringBuilder text, char value, int startIndex)
        {
            for (int index = startIndex;index < text.Length;index++)
            {
                if (text[index] == value)
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches a StringBuilder for a character in reverse
        /// </summary>
        /// <param name="text"> The text to search. </param>
        /// <param name="value"> The character to search for. </param>
        /// <param name="startIndex"> The index to start the search at. </param>
        /// <returns>The index of the last occurence of value in text or -1 if it is not found. </returns>
        public static int LastIndexOf(StringBuilder text, char value, int startIndex)
        {
            for (int index = Math.Min(startIndex, text.Length - 1);index >= 0;index --)
            {
                if (text[index] == value)
                {
                    return index;
                }
            }

            return -1;
        }

        private const int spaceBeforeParam = 2;

        /// <summary>
        /// Creates a new command line argument parser.
        /// </summary>
        /// <param name="argumentSpecification"> The type of object to  parse. </param>
        /// <param name="reporter"> The destination for parse errors. </param>
        public Parser(Type argumentSpecification, ReportError reporter)
        {
            this.reporter = reporter;
            arguments = new ArrayList();
            argumentMap = new Hashtable();

            foreach (FieldInfo field in argumentSpecification.GetFields())
            {
                if (!field.IsStatic && !field.IsInitOnly && !field.IsLiteral)
                {
                    ArgumentAttribute attribute = GetAttribute(field);
                    if (attribute is DefaultArgumentAttribute)
                    {
                        Debug.Assert(defaultArgument == null);
                        defaultArgument = new Argument(attribute, field, reporter);
                    }
                    else
                    {
                        arguments.Add(new Argument(attribute, field, reporter));
                    }
                }
            }

            // add explicit names to map
            foreach (Argument argument in arguments)
            {
                Debug.Assert(!argumentMap.ContainsKey(argument.LongName));
                argumentMap[argument.LongName] = argument;
                if (argument.ExplicitShortName)
                {
                    if (argument.ShortName != null && argument.ShortName.Length > 0)
                    {
                        Debug.Assert(!argumentMap.ContainsKey(argument.ShortName));
                        argumentMap[argument.ShortName] = argument;
                    }
                    else
                    {
                        argument.ClearShortName();
                    }
                }
            }

            // add implicit names which don't collide to map
            foreach (Argument argument in arguments)
            {
                if (!argument.ExplicitShortName)
                {
                    if (argument.ShortName != null && argument.ShortName.Length > 0 &&
                        !argumentMap.ContainsKey(argument.ShortName))
                    {
                        argumentMap[argument.ShortName] = argument;
                    }
                    else
                    {
                        argument.ClearShortName();
                    }
                }
            }
        }

        private static ArgumentAttribute GetAttribute(FieldInfo field)
        {
            object[] attributes = field.GetCustomAttributes(typeof (ArgumentAttribute), false);
            if (attributes.Length == 1)
            {
                return (ArgumentAttribute) attributes[0];
            }

            Debug.Assert(attributes.Length == 0);
            return null;
        }

        private void ReportUnrecognizedArgument(string argument)
        {
            reporter(string.Format("Unrecognized command line argument '{0}'", argument));
        }

        /// <summary>
        /// Parses an argument list into an object
        /// </summary>
        /// <param name="args"></param>
        /// <param name="destination"></param>
        /// <returns> true if an error occurred </returns>
        private bool ParseArgumentList(string[] args, object destination)
        {
            bool hadError = false;
            if (args != null)
            {
                foreach (string argItem in args)
                {
                    string argument;
                    if (argItem.Length > 0)
                    {
                        //treat '/' as '-' if Windows but on Unix it is a directory separator
                        if (Environment.OSVersion.Platform != PlatformID.Unix && argItem[0] == '/')
                        {
                            argument = '-' + argItem.Substring(1);
                        }
                        else
                        {
                            argument = argItem;
                        }

                        switch (argument[0])
                        {
                            case '-':
                                int endIndex = argument.IndexOfAny(new char[] {':', '+', '-'}, 1);
                                string option = argument.Substring(1,
                                                                   endIndex == -1
                                                                           ? argument.Length - 1
                                                                           : endIndex - 1);
                                string optionArgument;
                                if (option.Length + 1 == argument.Length)
                                {
                                    optionArgument = null;
                                }
                                else if (argument.Length > 1 + option.Length &&
                                         argument[1 + option.Length] == ':')
                                {
                                    optionArgument = argument.Substring(option.Length + 2);
                                }
                                else
                                {
                                    optionArgument = argument.Substring(option.Length + 1);
                                }

                                Argument arg = (Argument) argumentMap[option];
                                if (arg == null)
                                {
                                    ReportUnrecognizedArgument(argument);
                                    hadError = true;
                                }
                                else
                                {
                                    hadError |= !arg.SetValue(optionArgument, destination);
                                }
                                break;
                            case '@':
                                string[] nestedArguments;
                                hadError |= LexFileArguments(argument.Substring(1),
                                                             out nestedArguments);
                                hadError |= ParseArgumentList(nestedArguments, destination);
                                break;
                            default:
                                if (defaultArgument != null)
                                {
                                    hadError |= !defaultArgument.SetValue(argument, destination);
                                }
                                else
                                {
                                    ReportUnrecognizedArgument(argument);
                                    hadError = true;
                                }
                                break;
                        }
                    }
                }
            }

            return hadError;
        }

        /// <summary>
        /// Parses an argument list.
        /// </summary>
        /// <param name="args"> The arguments to parse. </param>
        /// <param name="destination"> The destination of the parsed arguments. </param>
        /// <returns> true if no parse errors were encountered. </returns>
        public bool Parse(string[] args, object destination)
        {
            bool hadError = ParseArgumentList(args, destination);

            // check for missing required arguments
            foreach (Argument arg in arguments)
            {
                hadError |= arg.Finish(destination);
            }
            if (defaultArgument != null)
            {
                hadError |= defaultArgument.Finish(destination);
            }

            return !hadError;
        }

        private struct ArgumentHelpStrings
        {
            public ArgumentHelpStrings(string syntax, string help)
            {
                this.syntax = syntax;
                this.help = help;
            }

            public readonly string syntax;
            public readonly string help;
        }

        /// <summary>
        /// A user firendly usage string describing the command line argument syntax.
        /// </summary>
        public string GetUsageString(int screenWidth)
        {
            ArgumentHelpStrings[] strings = GetAllHelpStrings();

            int maxParamLen = 0;
            foreach (ArgumentHelpStrings helpString in strings)
            {
                maxParamLen = Math.Max(maxParamLen, helpString.syntax.Length);
            }

            const int minimumNumberOfCharsForHelpText = 10;
            const int minimumHelpTextColumn = 5;
            const int minimumScreenWidth = minimumHelpTextColumn + minimumNumberOfCharsForHelpText;

            int helpTextColumn;
            int idealMinimumHelpTextColumn = maxParamLen + spaceBeforeParam;
            screenWidth = Math.Max(screenWidth, minimumScreenWidth);
            if (screenWidth < (idealMinimumHelpTextColumn + minimumNumberOfCharsForHelpText))
            {
                helpTextColumn = minimumHelpTextColumn;
            }
            else
            {
                helpTextColumn = idealMinimumHelpTextColumn;
            }

            const string newLine = "\n";
            StringBuilder builder = new StringBuilder();
            foreach (ArgumentHelpStrings helpStrings in strings)
            {
                // add syntax string
                int syntaxLength = helpStrings.syntax.Length;
                builder.Append(helpStrings.syntax);

                // start help text on new line if syntax string is too long
                int currentColumn = syntaxLength;
                if (syntaxLength >= helpTextColumn)
                {
                    builder.Append(newLine);
                    currentColumn = 0;
                }

                // add help text broken on spaces
                int charsPerLine = screenWidth - helpTextColumn;
                int index = 0;
                while (index < helpStrings.help.Length)
                {
                    // tab to start column
                    builder.Append(' ', helpTextColumn - currentColumn);
                    currentColumn = helpTextColumn;

                    // find number of chars to display on this line
                    int endIndex = index + charsPerLine;
                    if (endIndex >= helpStrings.help.Length)
                    {
                        // rest of text fits on this line
                        endIndex = helpStrings.help.Length;
                    }
                    else
                    {
                        endIndex = helpStrings.help.LastIndexOf(' ',
                                                                endIndex - 1,
                                                                Math.Min(endIndex - index,
                                                                         charsPerLine));
                        if (endIndex <= index)
                        {
                            // no spaces on this line, append full set of chars
                            endIndex = index + charsPerLine;
                        }
                    }

                    // add chars
                    builder.Append(helpStrings.help, index, endIndex - index);
                    index = endIndex;

                    // do new line
                    AddNewLine(newLine, builder, ref currentColumn);

                    // don't start a new line with spaces
                    while (index < helpStrings.help.Length && helpStrings.help[index] == ' ')
                    {
                        index ++;
                    }
                }

                // add newline if there's no help text                
                if (helpStrings.help.Length == 0)
                {
                    builder.Append(newLine);
                }
            }

            return builder.ToString();
        }

        private static void AddNewLine(string newLine, StringBuilder builder, ref int currentColumn)
        {
            builder.Append(newLine);
            currentColumn = 0;
        }

        private ArgumentHelpStrings[] GetAllHelpStrings()
        {
            ArgumentHelpStrings[] strings = new ArgumentHelpStrings[NumberOfParametersToDisplay()];

            int index = 0;
            foreach (Argument arg in arguments)
            {
                strings[index] = GetHelpStrings(arg);
                index++;
            }
            //hatton removed this: 
            //strings[index++] = new ArgumentHelpStrings("@<file>", "Read response file for more options");
            strings[index++] = new ArgumentHelpStrings("", "");

            if (defaultArgument != null)
            {
                strings[index++] = GetHelpStrings(defaultArgument);
            }

            return strings;
        }

        private static ArgumentHelpStrings GetHelpStrings(Argument arg)
        {
            return new ArgumentHelpStrings(arg.SyntaxHelp, arg.FullHelpText);
        }

        private int NumberOfParametersToDisplay()
        {
            int numberOfParameters = arguments.Count + 1;
            if (HasDefaultArgument)
            {
                numberOfParameters += 1;
            }
            return numberOfParameters;
        }

        /// <summary>
        /// Does this parser have a default argument.
        /// </summary>
        /// <value> Does this parser have a default argument. </value>
        public bool HasDefaultArgument
        {
            get { return defaultArgument != null; }
        }

        private bool LexFileArguments(string fileName, out string[] arguments)
        {
            string args = null;

            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    args = (new StreamReader(file)).ReadToEnd();
                }
            }
            catch (Exception e)
            {
                reporter(string.Format(
                                 "Error: Can't open command line argument file '{0}' : '{1}'",
                                 fileName,
                                 e.Message));
                arguments = null;
                return false;
            }

            bool hadError = false;
            ArrayList argArray = new ArrayList();
            StringBuilder currentArg = new StringBuilder();
            bool inQuotes = false;
            int index = 0;

            // while (index < args.Length)
            try
            {
                while (true)
                {
                    // skip whitespace
                    while (char.IsWhiteSpace(args[index]))
                    {
                        index += 1;
                    }

                    // # - comment to end of line
                    if (args[index] == '#')
                    {
                        index += 1;
                        while (args[index] != '\n')
                        {
                            index += 1;
                        }
                        continue;
                    }

                    // do one argument
                    do
                    {
                        if (args[index] == '\\')
                        {
                            int cSlashes = 1;
                            index += 1;
                            while (index == args.Length && args[index] == '\\')
                            {
                                cSlashes += 1;
                            }

                            if (index == args.Length || args[index] != '"')
                            {
                                currentArg.Append('\\', cSlashes);
                            }
                            else
                            {
                                currentArg.Append('\\', (cSlashes >> 1));
                                if (0 != (cSlashes & 1))
                                {
                                    currentArg.Append('"');
                                }
                                else
                                {
                                    inQuotes = !inQuotes;
                                }
                            }
                        }
                        else if (args[index] == '"')
                        {
                            inQuotes = !inQuotes;
                            index += 1;
                        }
                        else
                        {
                            currentArg.Append(args[index]);
                            index += 1;
                        }
                    }
                    while (!char.IsWhiteSpace(args[index]) || inQuotes);
                    argArray.Add(currentArg.ToString());
                    currentArg.Length = 0;
                }
            }
            catch (IndexOutOfRangeException)
            {
                // got EOF 
                if (inQuotes)
                {
                    reporter(
                            string.Format(
                                    "Error: Unbalanced '\"' in command line argument file '{0}'",
                                    fileName));
                    hadError = true;
                }
                else if (currentArg.Length > 0)
                {
                    // valid argument can be terminated by EOF
                    argArray.Add(currentArg.ToString());
                }
            }

            arguments = (string[]) argArray.ToArray(typeof (string));
            return hadError;
        }

        private static string LongName(ArgumentAttribute attribute, FieldInfo field)
        {
            return (attribute == null || attribute.DefaultLongName)
                           ? field.Name
                           : attribute.LongName;
        }

        private static string ShortName(ArgumentAttribute attribute, FieldInfo field)
        {
            if (attribute is DefaultArgumentAttribute)
            {
                return null;
            }
            if (!ExplicitShortName(attribute))
            {
                return LongName(attribute, field).Substring(0, 1);
            }
            return attribute.ShortName;
        }

        private static string HelpText(ArgumentAttribute attribute, FieldInfo field)
        {
            if (attribute == null)
            {
                return null;
            }
            else
            {
                return attribute.HelpText;
            }
        }

        private static bool HasHelpText(ArgumentAttribute attribute)
        {
            return (attribute != null && attribute.HasHelpText);
        }

        private static bool ExplicitShortName(ArgumentAttribute attribute)
        {
            return (attribute != null && !attribute.DefaultShortName);
        }

        private static object DefaultValue(ArgumentAttribute attribute, FieldInfo field)
        {
            return (attribute == null || !attribute.HasDefaultValue) ? null : attribute.DefaultValue;
        }

        private static Type ElementType(FieldInfo field)
        {
            if (IsCollectionType(field.FieldType))
            {
                return field.FieldType.GetElementType();
            }
            else
            {
                return null;
            }
        }

        private static ArgumentTypes Flags(ArgumentAttribute attribute, FieldInfo field)
        {
            if (attribute != null)
            {
                return attribute.Type;
            }
            else if (IsCollectionType(field.FieldType))
            {
                return ArgumentTypes.MultipleUnique;
            }
            else
            {
                return ArgumentTypes.AtMostOnce;
            }
        }

        private static bool IsCollectionType(Type type)
        {
            return type.IsArray;
        }

        private static bool IsValidElementType(Type type)
        {
            return type != null &&
                   (type == typeof (int) || type == typeof (uint) || type == typeof (string) ||
                    type == typeof (bool) || type.IsEnum);
        }

        private class Argument
        {
            public Argument(ArgumentAttribute attribute, FieldInfo field, ReportError reporter)
            {
                longName = Parser.LongName(attribute, field);
                explicitShortName = Parser.ExplicitShortName(attribute);
                shortName = Parser.ShortName(attribute, field);
                hasHelpText = Parser.HasHelpText(attribute);
                helpText = Parser.HelpText(attribute, field);
                defaultValue = Parser.DefaultValue(attribute, field);
                elementType = ElementType(field);
                flags = Flags(attribute, field);
                this.field = field;
                seenValue = false;
                this.reporter = reporter;
                isDefault = attribute != null && attribute is DefaultArgumentAttribute;

                if (IsCollection)
                {
                    collectionValues = new ArrayList();
                }

                Debug.Assert(longName != null && longName.Length > 0);
                Debug.Assert(!isDefault || !ExplicitShortName);
                Debug.Assert(!IsCollection || AllowMultiple,
                             "Collection arguments must have allow multiple");
                Debug.Assert(!Unique || IsCollection,
                             "Unique only applicable to collection arguments");
                Debug.Assert(IsValidElementType(Type) || IsCollectionType(Type));
                Debug.Assert((IsCollection && IsValidElementType(elementType)) ||
                             (!IsCollection && elementType == null));
                Debug.Assert(!(IsRequired && HasDefaultValue),
                             "Required arguments cannot have default value");
                Debug.Assert(!HasDefaultValue || (defaultValue.GetType() == field.FieldType),
                             "Type of default value must match field type");
            }

            public bool Finish(object destination)
            {
                if (!SeenValue && HasDefaultValue)
                {
                    field.SetValue(destination, DefaultValue);
                }
                if (IsCollection)
                {
                    field.SetValue(destination, collectionValues.ToArray(elementType));
                }

                return ReportMissingRequiredArgument();
            }

            private bool ReportMissingRequiredArgument()
            {
                if (IsRequired && !SeenValue)
                {
                    if (IsDefault)
                    {
                        reporter(string.Format("Missing required argument '<{0}>'.", LongName));
                    }
                    else
                    {
                        reporter(string.Format("Missing required argument '/{0}'.", LongName));
                    }
                    return true;
                }
                return false;
            }

            private void ReportDuplicateArgumentValue(string value)
            {
                reporter(string.Format("Duplicate '{0}' argument '{1}'", LongName, value));
            }

            public bool SetValue(string value, object destination)
            {
                if (SeenValue && !AllowMultiple)
                {
                    reporter(string.Format("Duplicate '{0}' argument", LongName));
                    return false;
                }
                seenValue = true;

                object newValue;
                if (!ParseValue(ValueType, value, out newValue))
                {
                    return false;
                }
                if (IsCollection)
                {
                    if (Unique && collectionValues.Contains(newValue))
                    {
                        ReportDuplicateArgumentValue(value);
                        return false;
                    }
                    else
                    {
                        collectionValues.Add(newValue);
                    }
                }
                else
                {
                    field.SetValue(destination, newValue);
                }

                return true;
            }

            public Type ValueType
            {
                get { return IsCollection ? elementType : Type; }
            }

            private void ReportBadArgumentValue(string value)
            {
                reporter(
                        string.Format(
                                "'{0}' is not a valid value for the '{1}' command line option",
                                value,
                                LongName));
            }

            private bool ParseValue(Type type, string stringData, out object value)
            {
                // null is only valid for bool variables
                // empty string is never valid
                if ((stringData != null || type == typeof (bool)) &&
                    (stringData == null || stringData.Length > 0))
                {
                    try
                    {
                        if (type == typeof (string))
                        {
                            value = stringData;
                            return true;
                        }
                        else if (type == typeof (bool))
                        {
                            if (stringData == null || stringData == "+")
                            {
                                value = true;
                                return true;
                            }
                            else if (stringData == "-")
                            {
                                value = false;
                                return true;
                            }
                        }
                        else if (type == typeof (int))
                        {
                            value = int.Parse(stringData);
                            return true;
                        }
                        else if (type == typeof (uint))
                        {
                            value = int.Parse(stringData);
                            return true;
                        }
                        else
                        {
                            Debug.Assert(type.IsEnum);
                            value = Enum.Parse(type, stringData, true);
                            return true;
                        }
                    }
                    catch
                    {
                        // catch parse errors
                    }
                }

                ReportBadArgumentValue(stringData);
                value = null;
                return false;
            }

            private void AppendValue(StringBuilder builder, object value)
            {
                if (value is string || value is int || value is uint || value.GetType().IsEnum)
                {
                    builder.Append(value.ToString());
                }
                else if (value is bool)
                {
                    builder.Append((bool) value ? "+" : "-");
                }
                else
                {
                    bool first = true;
                    foreach (object o in (Array) value)
                    {
                        if (!first)
                        {
                            builder.Append(", ");
                        }
                        AppendValue(builder, o);
                        first = false;
                    }
                }
            }

            public string LongName
            {
                get { return longName; }
            }

            public bool ExplicitShortName
            {
                get { return explicitShortName; }
            }

            public string ShortName
            {
                get { return shortName; }
            }

            public bool HasShortName
            {
                get { return shortName != null; }
            }

            public void ClearShortName()
            {
                shortName = null;
            }

            public bool HasHelpText
            {
                get { return hasHelpText; }
            }

            public string HelpText
            {
                get { return helpText; }
            }

            public object DefaultValue
            {
                get { return defaultValue; }
            }

            public bool HasDefaultValue
            {
                get { return null != defaultValue; }
            }

            public string FullHelpText
            {
                get
                {
                    StringBuilder builder = new StringBuilder();
                    if (HasHelpText)
                    {
                        builder.Append(HelpText);
                    }
                    if (HasDefaultValue)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append(" ");
                        }
                        builder.Append("Default value:'");
                        AppendValue(builder, DefaultValue);
                        builder.Append('\'');
                    }
                    if (HasShortName)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append(" ");
                        }
                        builder.Append("(short form /");
                        builder.Append(ShortName);
                        builder.Append(")");
                    }
                    return builder.ToString();
                }
            }

            public string SyntaxHelp
            {
                get
                {
                    StringBuilder builder = new StringBuilder();

                    if (IsDefault)
                    {
                        builder.Append("<");
                        builder.Append(LongName);
                        builder.Append(">");
                    }
                    else
                    {
                        builder.Append("/");
                        builder.Append(LongName);
                        Type valueType = ValueType;
                        if (valueType == typeof (int))
                        {
                            builder.Append(":<int>");
                        }
                        else if (valueType == typeof (uint))
                        {
                            builder.Append(":<uint>");
                        }
                        else if (valueType == typeof (bool))
                        {
                            builder.Append("[+|-]");
                        }
                        else if (valueType == typeof (string))
                        {
                            builder.Append(":<string>");
                        }
                        else
                        {
                            Debug.Assert(valueType.IsEnum);

                            builder.Append(":{");
                            bool first = true;
                            foreach (FieldInfo field in valueType.GetFields())
                            {
                                if (field.IsStatic)
                                {
                                    if (first)
                                    {
                                        first = false;
                                    }
                                    else
                                    {
                                        builder.Append('|');
                                    }
                                    builder.Append(field.Name);
                                }
                            }
                            builder.Append('}');
                        }
                    }

                    return builder.ToString();
                }
            }

            public bool IsRequired
            {
                get { return 0 != (flags & ArgumentTypes.Required); }
            }

            public bool SeenValue
            {
                get { return seenValue; }
            }

            public bool AllowMultiple
            {
                get { return 0 != (flags & ArgumentTypes.Multiple); }
            }

            public bool Unique
            {
                get { return 0 != (flags & ArgumentTypes.Unique); }
            }

            public Type Type
            {
                get { return field.FieldType; }
            }

            public bool IsCollection
            {
                get { return IsCollectionType(Type); }
            }

            public bool IsDefault
            {
                get { return isDefault; }
            }

            private readonly string longName;
            private string shortName;
            private readonly string helpText;
            private readonly bool hasHelpText;
            private readonly bool explicitShortName;
            private readonly object defaultValue;
            private bool seenValue;
            private readonly FieldInfo field;
            private readonly Type elementType;
            private readonly ArgumentTypes flags;
            private readonly ArrayList collectionValues;
            private readonly ReportError reporter;
            private readonly bool isDefault;
        }

        private readonly ArrayList arguments;
        private readonly Hashtable argumentMap;
        private readonly Argument defaultArgument;
        private readonly ReportError reporter;
    }
}