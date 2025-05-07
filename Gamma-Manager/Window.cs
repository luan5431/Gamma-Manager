using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Gma.System.MouseKeyHook;
using System.Globalization;

namespace Gamma_Manager
{
    public partial class Window : Form
    {
        private IKeyboardMouseEvents _globalHook; // For global keybind detection
        private Dictionary<Keys, string> _keybindProfiles; // Maps keys to profile names
        System.Globalization.CultureInfo customCulture;
        IniFile iniFile;

        List<Display.DisplayInfo> displays = new List<Display.DisplayInfo>();
        int numDisplay = 0;
        Display.DisplayInfo currDisplay;

        List<ToolStripComboBox> toolMonitors = new List<ToolStripComboBox>();
        ToolStripComboBox toolMonitor;

        bool disableChangeFunc = false;

        bool allColors = true;
        bool redColor = false;
        bool greenColor = false;
        bool blueColor = false;

        private void clearColors()
        {
            allColors = false;
            redColor = false;
            greenColor = false;
            blueColor = false;
        }

        private void initPresets()
        {
            comboBoxPresets.Items.Clear();
            comboBoxPresets.Text = string.Empty;
            string[] presets = iniFile.GetSections();
            if (presets != null)
            {
                for (int i = 0; i < presets.Length; i++)
                {
                    if (iniFile.Read("monitor", presets[i]) == currDisplay.displayName)
                    {
                        // Extract the encoded profile name (e.g., "Profile_Key=47")
                        string encodedKeyStr = presets[i].Split(':')[1].Trim();

                        // Decode the profile name (e.g., "Profile_Key=47" -> "/")
                        string keyStr = DecodeProfileName(encodedKeyStr);

                        // Add the decoded profile name to the dropdown
                        comboBoxPresets.Items.Add($"{currDisplay.displayName}: {keyStr}");
                    }
                }
            }
        }
        
        private void DrawLUTGraph()
        {
            // Create a bitmap to draw on
            Bitmap bmp = new Bitmap(picLUTGraph.Width, picLUTGraph.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw axes
                Pen axisPen = new Pen(Color.Black, 1);
                g.DrawLine(axisPen, 20, bmp.Height - 20, bmp.Width - 20, bmp.Height - 20); // X-axis
                g.DrawLine(axisPen, 20, bmp.Height - 20, 20, 20); // Y-axis

                // Draw labels
                Font font = new Font("Arial", 8);
                g.DrawString("Input", font, Brushes.Black, new PointF(bmp.Width - 30, bmp.Height - 20));
                g.DrawString("Output", font, Brushes.Black, new PointF(5, 10));

                // Calculate the gamma curve points
                PointF[] redPoints = new PointF[256];
                PointF[] greenPoints = new PointF[256];
                PointF[] bluePoints = new PointF[256];

                for (int i = 0; i < 256; i++)
                {
                    float x = 20 + (i * (bmp.Width - 40) / 255f);
                    float yRed = bmp.Height - 20 - (CalculateGamma((byte)i, currDisplay.rGamma, currDisplay.rContrast, currDisplay.rBright) * (bmp.Height - 40));
                    float yGreen = bmp.Height - 20 - (CalculateGamma((byte)i, currDisplay.gGamma, currDisplay.gContrast, currDisplay.gBright) * (bmp.Height - 40));
                    float yBlue = bmp.Height - 20 - (CalculateGamma((byte)i, currDisplay.bGamma, currDisplay.bContrast, currDisplay.bBright) * (bmp.Height - 40));

                    redPoints[i] = new PointF(x, yRed);
                    greenPoints[i] = new PointF(x, yGreen);
                    bluePoints[i] = new PointF(x, yBlue);
                }

                // Draw the curves
                g.DrawCurve(new Pen(Color.Red, 2), redPoints);
                g.DrawCurve(new Pen(Color.Green, 2), greenPoints);
                g.DrawCurve(new Pen(Color.Blue, 2), bluePoints);
            }

            // Assign the bitmap to the PictureBox
            picLUTGraph.Image = bmp;
        }

