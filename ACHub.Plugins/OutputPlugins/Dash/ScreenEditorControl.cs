using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ACHub.Plugins.OutputPlugins.Dash
{
    /// <summary>
    /// Screen Editor control
    /// </summary>
    public partial class ScreenEditorControl : UserControl
    {
        private PluginManager manager;

        private BindingList<ScreenPart> parts = new BindingList<ScreenPart>();

        public string Announce
        {
            get { return txtAnnouceText.Text; }
            set { txtAnnouceText.Text = value; }
        }

        /// <summary>
        /// Screen parts
        /// </summary>
        public BindingList<ScreenPart> Parts
        {
            get { return parts; }
            set { parts = value; this.lstParts.DataSource = parts; }
        }

        public string GetParent(string item)
        {
            if (item.Contains("."))
            {
                var tmp = item.Split('.').ToList();
                return string.Join(".", tmp.Take(tmp.Count - 1));
            }
            return "";
        }

        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="manager"></param>
        public ScreenEditorControl(PluginManager manager)
            : this()
        {
            foreach (var prop in manager.GeneratedProperties.OrderBy(i => GetParent(i.Key)).ThenBy(i => i.Key))
            {
                lstExpression.Items.Add(prop.Key);
            }

            RefreshDisplay();
            this.manager = manager;

            UpdateEditing(null);

            this.lstParts.DataSource = parts;
            this.lstParts.DisplayMember = "";
            this.lstParts.DisplayMember = "Description";

            //CreateItem();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public ScreenEditorControl()
        {
            InitializeComponent();
            this.numMaxLength.ValueChanged += new System.EventHandler(this.numMaxLength_ValueChanged);
            this.cbRightToLeft.CheckedChanged += new System.EventHandler(this.cbRightToLeft_CheckedChanged);
            this.txtCustomFormat.TextChanged += new System.EventHandler(this.txtCustomFormat_TextChanged);
            this.txtSearch.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            this.btnFindExp.Click += new System.EventHandler(this.btnFindExp_Click);
            this.lstExpression.SelectedIndexChanged += new System.EventHandler(this.lstExpression_SelectedIndexChanged);
            this.txtText.TextChanged += new System.EventHandler(this.txtText_TextChanged);
            this.rbExpression.CheckedChanged += new System.EventHandler(this.rbExpression_CheckedChanged);
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            this.lstParts.SelectedIndexChanged += new System.EventHandler(this.lstParts_SelectedIndexChanged);
        }

        private ScreenPart editing = null;

        private void button1_Click(object sender, EventArgs e)
        {
            CreateItem();
        }

        private void CreateItem()
        {
            lock (this)
            {
                var newitem = new ScreenPart() { Text = "-" };

                UpdateEditing(null);
                parts.Add(newitem);
                UpdateEditing(newitem);

                RefreshDisplay();
            }
        }

        private void UpdateEditing(ScreenPart part)
        {
            this.editing = null;
            if (part == null)
            {
                panelEdit.Enabled = false;
                this.editing = part;
            }
            else
            {
                lock (this)
                {
                    panelEdit.Enabled = true;

                    if (part.Expression != null)
                    {
                        this.lstExpression.SelectedItem = part.Expression;
                    }
                    else
                    {
                        this.lstExpression.SelectedIndex = 0;
                    }
                    this.txtText.Text = part.Text;
                    this.cbRightToLeft.Checked = part.RightAlign;
                    this.numMaxLength.Value = part.FixedLength;
                    this.rbExpression.Checked = part.Expression != null;
                    this.rbText.Checked = part.Expression == null;
                    this.txtCustomFormat.Text = part.FormatString ?? "";
                }
                this.editing = part;
                SetPartSourceStatus();
            }
        }

        private void RefreshDisplay()
        {
            this.lblPreview.Text = TextGenerator.GetText(manager, parts.ToList());
            this.display1.SetText(this.lblPreview.Text);
        }

        private void RefreshList()
        {
            this.lstParts.RefreshItems();
        }

        private void SetValuesToPart()
        {
            if (editing != null)
            {
                lock (this)
                {
                    editing.Expression = ToNotEmptyOrNull(this.lstExpression.SelectedItem as string);
                    editing.Text = ToNotEmptyOrNull(this.txtText.Text);
                    editing.RightAlign = this.cbRightToLeft.Checked;
                    editing.FixedLength = (int)this.numMaxLength.Value;
                    editing.FormatString = txtCustomFormat.Text;

                    if (rbText.Checked)
                    {
                        editing.Expression = null;
                    }
                    else
                    {
                        editing.Text = null;
                    }
                }
                RefreshList();
            }
            RefreshDisplay();
        }

        private string ToNotEmptyOrNull(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else return value;
        }

        private void txtText_TextChanged(object sender, EventArgs e)
        {
            SetValuesToPart();
        }

        private void lstExpression_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetValuesToPart();
        }

        private void numMaxLength_ValueChanged(object sender, EventArgs e)
        {
            SetValuesToPart();
        }

        private void cbRightToLeft_CheckedChanged(object sender, EventArgs e)
        {
            SetValuesToPart();
        }

        private void SetPartSourceStatus()
        {
            txtText.Enabled = !rbExpression.Checked;
            panelExpression.Enabled = rbExpression.Checked;
        }

        private void rbExpression_CheckedChanged(object sender, EventArgs e)
        {
            SetPartSourceStatus();
            SetValuesToPart();
        }

        private void lstParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditing(lstParts.SelectedItem as ScreenPart);
        }

        //private void btnEdit_Click(object sender, EventArgs e)
        //{
        //    UpdateEditing(lstParts.SelectedItem as ScreenPart);
        //}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.lstParts.SelectedItem != null)
            {
                this.parts.Remove(this.lstParts.SelectedItem as ScreenPart);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            FindItem(true);
        }

        private void btnFindExp_Click(object sender, EventArgs e)
        {
            FindItem(false);
        }

        private void FindItem(bool fromStart)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                int baseindex = fromStart ? 0 : lstExpression.SelectedIndex;
                for (int i = 1; i < lstExpression.Items.Count; i++)
                {
                    if (lstExpression.Items[(baseindex + i) % lstExpression.Items.Count].ToString().ToLower().Contains(txtSearch.Text.ToLower()))
                    {
                        lstExpression.SelectedIndex = (baseindex + i) % lstExpression.Items.Count;
                        return;
                    }
                }
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FindItem(false);
            }
        }

        private void txtCustomFormat_TextChanged(object sender, EventArgs e)
        {
            SetValuesToPart();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (this.lstParts.SelectedItem != null)
            {
                {
                    ScreenPart exp = this.lstParts.SelectedItem as ScreenPart;
                    var oldidx = parts.IndexOf(exp);
                    if (oldidx > 0)
                    {
                        parts.Remove(exp);
                        parts.Insert(oldidx - 1, exp);
                        this.lstParts.SelectedItem = exp;
                    }
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (this.lstParts.SelectedItem != null)
            {
                {
                    ScreenPart exp = this.lstParts.SelectedItem as ScreenPart;
                    var oldidx = parts.IndexOf(exp);
                    if (oldidx < parts.Count - 1)
                    {
                        parts.Remove(exp);
                        parts.Insert(oldidx + 1, exp);
                        this.lstParts.SelectedItem = exp;
                    }
                }
            }
        }
    }
}