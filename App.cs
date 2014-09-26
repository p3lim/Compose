using Microsoft.Win32;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WindowsInput;

namespace Compose
{
	public class App : Form
	{
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new App());
		}

		private KeyboardHookListener keyboardHook;
		private InputSimulator simulator;

		public int composeIndex = 0;
		public String composeString = "";
		public bool isComposing;

		public Dictionary<int, char> lowerCase;
		public Dictionary<int, char> upperCase;
		public Dictionary<string, char> combinations;

		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;

		public App()
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				string resourceName = new AssemblyName(args.Name).Name + ".dll";
				string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
				{
					Byte[] assemblyData = new Byte[stream.Length];
					stream.Read(assemblyData, 0, assemblyData.Length);
					return Assembly.Load(assemblyData);
				}
			};

			InititalizeComponent();
		}

		private void InititalizeComponent()
		{
			#region Keys to lowercase character
			lowerCase = new Dictionary<int, char>();
			lowerCase.Add(65, 'a');
			lowerCase.Add(66, 'b');
			lowerCase.Add(67, 'c');
			lowerCase.Add(68, 'd');
			lowerCase.Add(69, 'e');
			lowerCase.Add(70, 'f');
			lowerCase.Add(71, 'g');
			lowerCase.Add(72, 'h');
			lowerCase.Add(73, 'i');
			lowerCase.Add(74, 'j');
			lowerCase.Add(75, 'k');
			lowerCase.Add(76, 'l');
			lowerCase.Add(77, 'm');
			lowerCase.Add(78, 'n');
			lowerCase.Add(79, 'o');
			lowerCase.Add(80, 'p');
			lowerCase.Add(82, 'r');
			lowerCase.Add(83, 's');
			lowerCase.Add(84, 't');
			lowerCase.Add(85, 'u');
			lowerCase.Add(86, 'v');
			lowerCase.Add(87, 'w');
			lowerCase.Add(88, 'x');
			lowerCase.Add(89, 'y');
			lowerCase.Add(90, 'z');
			lowerCase.Add(49, '1');
			lowerCase.Add(50, '2');
			lowerCase.Add(51, '3');
			lowerCase.Add(52, '4');
			lowerCase.Add(53, '5');
			lowerCase.Add(54, '6');
			lowerCase.Add(55, '7');
			lowerCase.Add(56, '8');
			lowerCase.Add(57, '9');
			lowerCase.Add(48, '0');
			lowerCase.Add(189, '-');
			lowerCase.Add(187, '=');
			lowerCase.Add(186, ';');
			lowerCase.Add(222, '\'');
			lowerCase.Add(220, '\\');
			lowerCase.Add(188, ',');
			lowerCase.Add(190, '.');
			lowerCase.Add(191, '/');
			lowerCase.Add(192, '`');
			#endregion

			#region Keys to uppercase character
			upperCase = new Dictionary<int, char>();
			upperCase.Add(65, 'A');
			upperCase.Add(66, 'B');
			upperCase.Add(67, 'C');
			upperCase.Add(68, 'D');
			upperCase.Add(69, 'E');
			upperCase.Add(70, 'F');
			upperCase.Add(71, 'G');
			upperCase.Add(72, 'H');
			upperCase.Add(73, 'I');
			upperCase.Add(74, 'J');
			upperCase.Add(75, 'K');
			upperCase.Add(76, 'L');
			upperCase.Add(77, 'M');
			upperCase.Add(78, 'N');
			upperCase.Add(79, 'O');
			upperCase.Add(80, 'P');
			upperCase.Add(82, 'R');
			upperCase.Add(83, 'S');
			upperCase.Add(84, 'T');
			upperCase.Add(85, 'U');
			upperCase.Add(86, 'V');
			upperCase.Add(87, 'W');
			upperCase.Add(88, 'X');
			upperCase.Add(89, 'Y');
			upperCase.Add(90, 'Z');
			upperCase.Add(49, '!');
			upperCase.Add(53, '%');
			upperCase.Add(54, '^');
			upperCase.Add(57, '(');
			upperCase.Add(48, ')');
			upperCase.Add(189, '_');
			upperCase.Add(187, '+');
			upperCase.Add(186, ':');
			upperCase.Add(222, '"');
			upperCase.Add(220, '|');
			upperCase.Add(188, '<');
			upperCase.Add(190, '>');
			upperCase.Add(191, '?');
			upperCase.Add(192, '~');
			#endregion

			#region Compose combinations to unicode character
			combinations = new Dictionary<string, char>();
			combinations.Add("!!", '\u00a1'); // Inverted exclamation mark
			combinations.Add("??", '\u00bf'); // Inverted question mark
			combinations.Add("!^", '\u00a6'); // Broken bar
			combinations.Add("<<", '\u00ab'); // Left-pointing double angle quotation mark
			combinations.Add(">>", '\u00bb'); // Right-pointing double angle quotation mark
			combinations.Add("<'", '\u2018'); // Left single quotation mark
			combinations.Add("'<", '\u2018'); // Left single quotation mark
			combinations.Add(">'", '\u2019'); // Right single quotation mark
			combinations.Add("'>", '\u2019'); // Right single quotation mark
			combinations.Add("<\"", '\u201c'); // Left double quotation mark
			combinations.Add("\">", '\u201d'); // Right double quotation mark
			combinations.Add("|c", '\u00a2'); // Cent sign
			combinations.Add("c|", '\u00a2'); // Cent sign
			combinations.Add("/c", '\u00a2'); // Cent sign
			combinations.Add("c/", '\u00a2'); // Cent sign
			combinations.Add("L-", '\u00a3'); // Pound sign
			combinations.Add("-L", '\u00a3'); // Pound sign
			combinations.Add("ox", '\u00a4'); // Currency sign
			combinations.Add("xo", '\u00a4'); // Currency sign
			combinations.Add("Y=", '\u00a5'); // Yen sign
			combinations.Add("=Y", '\u00a5'); // Yen sign
			combinations.Add("Fr", '\u20a3'); // Franc sign
			combinations.Add("L=", '\u20a4'); // Lira sign
			combinations.Add("=L", '\u20a4'); // Lira sign
			combinations.Add("Pt", '\u20a7'); // Peseta sign
			combinations.Add("Rs", '\u20a8'); // Rupee sign
			combinations.Add("W=", '\u20a9'); // Won sign
			combinations.Add("=W", '\u20a9'); // Won sign
			combinations.Add("C=", '\u20ac'); // Euro sign
			combinations.Add("=C", '\u20ac'); // Euro sign
			combinations.Add("c=", '\u20ac'); // Euro sign
			combinations.Add("=c", '\u20ac'); // Euro sign
			combinations.Add("E=", '\u20ac'); // Euro sign
			combinations.Add("=E", '\u20ac'); // Euro sign
			combinations.Add("so", '\u00a7'); // Section sign
			combinations.Add("os", '\u00a7'); // Section sign
			combinations.Add("oc", '\u00a9'); // Copyright sign
			combinations.Add("oC", '\u00a9'); // Copyright sign
			combinations.Add("Oc", '\u00a9'); // Copyright sign
			combinations.Add("OC", '\u00a9'); // Copyright sign
			combinations.Add("or", '\u00ae'); // Registered sign
			combinations.Add("oR", '\u00ae'); // Registered sign
			combinations.Add("Or", '\u00ae'); // Registered sign
			combinations.Add("OR", '\u00ae'); // Registered sign
			combinations.Add("p!", '\u00b6'); // Pilcrow sign (paragraph)
			combinations.Add("P!", '\u00b6'); // Pilcrow sign (paragraph)
			combinations.Add("PP", '\u00b6'); // Pilcrow sign (paragraph)
			combinations.Add("sm", '\u2120'); // Service mark
			combinations.Add("sM", '\u2120'); // Service mark
			combinations.Add("Sm", '\u2120'); // Service mark
			combinations.Add("SM", '\u2120'); // Service mark
			combinations.Add("tm", '\u2122'); // Trade mark
			combinations.Add("tM", '\u2122'); // Trade mark
			combinations.Add("Tm", '\u2122'); // Trade mark
			combinations.Add("TM", '\u2122'); // Trade mark
			combinations.Add("oo", '\u00b0'); // Degree sign
			combinations.Add(",-", '\u00ac'); // Not sign
			combinations.Add("-,", '\u00ac'); // Not sign
			combinations.Add("+-", '\u00b1'); // Plus-minus sign
			combinations.Add("xx", '\u00d7'); // Multiplication sign
			combinations.Add(":-", '\u00f7'); // Division sign
			combinations.Add("-:", '\u00f7'); // Division sign
			combinations.Add("%o", '\u2030'); // Per mille sign
			combinations.Add("%0", '\u2030'); // Per mille sign
			combinations.Add("mu", '\u00b5'); // Micro sign
			combinations.Add("pi", '\u03c0'); // Pi sign
			combinations.Add("14", '\u00bc'); // Fraction (quarter)
			combinations.Add("12", '\u00bd'); // Fraction (half)
			combinations.Add("34", '\u00be'); // Fraction (three quarters)
			combinations.Add("^0", '\u2070'); // Superscript zero
			combinations.Add("^1", '\u00b9'); // Superscript one
			combinations.Add("^2", '\u00b2'); // Superscript two
			combinations.Add("^3", '\u00b3'); // Superscript three
			combinations.Add("^4", '\u2074'); // Superscript four
			combinations.Add("^5", '\u2075'); // Superscript five
			combinations.Add("^6", '\u2076'); // Superscript six
			combinations.Add("^7", '\u2077'); // Superscript seven
			combinations.Add("^8", '\u2078'); // Superscript eight
			combinations.Add("^9", '\u2079'); // Superscript nine
			combinations.Add("^+", '\u207a'); // Superscript plus sign
			combinations.Add("^=", '\u207c'); // Superscript equals sign
			combinations.Add("^(", '\u207d'); // Superscript left parenthesis
			combinations.Add("^)", '\u207e'); // Superscript right parenthesis
			combinations.Add("^n", '\u207f'); // Superscript letter n
			combinations.Add("_0", '\u2080'); // Subscript zero
			combinations.Add("_1", '\u2081'); // Subscript one
			combinations.Add("_2", '\u2082'); // Subscript two
			combinations.Add("_3", '\u2083'); // Subscript three
			combinations.Add("_4", '\u2084'); // Subscript four
			combinations.Add("_5", '\u2085'); // Subscript five
			combinations.Add("_6", '\u2086'); // Subscript six
			combinations.Add("_7", '\u2087'); // Subscript seven
			combinations.Add("_8", '\u2088'); // Subscript eight
			combinations.Add("_9", '\u2089'); // Subscript nine
			combinations.Add("_+", '\u208a'); // Subscript plus sign
			combinations.Add("_=", '\u208c'); // Subscript equals sign
			combinations.Add("_(", '\u208d'); // Subscript left parenthesis
			combinations.Add("_)", '\u208e'); // Subscript right parenthesis

			combinations.Add("`A", '\u00c0'); // Latin capital letter A with grave
			combinations.Add("'A", '\u00c1'); // Latin capital letter A with acute
			combinations.Add("^A", '\u00c2'); // Latin capital letter A with circumflex
			combinations.Add("~A", '\u00c3'); // Latin capital letter A with tilde
			combinations.Add("\"A", '\u00c4'); // Latin capital letter A with diaeresis
			combinations.Add("oA", '\u00c5'); // Latin capital letter A with ring above
			combinations.Add("AE", '\u00c6'); // Latin capital letter AE
			combinations.Add(",C", '\u00c7'); // Latin capital letter C with cedilla
			combinations.Add("`E", '\u00c8'); // Latin capital letter E with grave
			combinations.Add("'E", '\u00c9'); // Latin capital letter E with acute
			combinations.Add("^E", '\u00ca'); // Latin capital letter E with circumflex
			combinations.Add("\"E", '\u00cb'); // Latin capital letter E with diaeresis
			combinations.Add("`I", '\u00cc'); // Latin capital letter I with grave
			combinations.Add("'I", '\u00cd'); // Latin capital letter I with acute
			combinations.Add("^I", '\u00ce'); // Latin capital letter I with circumflex
			combinations.Add("\"I", '\u00cf'); // Latin capital letter I with diaeresis
			combinations.Add("DH", '\u00d0'); // Latin capital letter Eth
			combinations.Add("~N", '\u00d1'); // Latin capital letter N with tilde
			combinations.Add("`O", '\u00d2'); // Latin capital letter O with grave
			combinations.Add("'O", '\u00d3'); // Latin capital letter O with acute
			combinations.Add("^O", '\u00d4'); // Latin capital letter O with circumflex
			combinations.Add("~O", '\u00d5'); // Latin capital letter O with tilde
			combinations.Add("\"O", '\u00d6'); // Latin capital letter O with diaeresis
			combinations.Add("/O", '\u00d8'); // Latin capital letter O with stroke
			combinations.Add("`U", '\u00d9'); // Latin capital letter U with grave
			combinations.Add("'U", '\u00da'); // Latin capital letter U with acute
			combinations.Add("^U", '\u00db'); // Latin capital letter U with circumflex
			combinations.Add("\"U", '\u00dc'); // Latin capital letter U with diaeresis
			combinations.Add("'Y", '\u00dd'); // Latin capital letter Y with acute
			combinations.Add("TH", '\u00de'); // Latin capital letter Thorn
			combinations.Add("ss", '\u00df'); // Latin small letter sharp S
			combinations.Add("`a", '\u00e0'); // Latin small letter A with grave
			combinations.Add("'a", '\u00e1'); // Latin small letter A with acute
			combinations.Add("^a", '\u00e2'); // Latin small letter A with circumflex
			combinations.Add("~a", '\u00e3'); // Latin small letter A with tilde
			combinations.Add("\"a", '\u00e4'); // Latin small letter A with diaeresis
			combinations.Add("oa", '\u00e5'); // Latin small letter A with ring above
			combinations.Add("ae", '\u00e6'); // Latin small letter AE
			combinations.Add(",c", '\u00e7'); // Latin small letter C with cedilla
			combinations.Add("`e", '\u00e8'); // Latin small letter E with grave
			combinations.Add("'e", '\u00e9'); // Latin small letter E with acute
			combinations.Add("^e", '\u00ea'); // Latin small letter E with circumflex
			combinations.Add("\"e", '\u00eb'); // Latin small letter E with diaeresis
			combinations.Add("`i", '\u00ec'); // Latin small letter I with grave
			combinations.Add("'i", '\u00ed'); // Latin small letter I with acute
			combinations.Add("^i", '\u00ee'); // Latin small letter I with circumflex
			combinations.Add("\"i", '\u00ef'); // Latin small letter I with diaeresis
			combinations.Add("dh", '\u00f0'); // Latin small letter Eth
			combinations.Add("~n", '\u00f1'); // Latin small letter N with tilde
			combinations.Add("`o", '\u00f2'); // Latin small letter O with grave
			combinations.Add("'o", '\u00f3'); // Latin small letter O with acute
			combinations.Add("^o", '\u00f4'); // Latin small letter O with circumflex
			combinations.Add("~o", '\u00f5'); // Latin small letter O with tilde
			combinations.Add("\"o", '\u00f6'); // Latin small letter O with diaeresis
			combinations.Add("/o", '\u00f8'); // Latin small letter O with stroke
			combinations.Add("`u", '\u00f9'); // Latin small letter U with grave
			combinations.Add("'u", '\u00fa'); // Latin small letter U with acute
			combinations.Add("^u", '\u00fb'); // Latin small letter U with circumflex
			combinations.Add("\"u", '\u00fc'); // Latin small letter U with diaeresis
			combinations.Add("'y", '\u00fd'); // Latin small letter Y with acute
			combinations.Add("th", '\u00fe'); // Latin small letter Thorn
			combinations.Add("\"y", '\u00ff'); // Latin small letter Y with diaeresis
			combinations.Add("_A", '\u0100'); // Latin capital letter A with macron
			combinations.Add("_a", '\u0101'); // Latin small letter A with macron
			combinations.Add("UA", '\u0102'); // Latin capital letter A with breve
			combinations.Add("bA", '\u0102'); // Latin capital letter A with breve
			combinations.Add("Ua", '\u0103'); // Latin small letter A with breve
			combinations.Add("ba", '\u0103'); // Latin small letter A with breve
			combinations.Add(";A", '\u0104'); // Latin capital letter A with ogonek
			combinations.Add(";a", '\u0105'); // Latin small letter A with ogonek
			combinations.Add("'C", '\u0106'); // Latin capital letter C with acute
			combinations.Add("'c", '\u0107'); // Latin small letter C with acute
			combinations.Add("^C", '\u0108'); // Latin capital letter C with circumflex
			combinations.Add("^c", '\u0109'); // Latin small letter C with circumflex
			combinations.Add("cC", '\u010c'); // Latin capital letter C with caron
			combinations.Add("cc", '\u010d'); // Latin small letter C with caron
			combinations.Add("cD", '\u010e'); // Latin capital letter D with caron
			combinations.Add("cd", '\u010f'); // Latin small letter D with caron
			combinations.Add("-D", '\u0110'); // Latin capital letter D with stroke
			combinations.Add("/D", '\u0110'); // Latin capital letter D with stroke
			combinations.Add("-d", '\u0111'); // Latin small letter D with stroke
			combinations.Add("/d", '\u0111'); // Latin small letter D with stroke
			combinations.Add("_E", '\u0112'); // Latin capital letter E with macron
			combinations.Add("_e", '\u0113'); // Latin small letter E with macron
			combinations.Add("UE", '\u0114'); // Latin capital letter E with breve
			combinations.Add("bE", '\u0114'); // Latin capital letter E with breve
			combinations.Add("Ue", '\u0115'); // Latin small letter E with breve
			combinations.Add("be", '\u0115'); // Latin small letter E with breve
			combinations.Add(";E", '\u0118'); // Latin capital letter E with ogonek
			combinations.Add(";e", '\u0119'); // Latin small letter E with ogonek
			combinations.Add("cE", '\u011a'); // Latin capital letter E with caron
			combinations.Add("ce", '\u011b'); // Latin small letter E with caron
			combinations.Add("^G", '\u011c'); // Latin capital letter G with circumflex
			combinations.Add("^g", '\u011d'); // Latin small letter G with circumflex
			combinations.Add("UG", '\u011e'); // Latin capital letter G with breve
			combinations.Add("bG", '\u011e'); // Latin capital letter G with breve
			combinations.Add("Ug", '\u011f'); // Latin small letter G with breve
			combinations.Add("bg", '\u011f'); // Latin small letter G with breve
			combinations.Add(",G", '\u0122'); // Latin capital letter G with cedilla
			combinations.Add(",g", '\u0123'); // Latin small letter G with cedilla
			combinations.Add("^H", '\u0124'); // Latin capital letter H with circumflex
			combinations.Add("^h", '\u0125'); // Latin small letter H with circumflex
			combinations.Add("/H", '\u0126'); // Latin capital letter H with stroke
			combinations.Add("/h", '\u0127'); // Latin small letter H with stroke
			combinations.Add("~I", '\u0128'); // Latin capital letter I with tilde
			combinations.Add("~i", '\u0129'); // Latin small letter I with tilde
			combinations.Add("_I", '\u012a'); // Latin capital letter I with macron
			combinations.Add("_i", '\u012b'); // Latin small letter I with macron
			combinations.Add("UI", '\u012c'); // Latin capital letter I with breve
			combinations.Add("bI", '\u012c'); // Latin capital letter I with breve
			combinations.Add("Ui", '\u012d'); // Latin small letter I with breve
			combinations.Add("bi", '\u012d'); // Latin small letter I with breve
			combinations.Add(";I", '\u012e'); // Latin capital letter I with ogonek
			combinations.Add(";i", '\u012f'); // Latin small letter I with ogonek
			combinations.Add("i.", '\u0131'); // Latin small letter dotless I
			combinations.Add("^J", '\u0134'); // Latin capital letter j with circumflex
			combinations.Add("^j", '\u0135'); // Latin small letter j with circumflex
			combinations.Add(",K", '\u0136'); // Latin capital letter K with cedilla
			combinations.Add(",k", '\u0137'); // Latin small letter K with cedilla
			combinations.Add("kk", '\u0138'); // Latin small letter Kra
			combinations.Add("'L", '\u0139'); // Latin capital letter l with acute
			combinations.Add("'l", '\u013a'); // Latin small letter l with acute
			combinations.Add(",L", '\u013b'); // Latin capital letter l with cedilla
			combinations.Add(",l", '\u013c'); // Latin small letter l with cedilla
			combinations.Add("cL", '\u013d'); // Latin capital letter l with caron
			combinations.Add("cl", '\u013e'); // Latin small letter l with caron
			combinations.Add("/L", '\u0141'); // Latin capital letter l with stroke
			combinations.Add("/l", '\u0142'); // Latin small letter l with stroke
			combinations.Add("'N", '\u0143'); // Latin capital letter N with acute
			combinations.Add("'n", '\u0144'); // Latin small letter N with acute
			combinations.Add(",N", '\u0145'); // Latin capital letter N with cedilla
			combinations.Add(",n", '\u0146'); // Latin small letter N with cedilla
			combinations.Add("cN", '\u0147'); // Latin capital letter N with caron
			combinations.Add("cn", '\u0148'); // Latin small letter N with caron
			combinations.Add("NG", '\u014a'); // Latin capital letter Eng
			combinations.Add("ng", '\u014b'); // Latin small letter Eng
			combinations.Add("_O", '\u014c'); // Latin capital letter O with macron
			combinations.Add("_o", '\u014d'); // Latin small letter O with macron
			combinations.Add("UO", '\u014e'); // Latin capital letter O with breve
			combinations.Add("bO", '\u014e'); // Latin capital letter O with breve
			combinations.Add("Uo", '\u014f'); // Latin small letter O with breve
			combinations.Add("bo", '\u014f'); // Latin small letter O with breve
			combinations.Add("=O", '\u0150'); // Latin capital letter O with double acute
			combinations.Add("=o", '\u0151'); // Latin small letter O with double acute
			combinations.Add("OE", '\u0152'); // Latin capital ligature OE
			combinations.Add("oe", '\u0153'); // Latin small ligature OE
			combinations.Add("'R", '\u0154'); // Latin capital letter R with acute
			combinations.Add("'r", '\u0155'); // Latin small letter R with acute
			combinations.Add(",R", '\u0156'); // Latin capital letter R with cedilla
			combinations.Add(",r", '\u0157'); // Latin small letter R with cedilla
			combinations.Add("cR", '\u0158'); // Latin capital letter R with caron
			combinations.Add("cr", '\u0159'); // Latin small letter R with caron
			combinations.Add("'S", '\u015a'); // Latin capital letter S with acute
			combinations.Add("'s", '\u015b'); // Latin small letter S with acute
			combinations.Add("^S", '\u015c'); // Latin capital letter S with circumflex
			combinations.Add("^s", '\u015d'); // Latin small letter S with circumflex
			combinations.Add(",S", '\u015e'); // Latin capital letter S with cedilla
			combinations.Add(",s", '\u015f'); // Latin small letter S with cedilla
			combinations.Add("cS", '\u0160'); // Latin capital letter S with caron
			combinations.Add("cs", '\u0161'); // Latin small letter S with caron
			combinations.Add(",T", '\u0162'); // Latin capital letter T with cedilla
			combinations.Add(",t", '\u0163'); // Latin small letter T with cedilla
			combinations.Add("cT", '\u0164'); // Latin capital letter T with caron
			combinations.Add("ct", '\u0165'); // Latin small letter T with caron
			combinations.Add("/T", '\u0166'); // Latin capital letter T with stroke
			combinations.Add("/t", '\u0167'); // Latin small letter T with stroke
			combinations.Add("~U", '\u0168'); // Latin capital letter U with tilde
			combinations.Add("~u", '\u0169'); // Latin small letter U with tilde
			combinations.Add("_U", '\u016a'); // Latin capital letter U with macron
			combinations.Add("_u", '\u016b'); // Latin small letter U with macron
			combinations.Add("UU", '\u016c'); // Latin capital letter U with breve
			combinations.Add("bU", '\u016c'); // Latin capital letter U with breve
			combinations.Add("Uu", '\u016d'); // Latin small letter U with breve
			combinations.Add("bu", '\u016d'); // Latin small letter U with breve
			combinations.Add("oU", '\u016e'); // Latin capital letter U with ring above
			combinations.Add("ou", '\u016f'); // Latin small letter U with ring above
			combinations.Add("=U", '\u0170'); // Latin capital letter U with double acute
			combinations.Add("=u", '\u0171'); // Latin small letter U with double acute
			combinations.Add(";U", '\u0172'); // Latin capital letter U with ogonek
			combinations.Add(";u", '\u0173'); // Latin small letter U with ogonek
			combinations.Add("^W", '\u0174'); // Latin capital letter W with circumflex
			combinations.Add("^w", '\u0175'); // Latin small letter W with circumflex
			combinations.Add("^Y", '\u0176'); // Latin capital letter Y with circumflex
			combinations.Add("^y", '\u0177'); // Latin small letter Y with circumflex
			combinations.Add("\"Y", '\u0178'); // Latin capital letter Y with diaeresis
			combinations.Add("'Z", '\u0179'); // Latin capital letter Z with acute
			combinations.Add("'z", '\u017a'); // Latin small letter Z with acute
			combinations.Add("cZ", '\u017d'); // Latin capital letter Z with caron
			combinations.Add("cz", '\u017e'); // Latin small letter Z with caron
			combinations.Add("fs", '\u017f'); // Latin small letter long S
			combinations.Add("fS", '\u017f'); // Latin small letter long S
			combinations.Add("/b", '\u0180'); // Latin small letter B with stroke
			combinations.Add("/I", '\u0197'); // Latin capital letter I with stroke
			combinations.Add("/Z", '\u01b5'); // Latin capital letter Z with stroke
			combinations.Add("/z", '\u01b6'); // Latin small letter Z with stroke
			combinations.Add("cA", '\u01cd'); // Latin capital letter A with caron
			combinations.Add("ca", '\u01ce'); // Latin small letter A with caron
			combinations.Add("cI", '\u01cf'); // Latin capital letter I with caron
			combinations.Add("ci", '\u01d0'); // Latin small letter I with caron
			combinations.Add("cO", '\u01d1'); // Latin capital letter O with caron
			combinations.Add("co", '\u01d2'); // Latin small letter O with caron
			combinations.Add("cU", '\u01d3'); // Latin capital letter U with caron
			combinations.Add("cu", '\u01d4'); // Latin small letter U with caron
			combinations.Add("/G", '\u01e4'); // Latin capital letter G with stroke
			combinations.Add("/g", '\u01e5'); // Latin small letter G with stroke
			combinations.Add("cG", '\u01e6'); // Latin capital letter G with caron
			combinations.Add("cg", '\u01e7'); // Latin small letter G with caron
			combinations.Add("cK", '\u01e8'); // Latin capital letter K with caron
			combinations.Add("ck", '\u01e9'); // Latin small letter K with caron
			combinations.Add(";O", '\u01ea'); // Latin capital letter O with ogonek
			combinations.Add(";o", '\u01eb'); // Latin small letter O with ogonek
			combinations.Add("cj", '\u01f0'); // Latin small letter j with caron
			combinations.Add("'G", '\u01f4'); // Latin capital letter G with acute
			combinations.Add("'g", '\u01f5'); // Latin small letter G with acute
			combinations.Add("`N", '\u01f8'); // Latin capital letter N with grave
			combinations.Add("`n", '\u01f9'); // Latin small letter N with grave
			combinations.Add("cH", '\u021e'); // Latin capital letter H with caron
			combinations.Add("ch", '\u021f'); // Latin small letter H with caron
			combinations.Add(",E", '\u0228'); // Latin capital letter E with cedilla
			combinations.Add(",e", '\u0229'); // Latin small letter E with cedilla
			combinations.Add("_Y", '\u0232'); // Latin capital letter Y with macron
			combinations.Add("_y", '\u0233'); // Latin small letter Y with macron
			combinations.Add("ee", '\u0259'); // Latin small letter Schwa
			combinations.Add("/i", '\u0268'); // Latin small letter I with stroke
			combinations.Add(",D", '\u1e10'); // Latin capital letter D with cedilla
			combinations.Add(",d", '\u1e11'); // Latin small letter D with cedilla
			combinations.Add("_G", '\u1e20'); // Latin capital letter G with macron
			combinations.Add("_g", '\u1e21'); // Latin small letter G with macron
			combinations.Add("\"H", '\u1e26'); // Latin capital letter H with diaeresis
			combinations.Add("\"h", '\u1e27'); // Latin small letter H with diaeresis
			combinations.Add(",H", '\u1e28'); // Latin capital letter H with cedilla
			combinations.Add(",h", '\u1e29'); // Latin small letter H with cedilla
			combinations.Add("'K", '\u1e30'); // Latin capital letter K with acute
			combinations.Add("'k", '\u1e31'); // Latin small letter K with acute
			combinations.Add("'M", '\u1e3e'); // Latin capital letter M with acute
			combinations.Add("'m", '\u1e3f'); // Latin small letter M with acute
			combinations.Add("'P", '\u1e54'); // Latin capital letter P with acute
			combinations.Add("'p", '\u1e55'); // Latin small letter P with acute
			combinations.Add("~V", '\u1e7c'); // Latin capital letter V with tilde
			combinations.Add("~v", '\u1e7d'); // Latin small letter V with tilde
			combinations.Add("`W", '\u1e80'); // Latin capital letter W with grave
			combinations.Add("`w", '\u1e81'); // Latin small letter W with grave
			combinations.Add("'W", '\u1e82'); // Latin capital letter W with acute
			combinations.Add("'w", '\u1e83'); // Latin small letter W with acute
			combinations.Add("\"W", '\u1e84'); // Latin capital letter W with diaeresis
			combinations.Add("\"w", '\u1e85'); // Latin small letter W with diaeresis
			combinations.Add("\"X", '\u1e8c'); // Latin capital letter X with diaeresis
			combinations.Add("\"x", '\u1e8d'); // Latin small letter X with diaeresis
			combinations.Add("^Z", '\u1e90'); // Latin capital letter Z with circumflex
			combinations.Add("^z", '\u1e91'); // Latin small letter Z with circumflex
			combinations.Add("\"t", '\u1e97'); // Latin small letter T with diaeresis
			combinations.Add("ow", '\u1e98'); // Latin small letter W with ring above
			combinations.Add("oy", '\u1e99'); // Latin small letter Y with ring above
			combinations.Add("~E", '\u1ebc'); // Latin capital letter E with tilde
			combinations.Add("~e", '\u1ebd'); // Latin small letter E with tilde
			combinations.Add("`Y", '\u1ef2'); // Latin capital letter Y with grave
			combinations.Add("`y", '\u1ef3'); // Latin small letter Y with grave
			combinations.Add("~Y", '\u1ef8'); // Latin capital letter Y with tilde
			combinations.Add("~y", '\u1ef9'); // Latin small letter Y with tilde
			#endregion

			keyboardHook = new KeyboardHookListener(new GlobalHooker());
			keyboardHook.Enabled = true;
			keyboardHook.KeyDown += keyboardHook_KeyDown;

			simulator = new InputSimulator();

			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Settings", trayMenu_Settings);
			trayMenu.MenuItems.Add("Exit", trayMenu_Exit);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Compose";
			trayIcon.Icon = Resources.Icon;
			trayIcon.ContextMenu = trayMenu;
			trayIcon.Visible = true;
		}

		private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			if (Register.active)
				return;

			var modifier = (Keys)Settings.GetModifier();
			if (isComposing)
			{
				if (e.KeyCode == modifier || e.KeyCode == Keys.Escape || e.KeyCode == Keys.Back || e.KeyCode == Keys.Enter)
					isComposing = false;
				else
				{
					if (e.KeyCode != Keys.LShiftKey && e.KeyCode != Keys.RShiftKey)
						composeIndex++;
					else
						return;

					var keyCode = (int)e.KeyCode;
					if (e.KeyCode == Keys.Space)
						composeString += " ";
					else if (e.Shift && upperCase.ContainsKey(keyCode))
						composeString += upperCase[keyCode];
					else if (!e.Shift && lowerCase.ContainsKey(keyCode))
						composeString += lowerCase[keyCode];

					if (composeIndex == 2)
					{
						isComposing = false;

						if (combinations.ContainsKey(composeString))
							simulator.Keyboard.TextEntry(combinations[composeString]);
					}
				}

				e.SuppressKeyPress = true;
			}
			else
			{
				if (e.KeyCode == modifier)
				{
					isComposing = true;
					e.SuppressKeyPress = true;

					composeIndex = 0;
					composeString = "";
				}
			}
		}

		private void trayMenu_Settings(object sender, EventArgs e)
		{
			Options options = Application.OpenForms["Options"] as Options;
			if (options != null)
				options.BringToFront();
			else
			{
				var newOptions = new Options();
				newOptions.Show();
			}
		}

		private void trayMenu_Exit(object sender, EventArgs e)
		{
			Application.Exit();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				trayIcon.Dispose();

			base.Dispose(disposing);
		}

		protected override void OnLoad(EventArgs e)
		{
			Visible = false;
			ShowInTaskbar = false;

			base.OnLoad(e);
		}
	}

	public class Settings
	{
		private static String appPath = "HKEY_CURRENT_USER\\Software\\Compose";
		private static String runPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run";

		public static int GetModifier()
		{
			return Convert.ToInt32(Registry.GetValue(appPath, "Modifier", 0));
		}

		public static bool ShouldIndicate()
		{
			return Convert.ToString(Registry.GetValue(appPath, "Indicate", 0)).Length != 0;
		}

		public static bool ShouldAutoRun()
		{
			return Convert.ToString(Registry.GetValue(runPath, "Compose", "")).Length != 0;
		}

		public static void SetModifier(int modifier)
		{
			Registry.SetValue(appPath, "Modifier", modifier, RegistryValueKind.DWord);
		}

		public static void SetShouldIndicate(bool enabled)
		{
			Registry.SetValue(appPath, "Indicate", enabled, RegistryValueKind.DWord);
		}

		public static void SetShouldAutoRun(bool enabled)
		{
			if (enabled)
				Registry.SetValue(runPath, "Compose", Application.ExecutablePath, RegistryValueKind.String);
			else
				Registry.SetValue(runPath, "Compose", "", RegistryValueKind.String);
		}
	}
}
