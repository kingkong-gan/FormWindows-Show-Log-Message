using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FormLog
{
    static class FormLogMsg
    {
        static private Dictionary<string,LogBox> myLogCtrltable = new Dictionary<string, LogBox>();
               
            /// <summary>
            /// 往字典添加控件和控件标签，实例化LogBox，注意不能重复添加相同控件或相同控件标记
            /// </summary>
            /// <param name="textBox"></param>
            /// <param name="ctrlmark"></param>
            /// <param name="savepath"></param>
            /// <returns></returns>
        static public bool AddCtrl(TextBox textBox,string ctrlmark,string savepath)
        {
            List<TextBox> textBoxes =new List<TextBox>();   //使用list作为临时变量，读取已经初始化的log控件
            foreach(LogBox logbox in myLogCtrltable.Values)      
            {
                textBoxes.Add(logbox.myTextBox);
            }

            if(!textBoxes.Contains(textBox)&&!myLogCtrltable.ContainsKey(ctrlmark)&& ctrlmark != "default" && ctrlmark != null)
            {
                myLogCtrltable.Add(ctrlmark, new LogBox(textBox, savepath));
                return true;
            }
            else
            {
                ArgumentException arg;
                //此处应该添加错误提示或异常，表示出现了重控件，重名key
                if (textBoxes.Contains(textBox))
                {
                    arg = new ArgumentException("该控件已存在");
                    throw arg;
                }
                else    if (myLogCtrltable.ContainsKey(ctrlmark))
                {
                    arg = new ArgumentException("该控件标记已存在");
                    throw arg;
                }
                else   if(ctrlmark != "default")
                {
                    arg = new ArgumentException("请勿使用default作为控件标记");
                    throw arg;
                }
                else if(ctrlmark != null)
                {
                    arg = new ArgumentException("控件标记不允许为null");
                    throw arg;
                }
                return false;
            }
        }

        /// <summary>
        /// 往字典添加控件和控件标签，实例化LogBox，注意不能重复添加相同控件
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="savepath"></param>
        /// <returns></returns>
        static public bool AddCtrl(TextBox textBox,string savepath)
        {
            List<TextBox> textBoxes = new List<TextBox>();   //使用list作为临时变量，读取已经初始化的log控件
            foreach (LogBox logbox in myLogCtrltable.Values)
            {
                textBoxes.Add(logbox.myTextBox);
            }

            if (!textBoxes.Contains(textBox) && !myLogCtrltable.ContainsKey("default"))
            {
                myLogCtrltable.Add("default", new LogBox (textBox,savepath));                
                return true;
            }
            else
            {
                //此处应该添加错误提示或异常，表示控件已经存在
                ArgumentException arg = new ArgumentException("该控件已存在");
                throw arg;                
            }

        }
        
        /// <summary>
        /// 从字典中移除对应的key和对象，并且注销对象
        /// </summary>
        /// <param name="ctrlmark"></param>
        /// <returns></returns>
        static public bool RemoveCtrl(string ctrlmark="default")
        {
            //移除字典中对应key元素
            myLogCtrltable[ctrlmark].Dispose();
            myLogCtrltable.Remove(ctrlmark);
            return true;
        }

        /// <summary>
        /// 修改默认控件的保存地址
        /// </summary>
        /// <param name="savepath"></param>
        /// <returns></returns>
        static public bool RedirectFilePath(string savepath)
        {
            myLogCtrltable["default"].ChangeFilePath(savepath);
            return true;
        }

        /// <summary>
        /// 修改指定控件的保存地址
        /// </summary>
        /// <param name="ctrlmark"></param>
        /// <param name="savepath"></param>
        /// <returns></returns>
        static public bool RedirectFilePath(string ctrlmark,string savepath)
        {
            myLogCtrltable[ctrlmark].ChangeFilePath(savepath);
            return true;
        }

        /// <summary>
        /// 使用默认对象显示log信息
        /// </summary>
        /// <param name="message"></param>
        static public void LogMsgShow(string message)
        {
            if (myLogCtrltable.ContainsKey("default"))
            {
                myLogCtrltable["default"].DisplayLog(message);
            }
            else
            {
                //提示错误，找不到对应的default控件标志
                ArgumentException arg = new ArgumentException("Can not find the default control mark");
                throw arg;
            }
        }

        /// <summary>
        /// 使用指定控件标记对象显示log信息
        /// </summary>
        /// <param name="ctrlmark"></param>
        /// <param name="message"></param>
        static public void LogMsgShow(string ctrlmark ,string message)
        {
            if (myLogCtrltable.ContainsKey(ctrlmark))
            {
                myLogCtrltable[ctrlmark].DisplayLog(message);
            }
            else
            {
                //提示错误，找不到对应的key
                ArgumentException arg = new ArgumentException("Can not find the control mark");
                throw arg;
            }
        }

        /// <summary>
        /// 注销字典全部对象
        /// </summary>
        static  public void Dispose()
        {
            foreach(LogBox logBox in myLogCtrltable.Values)
            {
                logBox.Dispose();
            }
        }

        /// <summary>
        /// 注销指定mark的控件
        /// </summary>
        /// <param name="ctrlmark"></param>
        static public void Dispose(string ctrlmark)
        {
            myLogCtrltable[ctrlmark].Dispose();
        }

    }

    /// <summary>
    /// 用于实例化前面板log显示控件的类，供FormLogMsg类使用
    /// </summary>
    internal class LogBox:IDisposable
    {
        public TextBox myTextBox;
        private bool textbox_mouse_enter_flag = false;
        private int textbox_maxlines = 100;
        private string logfile_date=DateTime.Now.ToLongDateString().ToString();
        

        private int textbox_selectline_index
        {
            get;set;           
        }

        internal int textbox_selectchar_index
        {
            get;set;
        }

        private StreamWriter _logfile_streamwriter;

        private StreamWriter logfile_streamwriter
        {
            get
            {
                return _logfile_streamwriter;
            }
            set
            {
                _logfile_streamwriter = value;
                _logfile_streamwriter.AutoFlush = true;  
            }
        }

        /// <summary>
        /// 委托用于非主线程修改控件属性
        /// </summary>
        /// <param name="txt"></param>
        private delegate void LogBoxDel(string txt);
        
        /// <summary>
        /// 使用控件引用，保存日志文件夹路径进行实例化
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="savepath"></param>
        public LogBox(TextBox textBox,string savepath)
        {
            InitCtrl(textBox, savepath);
        }
        
        /// <summary>
        /// 初始化textbox控件的属性，挂载相关事件,默认不需要保存文件，savepath=null
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="savepath"></param> 不需要保存文件，写null
        private void InitCtrl(TextBox textBox, string savepath = null)
        {
            //保存控件引用，初始化控件重要部分，挂载事件
            myTextBox = textBox;                      
            textBox.Multiline = true;
            textBox.WordWrap = true;
            textBox.ReadOnly = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.MouseWheel += TextBox_MouseWheel;
            textBox.MouseEnter += TextBox_MouseEnter;
            textBox.MouseLeave += TextBox_MouseLeave;
            textBox.MouseDoubleClick += TextBox_ClearLog;
            MenuInit(textBox);
        }

        /// <summary>
        /// 初始化控件的右键窗口和事件，后续可以在此添加更多
        /// </summary>
        /// <param name="textBox"></param>
        private void MenuInit(TextBox textBox)
        {
            //textBox.ContextMenuStrip = new ContextMenuStrip();
            //textBox.ContextMenuStrip.Items.Add("打开历史文件");
            //textBox.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;            
            MenuItem menuItem = new MenuItem("打开历史文件");
            textBox.ContextMenu = new ContextMenu(new MenuItem[1] {menuItem});
            menuItem.Click += ContextMenu_ItemClicked;
            
        }

        /// <summary>
        /// 右键菜单选中事项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_ItemClicked(object sender, EventArgs e)  //ToolStripItemClicked
        {
            switch (((MenuItem)sender).Text)
            {
                case "打开历史文件":
                    System.Diagnostics.Process.Start("explorer.exe", (string)myTextBox.Tag);
                    break;

                case null:
                    MessageBox.Show("该日志无文件保存！", "无历史文件", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    MessageBox.Show("程序未设置该选型操作", "无对应操作", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }                
        }



        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="e"></param>
        private void TextBox_MouseWheel(object textbox, MouseEventArgs e)
        {
            TextBox tb = (TextBox)textbox;
            int linestep = e.Delta / 40; //鼠标滚轮一次Delta是±120，一次移动三行
            textbox_selectline_index = textbox_selectline_index < linestep ? 0 : textbox_selectline_index - linestep;
            textbox_selectchar_index = tb.GetFirstCharIndexFromLine(textbox_selectline_index + 1 >= tb.Lines.GetLength(0) ? tb.Lines.GetLength(0) - 1 : textbox_selectline_index);
        }

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="e"></param>
        private void TextBox_MouseEnter(object textbox, EventArgs e)
        {
            //获取当前显示的首行文本坐标
            textbox_mouse_enter_flag = true;
            textbox_selectline_index= myTextBox.GetLineFromCharIndex(myTextBox.GetCharIndexFromPosition(new Point(0, 10)));
            textbox_selectchar_index= myTextBox.GetFirstCharIndexFromLine(textbox_selectline_index);
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="e"></param>
        private void TextBox_MouseLeave(object textbox, EventArgs e)
        {
            //清除标志位
            textbox_mouse_enter_flag = false;
        }

        /// <summary>
        /// 清空显示事件
        /// </summary>
        public void TextBox_ClearLog(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("清空信息？", "ClearMessage", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                myTextBox.Clear();
            }
        }

        /// <summary>
        /// 往文本末尾添加信息，且保持信息行数不超过textbox_maxlines
        /// </summary>
        /// <param name="txt"></param>
        internal void AppendTxt(string txt)
        {
            if (myTextBox.Lines.GetLength(0) >= textbox_maxlines)
            {
                List<string> tmplines = Enumerable.ToList<string>(myTextBox.Lines);
                textbox_selectchar_index = textbox_selectchar_index > tmplines[0].Length + 2 ? textbox_selectchar_index - tmplines[0].Length - 2 : 0;
                tmplines.RemoveAt(0);                
                myTextBox.Lines = tmplines.ToArray(); //因为不断重复指向新引用，导致了频繁的GC发生
                tmplines.Clear();
                tmplines = null;
            }
            string tmptxt = "\r\n" + DateTime.Now.ToLongTimeString() + "   " + txt;
            myTextBox.AppendText(tmptxt);
            if (textbox_mouse_enter_flag)
            {
                myTextBox.Select(textbox_selectchar_index, 0);
                myTextBox.ScrollToCaret();
            }
            if (myTextBox.Tag != null)
            {
                //保存文件
                WriteLogFile(tmptxt);
                //保存文件失败时如何提示
            }
        }

        /// <summary>
        /// 跨线程修改控件信息，供外部库使用
        /// </summary>
        /// <param name="txt"></param>
        public void DisplayLog(string txt)
        {
            myTextBox.BeginInvoke(new LogBoxDel(AppendTxt), new string[1] { txt }); //开始异步改变前面板控件显示值
        }

        /// <summary>
        /// 保存log文件，判断当前日期是否与初始化时一致，一致则直接写入，否则替换
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        internal bool WriteLogFile(string txt)
        {
            if (logfile_date != DateTime.Now.ToLongDateString().ToString())
            {
                logfile_streamwriter.Close();
                logfile_date = DateTime.Now.ToLongDateString().ToString();
                logfile_streamwriter = new StreamWriter(Path.Combine((string)myTextBox.Tag, logfile_date + ".log"), true);                
            }
            logfile_streamwriter.Write(txt);
            return true;
        }

        /// <summary>
        /// 修改文件保存路径
        /// </summary>
        /// <param name="savepath"></param>
        /// <returns></returns>
        public bool ChangeFilePath(string savepath)
        {
            Regex regex = new Regex(@"^[A-Z]:(\\{1}\S[^\\^\/^\:^\?^\*^""^\<^\>^\.]+)*\S\\?$", RegexOptions.IgnoreCase); //判断文件路径是否合法
                                                        //  无法判定文件一头一尾空格非法
            if (savepath != null && regex.IsMatch(savepath))
            {
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                myTextBox.Tag = savepath;
                
                try
                {
                    logfile_streamwriter.Close(); //先关闭原来的文件流，用try是因为此处异常不影响
                }
                catch
                {

                }
                logfile_streamwriter = new StreamWriter(Path.Combine((string)myTextBox.Tag, logfile_date + ".log"), true);
                return true;
            }
            else
            {
                if (savepath != null)
                {
                    MessageBox.Show("Log信息保存路径不合法，请修改！", "路径不合法", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                myTextBox.Tag = null;
                return false;
            }
        }

        /// <summary>
        /// 注销控件相关数据
        /// </summary>
        public void Dispose()
        {
            //移除事件
            myTextBox.MouseWheel -= TextBox_MouseWheel;
            myTextBox.MouseEnter -= TextBox_MouseEnter;
            myTextBox.MouseLeave -= TextBox_MouseLeave;
            myTextBox.MouseDoubleClick -= TextBox_ClearLog;
            
            //关闭文件
            if (myTextBox.Tag != null)
            {
                logfile_streamwriter.Close();
            }
            //清除Tag
            myTextBox.Tag = null;
        }
    }
}