        // Helper method to calculate gamma-adjusted value
        private float CalculateGamma(byte input, float gamma, float contrast, float brightness)
        {
            float normalized = input / 255f;

            // Replace MathF.Pow with regular Math.Pow (double) and cast back to float
            float adjusted = (float)Math.Pow(normalized, gamma) * contrast + brightness;

            // Manual clamp implementation for older .NET versions
            if (adjusted < 0) return 0;
            if (adjusted > 1) return 1;
            return adjusted;
        }

        private void initTrayMenu()
        {
            contextMenu.Items.Clear();

            ToolStripMenuItem toolSetting = new ToolStripMenuItem("Settings", null, toolSettings_Click);
            contextMenu.Items.Add(toolSetting);

            ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
            contextMenu.Items.Add(toolStripSeparator1);

            for (int i = 0; i < displays.Count; i++)
            {
                toolMonitor = new ToolStripComboBox(displays[i].displayName);
                toolMonitor.DropDownStyle = ComboBoxStyle.DropDownList;

                toolMonitor.Items.Add(displays[i].displayName + ":");
                toolMonitor.Text = displays[i].displayName + ":";

                toolMonitor.SelectedIndexChanged += new EventHandler(comboBoxToolMonitor_SelectedIndexChanged);

                string[] presets = iniFile.GetSections();
                if (presets != null)
                {
                    for (int j = 0; j < presets.Length; j++)
                    {
                        if (iniFile.Read("monitor", presets[j]) == displays[i].displayName)
                        {
                            //preset.name = presets[j].Substring(presets[j].IndexOf(")") + 1);
                            toolMonitor.Items.Add(presets[j]);
                        }
                    }
                }
                toolMonitors.Add(toolMonitor);
                contextMenu.Items.Add(toolMonitor);
            }
            ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
            contextMenu.Items.Add(toolStripSeparator2);
            ToolStripMenuItem toolExit = new ToolStripMenuItem("Exit", null, toolExit_Click);
            contextMenu.Items.Add(toolExit);
        }

        private void fillInfo(Display.DisplayInfo currDisplay)
        {
            disableChangeFunc = true;

            textBoxGamma.Text = ((currDisplay.rGamma + currDisplay.gGamma + currDisplay.bGamma) / 3f).ToString("0.0");
            textBoxBrightness.Text = ((currDisplay.rBright + currDisplay.gBright + currDisplay.bBright) / 3f).ToString("0.0");

            trackBarGamma.Value = (int)(((currDisplay.rGamma + currDisplay.gGamma + currDisplay.bGamma) / 3f) * 10f);
            trackBarBrightness.Value = (int)(((currDisplay.rBright + currDisplay.gBright + currDisplay.bBright) / 3f) * 10f);

            disableChangeFunc = false;
        }
        private void Window_Load(object sender, EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Size.Width;
            int windowWidth = Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Size.Height;
            int windowHeight = Height;
            int tmp = Screen.PrimaryScreen.Bounds.Height;
            int TaskBarHeight = tmp - Screen.PrimaryScreen.WorkingArea.Height;

            //dpi
            /*int PSH = SystemParameters.PrimaryScreenHeight;
            int PSBH = Screen.PrimaryScreen.Bounds.Height;
            double ratio = PSH / PSBH;
            int TaskBarHeight = PSBH - Screen.PrimaryScreen.WorkingArea.Height;
            TaskBarHeight *= ratio;*/

            Location = new Point(screenWidth - windowWidth, screenHeight - (windowHeight + TaskBarHeight));
            DrawLUTGraph();
        }

