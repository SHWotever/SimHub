using System;
using System.Linq;
using System.Windows.Forms;

namespace ACHub.Plugins.UI
{
    /// <summary>
    /// Plungin Manager Settings UI
    /// </summary>
    public partial class PluginManagerUI : UserControl
    {
        private PluginManager manager;

        /// <summary>
        /// Constructor
        /// </summary>
        public PluginManagerUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="manager"></param>
        public void Init(PluginManager manager)
        {
            this.manager = manager;

            BindInputMappings();
            BindEventMappings();

            foreach (var plugin in manager.inputPlugins)
            {
                var control = plugin.GetSettingsControl(manager);
                if (control == null)
                {
                    control = new PluginDefaultUI();
                }
                this.InputSettings.AddPlugin(plugin.Name, plugin.Version, control);
            }
            foreach (var plugin in manager.dataPlugins)
            {
                var control = plugin.GetSettingsControl(manager);
                if (control == null)
                {
                    control = new PluginDefaultUI();
                }
                this.InputSettings.AddPlugin(plugin.Name, plugin.Version, control);
            }
            foreach (var plugin in manager.outputPlugins)
            {
                var control = plugin.GetSettingsControl(manager);
                if (control == null)
                {
                    control = new PluginDefaultUI();
                }
                this.InputSettings.AddPlugin(plugin.Name, plugin.Version, control);
            }
        }

        private void btnAddActionMapping_Click(object sender, EventArgs e)
        {
            var mapping = (new MappingPicker()).ShowInputDialog(manager,
                manager.GeneratedInputs.Select(i => i.Key).ToList(),
                manager.GeneratedActions.Select(i => i.Key).ToList()
            );

            if (mapping != null)
            {
                manager.Settings.InputActionMapping.Add(mapping);
                BindInputMappings();
            }
            manager.SaveSettings();
        }

        private void BindInputMappings()
        {
            var idx = this.lstInputMappings.SelectedIndex;
            this.lstInputMappings.Items.Clear();
            this.lstInputMappings.Items.AddRange(manager.Settings.InputActionMapping.ToArray());
            manager.SaveSettings();
            try
            {
                if (this.lstInputMappings.Items.Count > 0)
                {
                    this.lstInputMappings.SelectedIndex = Math.Min(this.lstInputMappings.Items.Count - 1, idx);
                }
            }
            catch { }
        }

        private void BindEventMappings()
        {
            var idx = this.lstEventMapping.SelectedIndex;
            this.lstEventMapping.Items.Clear();
            this.lstEventMapping.Items.AddRange(manager.Settings.EventActionMapping.ToArray());
            manager.SaveSettings();
            try
            {
                if (this.lstEventMapping.Items.Count > 0)
                {
                    this.lstEventMapping.SelectedIndex = Math.Min(this.lstEventMapping.Items.Count - 1, idx);
                }
            }
            catch { }
        }

        private void btnEditInputMapping_Click(object sender, EventArgs e)
        {
            if (lstInputMappings.SelectedItem != null)
            {
                var mapping = (new MappingPicker()).ShowInputDialog(manager,
                    manager.GeneratedInputs.Select(i => i.Key).ToList(),
                    manager.GeneratedActions.Select(i => i.Key).ToList(),
                    (lstInputMappings.SelectedItem as InputMapping)
                );
                if (mapping != null)
                {
                    (lstInputMappings.SelectedItem as InputMapping).Trigger = mapping.Trigger;
                    (lstInputMappings.SelectedItem as InputMapping).Target = mapping.Target;
                    (lstInputMappings.SelectedItem as InputMapping).PressType = mapping.PressType;
                    BindInputMappings();
                }
            }
            manager.SaveSettings();
        }

        private void btnRemoveInputMapping_Click(object sender, EventArgs e)
        {
            if (lstInputMappings.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure to want to remove this mapping ?", "Remove mapping", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    manager.Settings.InputActionMapping.Remove(lstInputMappings.SelectedItem as InputMapping);
                    BindInputMappings();
                }
            }
            manager.SaveSettings();
        }

