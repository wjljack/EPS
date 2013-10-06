using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Net;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using IWshRuntimeLibrary;
using System.Text.RegularExpressions;

namespace HRSeat.CommonClass
{
    class CommonFunction
    {/// <summary>  
        /// 是否中文  
        /// </summary>  
        /// <param name="chars"></param>  
        /// <param name="RegType">true:包含中文;false:全部为中文</param>  
        /// <returns></returns>  
        public static bool IsChinese(string chars, bool RegType)
        {
            if (RegType)
            {
                return Regex.IsMatch(chars, @"^[\u4E00-\u9FA5]+$");
            }
            return Regex.IsMatch(chars, @"[\u4E00-\u9FA5]+");
        }

        /// <summary>  
        /// 字符串截取（按字节）  
        /// </summary>  
        /// <param name="s"></param>  
        /// <param name="length"></param>  
        /// <returns></returns>  
        public static string bSubstring(string s, int length)
        {
            string str = "";
            if (Encoding.GetEncoding("GB2312").GetBytes(s).Length < length)
            {
                return s;
            }
            if (!IsChinese(s, false))
            {
                return s.Substring(0, length);
            }
            if (IsChinese(s, true))
            {
                return s.Substring(0, length / 2);
            }
            int num = length / 2;
            int num2 = length;
            while (true)
            {
                str = str + s.Substring(str.Length, num);
                num2 = length - Encoding.GetEncoding("GB2312").GetBytes(str).Length;
                if (num2 <= 1)
                {
                    if ((num2 == 1) && (Encoding.GetEncoding("GB2312").GetBytes(s.Substring(str.Length, 1)).Length == 1))
                    {
                        str = str + s.Substring(str.Length, 1);
                    }
                    return str;
                }
                num = num2 / 2;
            }
        }


        /// <summary>  
        /// 截取指定长度的字节数，并在末尾加上指定字符，比如 “...”  
        /// </summary>  
        /// <param name="s"></param>  
        /// <param name="length"></param>  
        /// <param name="p_TailString"></param>  
        /// <returns></returns>  
        public static string bSubstring(string s, int length, string p_TailString)
        {
            string str = s;
            if (Encoding.GetEncoding("GB2312").GetBytes(str).Length > length)
            {
                str = bSubstring(s, length - 2);
                str = str + p_TailString;
            }
            return str;
        }
        public static void DeleteStartupFolderShortcuts(string targetExeName)
        {
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            DirectoryInfo di = new DirectoryInfo(startUpFolderPath);
            FileInfo[] files = di.GetFiles("*.lnk");

            foreach (FileInfo fi in files)
            {
                string shortcutTargetFile = GetShortcutTargetFile(fi.FullName);

                if (shortcutTargetFile.EndsWith(targetExeName,
                      StringComparison.InvariantCultureIgnoreCase))
                {
                    System.IO.File.Delete(fi.FullName);
                }
            }
        }
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = Path.GetFileName(shortcutFilename);

