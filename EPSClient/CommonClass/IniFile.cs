using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace HRSeat.CommonClass
{
    public class IniFile
    {
        /// <summary>
        /// 获取ini文件名 
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 写入ini文件的API函数
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="val">键值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 读取ini文件的API函数
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="def">缺省值</param>
        /// <param name="retVal">获取的数据</param>
        /// <param name="size">数据的大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        
        public IniFile(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            if (!fileInfo.Exists)
            {
                //文件不存在，建立文件 
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("ini文件不存在"));
                }
            }
            //必须是完全路径，不能是相对路径 
            FileName = fileInfo.FullName;
        }

        /// <summary>
        /// 向ini文件读取string类型数据
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="def">缺省值</param>
        /// <returns></returns>
        public string ReadString(string section, string key, string def)
        {
            Byte[] buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(section, key, def, buffer, buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文 
            string s = Encoding.GetEncoding(0).GetString(buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

        /// <summary>
        /// 向ini文件写入string类型数据
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void WriteString(string section, string key, string value)
        {
            if (!WritePrivateProfileString(section, key, value, FileName))
            {
                throw (new ApplicationException("写Ini文件出错"));
            }
        }

        /// <summary>
        /// 向ini文件读取int类型数据
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="def">缺省值</param>
        /// <returns></returns>
        public int ReadInt(string section, string key, int def)
        {
            string intStr = ReadString(section, key, Convert.ToString(def));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return def;
            }
        }

        /// <summary>
        /// 向ini文件写入int类型数据
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void WriteInt(string section, string key, int value)
        {
            WriteString(section, key, value.ToString());
        }

        /// <summary>
        /// 向ini文件读取bool类型数据
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="def">缺省值</param>
        /// <returns></returns>
        public bool ReadBool(string section, string key, bool def)
        {
            try
            {
                return Convert.ToBoolean(ReadString(section, key, Convert.ToString(def)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return def;
            }
        }

        /// <summary>
        /// 向ini文件写入bool类型数据
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void WriteBool(string section, string key, bool value)
        {
            WriteString(section, key, Convert.ToString(value));
        }

        /// <summary>
        /// 从ini文件中，获取指定的配置节名称中的所有键名 
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <returns></returns>
        public StringCollection ReadSection(string section)
        {
            Byte[] buffer = new Byte[16384];
            int bufLen = GetPrivateProfileString(section, null, null, buffer, buffer.GetUpperBound(0), FileName);
            //对Section进行解析 
            StringCollection keys = new StringCollection();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(buffer, start, i - start);
                        keys.Add(s);
                        start = i + 1;
                    }
                }
            }
            return keys;
        }

        /// <summary>
        /// 获取指定的配置节中的所有键的键名和键值 
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <returns></returns>
        public NameValueCollection ReadSectionValues(string section)
        {
            StringCollection KeyList = new StringCollection();
            KeyList = ReadSection(section);
            NameValueCollection values = new NameValueCollection();
            foreach (string key in KeyList)
            {
                values.Add(key, ReadString(section, key, ""));
            }
            return values;
        }

        /// <summary>
        /// 删除某个配置节
        /// </summary>
        /// <param name="section">配置节名称</param>
        public void DeleteSection(string section)
        {
            if (!WritePrivateProfileString(section, null, null, FileName))
            {
                throw (new ApplicationException("无法清除Ini文件中的Section"));
            }
        }

        /// <summary>
        /// 删除某个配置节下的键 
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        public void DeleteKey(string section, string key)
        {
            WritePrivateProfileString(section, key, null, FileName);
        }

        /// <summary>
        /// 检查某个配置节下的某个键是否有键值
        /// </summary>
        /// <param name="section">配置节名称</param>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public bool KeyExists(string section, string key)
        {
            StringCollection keys = new StringCollection();
            ReadSection(section);
            return keys.IndexOf(key) > -1;
        }
    }
}
