using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;

namespace TimingClient.Plugins.OutputPlugins.Dash
{


    public partial class ScreenEditor : Form
    {
        private PluginManager manager;

        private BindingList<ScreenPart> parts = new BindingList<ScreenPart>();

        public ScreenEditor(PluginManager manager)
            : this()
        {

            foreach (var prop in manager.GeneratedProperties)
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

        public ScreenEditor()
        {
            InitializeComponent();
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

        public void UpdateEditing(ScreenPart part)
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

        public void RefreshDisplay()
        {

            this.lblPreview.Text = TextGenerator.GetText(manager, parts.ToList());
            this.display1.SetText(this.lblPreview.Text);
        }

        private void RefreshList()
        {
            this.lstParts.RefreshItems();
        }

        public void SetValuesToPart()
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            UpdateEditing(lstParts.SelectedItem as ScreenPart);
        }

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

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
    public class RefreshingListBox : ListBox
    {
        public new void RefreshItem(int index)
        {
            base.RefreshItem(index);
        }

        public new void RefreshItems()
        {
            base.RefreshItems();
        }
    }
}