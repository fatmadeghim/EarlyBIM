using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIHelpFunctions
{
    public static class FormsHelper
    {
        public static int standardheight = 40;

        public static void ChangeDefaultHeight(int height)
        { standardheight = height; }

        public static int GetDefaultHeight() 
        { return standardheight; }

        public static System.Windows.Forms.TextBox CreateTextBox(string text, int length, int height)
        {
            var textBox = new System.Windows.Forms.TextBox();
            textBox.ReadOnly = true;
            textBox.Text = text;
            textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox.Size = new System.Drawing.Size(length, height);
            textBox.AutoSize = false;
            textBox.Multiline = true;
            return textBox;
        }

        public static System.Windows.Forms.TextBox CreateTextBox(string text, int length)
        {
            return CreateTextBox(text, length, standardheight);
        }

        public static System.Windows.Forms.TextBox CreateTextBox(string text)
        {
            var textBox = new System.Windows.Forms.TextBox();
            textBox.ReadOnly = true;
            textBox.Text = text;
            textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox.AutoSize = true;
            textBox.Multiline = true;
            return textBox;
        }

        public static System.Windows.Forms.CheckBox CreateCheckBox(string text, bool initialState, bool locked, int length, int height, Action<object, EventArgs> action)
        {
            var checkBox = new System.Windows.Forms.CheckBox();
            checkBox.Text = text;
            checkBox.Checked = initialState;
            checkBox.AutoCheck = !locked;
            checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBox.Size = new System.Drawing.Size(length, height);
            checkBox.CheckedChanged += new System.EventHandler(action);
            return checkBox;
        }

        public static System.Windows.Forms.CheckBox CreateCheckBox(string text, bool initialState, bool locked, Action<object, EventArgs> action)
        {
            var checkBox = new System.Windows.Forms.CheckBox();
            checkBox.Text = text;
            checkBox.Checked = initialState;
            checkBox.AutoCheck = !locked;
            checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBox.AutoSize = true;
            checkBox.CheckedChanged += new System.EventHandler(action);
            return checkBox;
        }

        public static System.Windows.Forms.TableLayoutPanel CreateTable(int numColumns, int numRows)
        {
            var table = new System.Windows.Forms.TableLayoutPanel();
            table.AutoSize = true;
            table.Dock = System.Windows.Forms.DockStyle.Fill;
            table.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.None;
            table.ColumnCount = numColumns;
            table.RowCount = numRows;
            return table;
        }

        public static System.Windows.Forms.ComboBox CreateComboBox(List<String> items)
        {
            var comboBox = new System.Windows.Forms.ComboBox();
            foreach (var item in items)
            {
                comboBox.Items.Add(item);
            }
            comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            return comboBox;
        }

        public static System.Windows.Forms.CheckedListBox CreateCheckedListBox(List<String> items)
        {
            var checkedlistbox = new CheckedListBox();
            checkedlistbox.CheckOnClick = true;
            checkedlistbox.FormattingEnabled = true;
            foreach (var item in items)
            {
                if (item is object) //Skips null values
                {
                    checkedlistbox.Items.Add(item);
                }
            }
            return checkedlistbox;
        }

        public static System.Windows.Forms.Button CreateButton(string text)
        {
            var button = new Button();
            button.AutoSize = true;
            button.Text = text;
            return button;
        }

        public static System.Windows.Forms.Button CreateButton(string text, int length, int height)
        {
            var button = new Button();
            button.Size = new System.Drawing.Size(length, height);
            button.Text = text;
            return button;
        }


        public static void ShowErrorMessage(string text, string title)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static TabPage CreateTabPage(string text)
        {
            var tabPage = new TabPage();
            tabPage.Text = text;
            return tabPage;
        }

        public static PictureBox CreatePictureBox(string filepathPicture)
        {
            var pictureBox = new PictureBox();
            pictureBox.ImageLocation = filepathPicture;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            return pictureBox;
        }
    }
}