        public Window()
        {
            InitializeComponent();
            customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ",";

            iniFile = new IniFile("GammaManager.ini");

            displays = Display.QueryDisplayDevices();
            displays.Reverse();
            for (int i = 0; i < displays.Count; i++)
            {
                displays[i].numDisplay = i;
                comboBoxMonitors.Items.Add(i + 1 + ") " + displays[i].displayName);
            }
            currDisplay = displays[numDisplay];
            comboBoxMonitors.SelectedIndex = numDisplay;

            fillInfo(currDisplay);

            initPresets();

            initTrayMenu();
            notifyIcon.ContextMenuStrip = contextMenu;

            // Initialize keybinds
            LoadKeybinds();
            SetupGlobalHook();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _globalHook.KeyPress -= GlobalHook_KeyPress;
            _globalHook.Dispose();
            base.OnFormClosed(e);
        }
        private void ApplyProfile(string profileName)
        {
            // Read the monitor name from the profile
            string profileMonitor = iniFile.Read("monitor", profileName);

            // Find the monitor in the displays list
            Display.DisplayInfo targetDisplay = displays.FirstOrDefault(d => d.displayName == profileMonitor);
            if (targetDisplay == null)
            {
                MessageBox.Show($"Monitor '{profileMonitor}' specified in profile '{profileName}' was not found.", "Monitor Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit if the monitor is not found
            }

            // Switch to the target monitor
            currDisplay = targetDisplay;
            numDisplay = currDisplay.numDisplay;

            // Update the UI to reflect the new monitor
            comboBoxMonitors.SelectedIndex = numDisplay;
            fillInfo(currDisplay);

            // Load gamma settings
            currDisplay.rGamma = float.Parse(iniFile.Read("rGamma", profileName), customCulture);
            currDisplay.gGamma = float.Parse(iniFile.Read("gGamma", profileName), customCulture);
            currDisplay.bGamma = float.Parse(iniFile.Read("bGamma", profileName), customCulture);

            // Load brightness settings
            currDisplay.rBright = float.Parse(iniFile.Read("rBright", profileName), customCulture);
            currDisplay.gBright = float.Parse(iniFile.Read("gBright", profileName), customCulture);
            currDisplay.bBright = float.Parse(iniFile.Read("bBright", profileName), customCulture);

            // Apply gamma and brightness settings
            Gamma.SetGammaRamp(currDisplay.displayLink,
                Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma,
                currDisplay.rContrast, currDisplay.gContrast, currDisplay.bContrast, // Keep existing contrast
                currDisplay.rBright, currDisplay.gBright, currDisplay.bBright)); // Apply brightness

            // Update UI with the new gamma and brightness values
            fillInfo(currDisplay);
            DrawLUTGraph();
        }
        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            // Get the key that was pressed
            Keys key = e.KeyCode;

            // Handle the '´' key explicitly using its raw key code
            if (e.KeyValue == 192) // Raw key code for '´' and '`'
            {
                key = Keys.Oemtilde; // Explicitly map to '´'
            }

            // Check if the key is mapped to a profile
            if (_keybindProfiles.ContainsKey(key))
            {
                string profileName = _keybindProfiles[key];
                ApplyProfile(profileName);
            }
        }
        private void GlobalHook_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Get the character that was pressed
            char keyChar = e.KeyChar;

            // Map the character to its corresponding Keys enum value
            Keys key;
            switch (keyChar)
            {
                case '-': key = Keys.OemMinus; break;
                case '[': key = Keys.OemOpenBrackets; break;
                case ']': key = Keys.OemCloseBrackets; break;
                case '=': key = Keys.Oemplus; break;
                case '/': key = Keys.OemQuestion; break; // '/' is mapped to OemQuestion
                default: key = (Keys)keyChar; break; // Default mapping for other characters
            }