            Shell32.Shell shell = new Shell32.ShellClass();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link =
                  (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return String.Empty; // Not found
        }
        public static void CreateStartupFolderShortcut()
        {
            WshShellClass wshShell = new WshShellClass();
            IWshRuntimeLibrary.IWshShortcut shortcut;
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Create the shortcut
            shortcut =
              (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
                startUpFolderPath + "\\" +
                "HRStartUp.lnk");

            shortcut.TargetPath = Application.StartupPath + "\\HRStartUp.bat";
            shortcut.WorkingDirectory = Application.StartupPath;
            shortcut.Description = "Launch My Application";
            // shortcut.IconLocation = Application.StartupPath + @"\App.ico";
            shortcut.Save();
        }
        /// <summary>
        /// 设置程序开机自动运行
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isAutoRun">是否自动运行</param>
        public static void SetAutoRun(string fileName, bool isAutoRun)
        {
            string batFile = Application.StartupPath + "\\HRStartUp.bat"; //打开文件并显示其内容 
            try
            {
                FileStream fs = new FileStream(batFile, FileMode.Create);
                string fileContent = "start \"HRStartUp\" \"" + fileName + "\"\r\nexit";
                byte[] data = Encoding.Default.GetBytes(fileContent);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
            catch /*(System.Exception ex)*/
            {
                MessageBox.Show("设置失败");
                return;
            }
            try
            {
                if (isAutoRun)
                {
                    CreateStartupFolderShortcut();
                }
                else
                {
                    DeleteStartupFolderShortcuts(batFile);
                }
            }
            catch
            {
            }
            finally
            {
            }

        }
        static string ip = null;
        public static string GetIPAddress()
        {
            if (ip == null)
            {
                IniFile inifile = new IniFile("config.ini");
                ip = inifile.ReadString("LOCAL_CONFIG", "IP", "");
            }
            if (ip == "")
            {
                frmIPSet ipFrm = new frmIPSet();
                ipFrm.ShowDialog();
                IniFile inifile = new IniFile("config.ini");
                ip = inifile.ReadString("LOCAL_CONFIG", "IP", "");
            }
            return ip;
        }
        public static void InputPhoneNumber(object sender, KeyPressEventArgs e)
        {
            char kc = e.KeyChar;
            if ((kc < 48 || kc > 57) && kc != 8 && kc != (char)22 && kc != (char)3 && kc != (char)24 && kc != '-')
                e.Handled = true;
        }
        public static void InputNumberOnly(object sender, KeyPressEventArgs e)
        {
            char kc = e.KeyChar;
            if ((kc < 48 || kc > 57) && kc != 8 && kc != (char)22 && kc != (char)3 && kc != (char)24)
                e.Handled = true;
        }
        public static void InputIP(object sender, KeyPressEventArgs e)
        {
            char kc = e.KeyChar;
            if ((kc < 48 || kc > 57) && kc != 8 && kc != 46 && kc != (char)22 && kc != (char)3 && kc != (char)24)
                e.Handled = true;
        }
        public static void InputEngNumber(object sender, KeyPressEventArgs e)
        {
            char kc = e.KeyChar;
            if ((kc >= 'A' && kc <= 'Z') || (kc >= 'a' && kc <= 'z') || (kc >= '0' && kc <= '9') || kc == 8 && kc != (char)22 && kc != (char)3 && kc != (char)24)
                e.Handled = false;
            else
                e.Handled = true;
        }
        public static void InputPWD(object sender, KeyPressEventArgs e)
        {
            char kc = e.KeyChar;
            if ((kc >= '!' && kc <= '~') || kc == 8 && kc != (char)22 && kc != (char)3 && kc != (char)24)
                e.Handled = false;
            else
                e.Handled = true;
        }
        public static bool IsEmail(string strEmail)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strEmail, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        public static string GetAppConfig(string key)
        {
            string value = null;
            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {

            }
            if (value == "" || value == null)
            {
                MessageBox.Show("系统配置错误!");
                Environment.Exit(0);
            }
            return value;
        }
        public static void ButtonOver(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = HRSeat.Properties.Resources.button_lod_over;
            btn.BackgroundImageLayout = ImageLayout.Tile;

        }
        public static void ButtonDown(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = HRSeat.Properties.Resources.button_lod_down;
            btn.BackgroundImageLayout = ImageLayout.Tile;
        }
        public static void ButtonLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = HRSeat.Properties.Resources.button_lod;
            btn.BackgroundImageLayout = ImageLayout.Tile;
        }
        public static void SetButtonStyle(Control ctl)
        {
            foreach (Control c in ctl.Controls)
            {
                if (c is Button)
                {
                    Button btn = c as Button;
                    if (btn.Size.Width > 74 && btn.Size.Height > 28)
                    {
                        continue;
                    }
                    if (btn.Name == "btnUpload" || btn.Name == "btnAddRoom" || btn.Name == "btnTpgAddRoomSave")
                    {
                        btn.BackgroundImage = HRSeat.Properties.Resources.view_button_bj_up;
                    }
                    else
                    {
                        btn.BackgroundImage = HRSeat.Properties.Resources.button_lod;

                    }
                    btn.BackgroundImageLayout = ImageLayout.Tile;
                    btn.MouseDown += new MouseEventHandler(CommonFunction.ButtonDown);
                    btn.MouseHover += new EventHandler(CommonFunction.ButtonOver);
                    btn.MouseLeave += new EventHandler(CommonFunction.ButtonLeave);
                    btn.MouseUp += new MouseEventHandler(CommonFunction.ButtonLeave);
                }
                SetButtonStyle(c);
            }
        }

        public static void SetComboBoxStyle(ComboBox c)
        {
            c.DrawItem += new DrawItemEventHandler(Combobox_DrawItem);
            c.MeasureItem += new MeasureItemEventHandler(ComboBox_MeasureItem);
        }
        private static void Combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            Rectangle r = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            r.Y += 2;
            e.Graphics.DrawString(c.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), r);
        }

        private static void ComboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 16;
        }
    }
}
