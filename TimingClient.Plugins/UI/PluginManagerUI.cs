using System;
using System.Linq;
using System.Windows.Forms;

namespace TimingClient.Plugins.UI
{
    public partial class PluginManagerUI : UserControl
    {
        private PluginManager manager;

        public PluginManagerUI()
        {
            InitializeComponent();
        }

        public void Init(PluginManager manager)
        {
            this.manager = manager;

            BindInputMappings();

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
                this.DataSettings.AddPlugin(plugin.Name, plugin.Version, control);
            }
            foreach (var plugin in manager.outputPlugins)
            {
                var control = plugin.GetSettingsControl(manager);
                if (control == null)
                {
                    control = new PluginDefaultUI();
                }
                this.outputSettings.AddPlugin(plugin.Name, plugin.Version, control);
            }
        }

        private void btnAddActionMapping_Click(object sender, EventArgs e)
        {
            var mapping = (new MappingPicker()).ShowDialog(manager,
                manager.GeneratedInputs.Select(i => i.Key).ToList(),
                manager.GeneratedActions.Select(i => i.Key).ToList()
            );

            if (mapping != null)
            {
                manager.Settings.InputActionMapping.Add(mapping);
                BindInputMappings();
            }
        }

        private void BindInputMappings()
        {
            var idx = this.lstInputMappings.SelectedIndex;
            this.lstInputMappings.Items.Clear();
            this.lstInputMappings.Items.AddRange(manager.Settings.InputActionMapping.ToArray());
            try
            {
                if (this.lstInputMappings.Items.Count > 0)
                {
                    this.lstInputMappings.SelectedIndex = Math.Max(this.lstInputMappings.Items.Count - 1, idx);
                }
            }
            catch { }
        }

        private void BindEventMappings()
        {
            var idx = this.lstEventMapping.SelectedIndex;
            this.lstEventMapping.Items.Clear();
            this.lstEventMapping.Items.AddRange(manager.Settings.EventActionMapping.ToArray());
            try
            {
                if (this.lstEventMapping.Items.Count > 0)
                {
                    this.lstEventMapping.SelectedIndex = Math.Max(this.lstEventMapping.Items.Count - 1, idx);
                }
            }
            catch { }
        }

        private void btnEditInputMapping_Click(object sender, EventArgs e)
        {
            if (lstInputMappings.SelectedItem != null)
            {
                var mapping = (new MappingPicker()).ShowDialog(manager,
                    manager.GeneratedInputs.Select(i => i.Key).ToList(),
                    manager.GeneratedActions.Select(i => i.Key).ToList(),
                    (lstInputMappings.SelectedItem as Mapping)
                );
                if (mapping != null)
                {
                    (lstInputMappings.SelectedItem as Mapping).Trigger = mapping.Trigger;
                    (lstInputMappings.SelectedItem as Mapping).Target = mapping.Target;
                    BindInputMappings();
                }
            }
        }

        private void btnRemoveInputMapping_Click(object sender, EventArgs e)
        {
            if (lstInputMappings.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure to want to remove this mapping ?", "Remove mapping", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    manager.Settings.InputActionMapping.Remove(lstInputMappings.SelectedItem as Mapping);
                    BindInputMappings();
                }
            }
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
        }
    }
}