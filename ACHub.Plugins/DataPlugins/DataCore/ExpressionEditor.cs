using ACToolsUtilities;
using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Windows.Forms;

namespace ACHub.Plugins.DataPlugins.DataCore
{
    /// <summary>
    /// Expression editor form
    /// </summary>
    public partial class ExpressionEditor : Form
    {
        private PluginManager manager;

        /// <summary>
        /// CTOR
        /// </summary>
        public ExpressionEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Edit expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public Expression ShowDialog(Expression expression, PluginManager manager)
        {
            this.manager = manager;
            this.txtName.Text = expression.Name;
            this.txtAssemblies.Text = string.Join("\r\"n", expression.Assemblies);
            this.txtCode.Text = expression.Code;

            if (this.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return new Expression
                {
                    Name = this.txtName.Text,
                    Assemblies = this.txtAssemblies.Lines.Where(i => !string.IsNullOrWhiteSpace(i)).OrderBy(i => i).ToList(),
                    Code = this.txtCode.Text
                };
            }
            return null;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var assemblies = this.txtAssemblies.Lines.Where(i => !string.IsNullOrWhiteSpace(i)).OrderBy(i => i).ToList();
            assemblies.Add(typeof(PluginManager).Assembly.Location);
            assemblies.Add(typeof(ACSharedMemory.ACManager).Assembly.Location);

            var result = EvalProvider.EvalCode<PluginManager, object>(manager, this.txtCode.Text, null, assemblies.Distinct().ToArray());
            this.txtErrors.Text = string.Join("\r\n", result.CompilerResults.Errors.Cast<CompilerError>().Select(i => i.Line + " : " + i.ErrorText));
            this.txtGeneratedClass.IsReadOnly = false;
            this.txtGeneratedClass.Text = result.GeneratedClass;
            this.txtGeneratedClass.IsReadOnly = true;

            if (result.TypedMethod != null)
            {
                try
                {
                    this.txtResult.Text = "";
                    this.txtResult.Text = string.Format("{0}", result.TypedMethod(manager));
                }
                catch (Exception ex)
                {
                    this.txtErrors.Text = "Error while running method :\r\n" + ex.ToString();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.txtDump.Text = string.Empty;
            foreach (var prop in this.manager.GeneratedProperties)
            {
                this.txtDump.Text += string.Format("{0}\t{2} [{1}]\r\n", prop.Key, prop.Value.Key, prop.Value.Value);
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            var i = e.KeyChar;
            if (i == '\b') { return; }
            if ((i >= 'a' && i <= 'z') || (i >= 'A' && i <= 'Z') || (i >= '0' && i <= '9') || i == '.')
            {
            }
            else { e.Handled = true; }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            txtName.Text = new String(txtName.Text.Where(i => (i >= 'a' && i <= 'z') || (i >= 'A' && i <= 'Z') || (i >= '0' && i <= '9') || i == '.').ToArray());
        }
    }
}