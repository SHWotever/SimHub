using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimingClient.Plugins.DataPlugins.DataCore
{

	/// <summary>
	/// Date Core plugin settings control
	/// </summary>
    public partial class DataCorePluginSettingsControl : UserControl
    {
        DataCorePlugin plugin;
        PluginManager manager;

        /// <summary>
        /// CTor
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="manager"></param>
        public DataCorePluginSettingsControl(DataCorePlugin plugin, PluginManager manager)
            : this()
        {
            this.plugin = plugin;
            this.manager = manager;
            this.lstExpressions.DataSource = plugin.Settings.Expressions;
            this.lstExpressions.DisplayMember = "Name";
        }

		/// <summary>
		/// CTor
		/// </summary>
        public DataCorePluginSettingsControl()
        {
            InitializeComponent();
        }

        private void btnAddExpression_Click(object sender, EventArgs e)
        {
            Expression exp = new Expression();
            var editor = new ExpressionEditor();
            var result = EditExpression(null);
            if (result != null)
            {
                plugin.Settings.Expressions.Add(result);
            }
            this.lstExpressions.RefreshItems();
            plugin.SaveSettings();
        }

        private void btnEditExpression_Click(object sender, EventArgs e)
        {
            if (this.lstExpressions.SelectedItem != null)
            {

                Expression exp = this.lstExpressions.SelectedItem as Expression;
                var result = EditExpression(exp);
                if (result != null)
                {
                    plugin.Settings.Expressions.Remove(exp);
                    plugin.Settings.Expressions.Add(result);
                    plugin.Settings.Expressions.OrderBy(i => i.Name);
                }
                this.lstExpressions.RefreshItems();
            }
            plugin.SaveSettings();
        }

        private Expression EditExpression(Expression current)
        {
            var editor = new ExpressionEditor();
            var result = editor.ShowDialog(current ?? new Expression() { Name = "NewExpression" }, manager);
            if (result != null)
            {

                bool ok = false;
                while (!ok)
                {
                    ok = true;
                    if (result.Name == string.Empty)
                    {
                        MessageBox.Show("Please give a name to your expression");
                        ok = false;
                    }
                    else if (plugin.Settings.Expressions.Exists(i => i.Name.Equals(result.Name) && i != current))
                    {
                        MessageBox.Show("An expression with this name already exists");
                        ok = false;
                    }

                    if (ok) break;

                    result = editor.ShowDialog(result, manager);
                    if (result == null)
                    {
                        return null;
                    }
                }
            }
            return result;


        }

        private void btnRemoveExpression_Click(object sender, EventArgs e)
        {
            if (this.lstExpressions.SelectedItem != null)
            {
                Expression exp = this.lstExpressions.SelectedItem as Expression;
                if (exp != null)
                {
                    plugin.Settings.Expressions.Remove(exp);
                }
                this.lstExpressions.RefreshItems();
            }
            plugin.SaveSettings();
        }
    }
}