        private void btnRemoveEventMapping_Click(object sender, EventArgs e)
        {
            if (lstEventMapping.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure to want to remove this mapping ?", "Remove mapping", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    manager.Settings.EventActionMapping.Remove(lstEventMapping.SelectedItem as Mapping);
                    BindEventMappings();
                }
            }
            manager.SaveSettings();
        }

        private void btnEditEventMapping_Click(object sender, EventArgs e)
        {
            if (lstEventMapping.SelectedItem != null)
            {
                var mapping = (new MappingPicker()).ShowDialog(manager,
                    manager.GeneratedEvents.Select(i => i.Key).ToList(),
                    manager.GeneratedActions.Select(i => i.Key).ToList(),
                    (lstEventMapping.SelectedItem as Mapping)
                );
                if (mapping != null)
                {
                    (lstEventMapping.SelectedItem as Mapping).Trigger = mapping.Trigger;
                    (lstEventMapping.SelectedItem as Mapping).Target = mapping.Target;
                    BindEventMappings();
                }
            }
            manager.SaveSettings();
        }

        private void btnAddEventMapping_Click(object sender, EventArgs e)
        {
            var mapping = (new MappingPicker()).ShowDialog(manager,
              manager.GeneratedEvents.Select(i => i.Key).ToList(),
              manager.GeneratedActions.Select(i => i.Key).ToList()
          );

            if (mapping != null)
            {
                manager.Settings.EventActionMapping.Add(mapping);
                BindEventMappings();
            }
            manager.SaveSettings();
        }

        private void btnUpInput_Click(object sender, EventArgs e)
        {
            if (this.lstInputMappings.SelectedItem != null)
            {
                InputMapping exp = this.lstInputMappings.SelectedItem as InputMapping;
                var oldidx = manager.Settings.InputActionMapping.IndexOf(exp);
                if (oldidx > 0)
                {
                    manager.Settings.InputActionMapping.Remove(exp);
                    manager.Settings.InputActionMapping.Insert(oldidx - 1, exp);
                    BindInputMappings();
                    manager.SaveSettings();
                    this.lstInputMappings.SelectedItem = exp;
                }
            }
        }

        private void btnDownInput_Click(object sender, EventArgs e)
        {
            if (this.lstInputMappings.SelectedItem != null)
            {
                InputMapping exp = this.lstInputMappings.SelectedItem as InputMapping;
                var oldidx = manager.Settings.InputActionMapping.IndexOf(exp);
                if (oldidx < manager.Settings.InputActionMapping.Count - 1)
                {
                    manager.Settings.InputActionMapping.Remove(exp);
                    manager.Settings.InputActionMapping.Insert(oldidx + 1, exp);
                    BindInputMappings();
                    manager.SaveSettings();
                    this.lstInputMappings.SelectedItem = exp;
                }
            }
        }

        private void btnUpEvent_Click(object sender, EventArgs e)
        {
            if (this.lstEventMapping.SelectedItem != null)
            {
                Mapping exp = this.lstEventMapping.SelectedItem as Mapping;
                var oldidx = manager.Settings.EventActionMapping.IndexOf(exp);
                if (oldidx > 0)
                {
                    manager.Settings.EventActionMapping.Remove(exp);
                    manager.Settings.EventActionMapping.Insert(oldidx - 1, exp);
                    BindEventMappings();
                    manager.SaveSettings();
                    this.lstEventMapping.SelectedItem = exp;
                }
            }
        }

        private void btnDownEvent_Click(object sender, EventArgs e)
        {
            if (this.lstEventMapping.SelectedItem != null)
            {
                Mapping exp = this.lstEventMapping.SelectedItem as Mapping;
                var oldidx = manager.Settings.EventActionMapping.IndexOf(exp);
                if (oldidx < manager.Settings.EventActionMapping.Count - 1)
                {
                    manager.Settings.EventActionMapping.Remove(exp);
                    manager.Settings.EventActionMapping.Insert(oldidx + 1, exp);
                    BindEventMappings();
                    manager.SaveSettings();
                    this.lstEventMapping.SelectedItem = exp;
                }
            }
        }
    }
}