            // Check if the key is mapped to a profile
            if (_keybindProfiles.ContainsKey(key))
            {
                string profileName = _keybindProfiles[key];
                ApplyProfile(profileName);
            }
        }

        private static readonly Dictionary<char, Keys> SpecialCharToKeys = new Dictionary<char, Keys>
        {
            { ',', Keys.Oemcomma },
            { '.', Keys.OemPeriod },
            { ';', Keys.OemSemicolon },
            { '/', Keys.OemQuestion },
            { '`', Keys.Oemtilde },
            { '~', Keys.Oemtilde }, // '~' shares the same key as '`'
            { '-', Keys.OemMinus },
            { '=', Keys.Oemplus },
            { '\\', Keys.OemBackslash },
            { '[', Keys.OemOpenBrackets },
            { ']', Keys.OemCloseBrackets },
            { '´', Keys.Oemtilde }, // Add the '´' key
            { '\'', Keys.OemQuotes } // Single quote
        };
        private void SetupGlobalHook()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyPress += GlobalHook_KeyPress;
        }
        private void LoadKeybinds()
        {
            _keybindProfiles = new Dictionary<Keys, string>();

            string[] sections = iniFile.GetSections();
            if (sections != null)
            {
                foreach (string section in sections)
                {
                    if (section.Contains(":"))
                    {
                        string encodedKeyStr = section.Split(':')[1].Trim();
                        string keyStr = DecodeProfileName(encodedKeyStr); // Decode the profile name
                        if (keyStr.Length == 1) // Ensure it's a single character
                        {
                            // Convert the character to a Keys enum value
                            char keyChar = keyStr[0];
                            Keys key;

                            if (keyChar == '´' || keyChar == '`')
                            {
                                // Map '´' and '`' to Keys.Oemtilde
                                key = Keys.Oemtilde;
                            }
                            else if (SpecialCharToKeys.TryGetValue(keyChar, out key))
                            {
                                // If the character is a special character, use the mapped Keys value
                                _keybindProfiles[key] = section;
                            }
                            else
                            {
                                // For regular characters, use the default mapping
                                key = (Keys)keyChar;
                                _keybindProfiles[key] = section;
                            }
                        }
                    }
                }
            }
        }
        private void trackBarGamma_ValueChanged(object sender, EventArgs e)
        {
            DrawLUTGraph();
            comboBoxPresets.Text = string.Empty;
            if (!disableChangeFunc)
            {
                textBoxGamma.Text = ((float)trackBarGamma.Value / 10f).ToString("0.0");

                if (allColors)
                {
                    currDisplay.rGamma = (float)trackBarGamma.Value / 10f;
                    currDisplay.gGamma = (float)trackBarGamma.Value / 10f;
                    currDisplay.bGamma = (float)trackBarGamma.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (redColor)
                {
                    currDisplay.rGamma = (float)trackBarGamma.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (greenColor)
                {
                    currDisplay.gGamma = (float)trackBarGamma.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (blueColor)
                {
                    currDisplay.bGamma = (float)trackBarGamma.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                }

            EndColors:
                return;

            }

        }

        private void trackBarBrightness_ValueChanged(object sender, EventArgs e)
        {
            DrawLUTGraph();
            comboBoxPresets.Text = string.Empty;
            if (!disableChangeFunc)
            {
                textBoxBrightness.Text = ((float)trackBarBrightness.Value / 10f).ToString("0.0");

                if (allColors)
                {
                    currDisplay.rBright = (float)trackBarBrightness.Value / 10f;
                    currDisplay.gBright = (float)trackBarBrightness.Value / 10f;
                    currDisplay.bBright = (float)trackBarBrightness.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (redColor)
                {
                    currDisplay.rBright = (float)trackBarBrightness.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (greenColor)
                {
                    currDisplay.gBright = (float)trackBarBrightness.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (blueColor)
                {
                    currDisplay.bBright = (float)trackBarBrightness.Value / 10f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                }

            EndColors:
                return;
            }
        }


        private string EncodeProfileName(string profileName)
        {
            // Convert the profile name to a "key coordinate" format
            // Example: "[" becomes "Profile_Key=91" (91 is the ASCII code for '[')
            return $"Profile_Key={(int)profileName[0]}";
        }

        private string DecodeProfileName(string encodedName)
        {
            // Convert the "key coordinate" back to the original profile name
            // Example: "Profile_Key=91" becomes "["
            if (encodedName.StartsWith("Profile_Key="))
            {
                int keyCode = int.Parse(encodedName.Substring("Profile_Key=".Length));
                return ((char)keyCode).ToString();
            }
            return encodedName; // Fallback for non-encoded names
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Get the profile name from the comboBoxPresets
            string profileName = comboBoxPresets.Text.Trim();

            // Validate that the profile name is a single character
            if (profileName.Length != 1)
            {
                MessageBox.Show("Profile names must be a single character (e.g., 'a', 'b', 'c').", "Invalid Profile Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit if the profile name is invalid
            }

            // Encode the profile name
            string encodedProfileName = EncodeProfileName(profileName);

            // Save the profile
            string fullProfileName = $"{currDisplay.displayName}: {encodedProfileName}";
            iniFile.Write("monitor", currDisplay.displayName, fullProfileName);
            iniFile.Write("rGamma", currDisplay.rGamma.ToString(), fullProfileName);
            iniFile.Write("gGamma", currDisplay.gGamma.ToString(), fullProfileName);
            iniFile.Write("bGamma", currDisplay.bGamma.ToString(), fullProfileName);
            iniFile.Write("rBright", currDisplay.rBright.ToString(), fullProfileName);
            iniFile.Write("gBright", currDisplay.gBright.ToString(), fullProfileName);
            iniFile.Write("bBright", currDisplay.bBright.ToString(), fullProfileName);
            
            // Reload profiles and update UI
            LoadKeybinds(); // Reload keybinds from the INI file
            initPresets(); // Reload presets in the comboBoxPresets
            initTrayMenu(); // Update the tray menu with the new profiles

            // Restore the selected preset in the comboBoxPresets
            comboBoxPresets.Text = $"{currDisplay.displayName}: {profileName}";
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            iniFile.DeleteSection(comboBoxPresets.Text);

            LoadKeybinds(); // Reload keybinds from the INI file
            initPresets(); // Reload presets in the comboBoxPresets
            initTrayMenu(); // Update the tray menu with the new profiles
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;

            trackBarGamma.Value = 10;
            trackBarBrightness.Value = 0;


            Gamma.SetGammaRamp(displays[numDisplay].displayLink, Gamma.CreateGammaRamp(1, 1, 1, 1, 1, 1, 0, 0, 0));
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void comboBoxMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            string num = comboBoxMonitors.SelectedItem.ToString();

            num = num.Substring(0, num.IndexOf(")"));
            numDisplay = Int32.Parse(num)-1;

            currDisplay = displays[numDisplay];
            fillInfo(currDisplay);
            
            initPresets();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            if (numDisplay + 1 <= displays.Count-1)
            {
                comboBoxMonitors.SelectedIndex = numDisplay + 1;
            } else
            {
                comboBoxMonitors.SelectedIndex = 0;
            }
        }
        private float ReadFloatFromIni(string key, string section, float defaultValue)
        {
            string value = iniFile.Read(key, section);
            if (string.IsNullOrEmpty(value) || !float.TryParse(value, NumberStyles.Any, customCulture, out float result))
            {
                return defaultValue;
            }
            return result;
        }

        private int ReadIntFromIni(string key, string section, int defaultValue)
        {
            string value = iniFile.Read(key, section);
            if (string.IsNullOrEmpty(value) || !int.TryParse(value, out int result))
            {
                return defaultValue;
            }
            return result;
        }

        private void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Get the selected profile name (e.g., "AOC2402: /")
                string selectedProfile = comboBoxPresets.Text;

                // Extract the full section name (e.g., "AOC2402: Profile_Key=47")
                string profileName = $"{currDisplay.displayName}: Profile_Key={(int)selectedProfile.Split(':')[1].Trim()[0]}";

                // Read and apply profile settings
                currDisplay.rGamma = ReadFloatFromIni("rGamma", profileName, 1.0f);
                currDisplay.gGamma = ReadFloatFromIni("gGamma", profileName, 1.0f);
                currDisplay.bGamma = ReadFloatFromIni("bGamma", profileName, 1.0f);
                currDisplay.rContrast = ReadFloatFromIni("rContrast", profileName, 1.0f);
                currDisplay.gContrast = ReadFloatFromIni("gContrast", profileName, 1.0f);
                currDisplay.bContrast = ReadFloatFromIni("bContrast", profileName, 1.0f);
                currDisplay.rBright = ReadFloatFromIni("rBright", profileName, 0.0f);
                currDisplay.gBright = ReadFloatFromIni("gBright", profileName, 0.0f);
                currDisplay.bBright = ReadFloatFromIni("bBright", profileName, 0.0f);
                currDisplay.monitorContrast = ReadIntFromIni("monitorContrast", profileName, 50);

                // Apply the profile settings
                Gamma.SetGammaRamp(currDisplay.displayLink,
                    Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma,
                    currDisplay.rContrast, currDisplay.gContrast, currDisplay.bContrast,
                    currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));

                // Update the UI
                fillInfo(currDisplay);
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show($"Error applying profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //tray

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void toolSettings_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void toolExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBoxToolMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!disableChangeFunc)
            {
                string monitor = sender.ToString().Substring(0, sender.ToString().IndexOf(":"));
                /*string comName = toolMonitor.Items[i].ToString().Substring
                        (0, toolMonitor.Items[i].ToString().IndexOf(":"));*/

                int tmp = 0;

                disableChangeFunc = true;
                for (int i = 0; i < displays.Count; i++)
                {
                    if (monitor.Equals(displays[i].displayName))
                    {
                        tmp = i;
                    }
                    else
                    {
                        toolMonitor = toolMonitors[i];
                        toolMonitor.SelectedIndex = 0;
                    }
                }
                disableChangeFunc = false;

                toolMonitor = toolMonitors[tmp];

                if (toolMonitor.SelectedIndex != 0)
                {

                    for (int i = 0; i < displays.Count; i++)
                    {
                        if (displays[i].displayName.Equals(toolMonitor.Items[0].ToString().Substring(0, toolMonitor.Items[0].ToString().IndexOf(":"))))
                        {
                            comboBoxMonitors.Text = i + 1 + ") " + displays[i].displayName;

                            numDisplay = i;
                            currDisplay.numDisplay = numDisplay;
                            currDisplay.displayLink = displays[i].displayLink;
                            currDisplay.isExternal = displays[i].isExternal;
                            break;
                        }
                    }

                    currDisplay.displayName = toolMonitor.Items[0].ToString().Substring(0, toolMonitor.Items[0].ToString().IndexOf(":"));

                    currDisplay.rGamma = float.Parse(iniFile.Read("rGamma", toolMonitor.Text), customCulture);
                    currDisplay.gGamma = float.Parse(iniFile.Read("gGamma", toolMonitor.Text), customCulture);
                    currDisplay.bGamma = float.Parse(iniFile.Read("bGamma", toolMonitor.Text), customCulture);
                    currDisplay.rBright = float.Parse(iniFile.Read("rBright", toolMonitor.Text), customCulture);
                    currDisplay.gBright = float.Parse(iniFile.Read("gBright", toolMonitor.Text), customCulture);
                    currDisplay.bBright = float.Parse(iniFile.Read("bBright", toolMonitor.Text), customCulture);
                    
                    fillInfo(currDisplay);

                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma,
                        currDisplay.rContrast, currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright,
                        currDisplay.bBright));

                    comboBoxPresets.Text = toolMonitor.Text;
                }
            }
        }

        private void textBoxGamma_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void picLUTGraph_Click(object sender, EventArgs e)
        {

        }

        private void labelBrightness_Click(object sender, EventArgs e)
        {

        }

        //destroy focuses on buttons, trackbars, comboboxes, text, checkbox
    }
}
