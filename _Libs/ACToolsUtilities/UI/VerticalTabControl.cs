using System.Linq;

namespace ACToolsUtilities.UI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class VerticalTabControl : TabControl
    {
        /// <summary>Specifies the alignment of the tabs on a <see cref="VerticalTabControl"/>.</summary>
        public enum VerticalTabAlignment
        {
            /// <summary>Aligns tabs on the left side.</summary>
            Left,

            /// <summary>Aligns tabs on the right side.</summary>
            Right
        }

        private bool customItemSize;

        /// <summary>Initializes a new instance of the <see cref="VerticalTabControl"/> class.</summary>
        public VerticalTabControl()
            : base()
        {
            // set the properties on MyBase
            base.SizeMode = TabSizeMode.Fixed;
            base.Alignment = System.Windows.Forms.TabAlignment.Left;
            base.DrawMode = TabDrawMode.OwnerDrawFixed;

            // set the properties on Me
            this.Alignment = VerticalTabAlignment.Left;
            customItemSize = false;
        }

        /// <summary>
        /// Gets or sets the area of the control (for example, along the left) where the tabs are aligned.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Windows.Forms.VerticalTabControl.TabAlignment" /> values. The default is Left.</returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   </PermissionSet>
        [DefaultValue(typeof(VerticalTabControl.VerticalTabAlignment), "Left")]
        [Description("Determines whether the tabs appear on the left or right side of the Control.")]
        public new VerticalTabAlignment Alignment
        {
            get { return base.Alignment == TabAlignment.Left ? VerticalTabAlignment.Left : VerticalTabAlignment.Right; }
            set
            {
                if ((value != this.Alignment))
                {
                    if (value == VerticalTabAlignment.Left)
                    {
                        base.Alignment = TabAlignment.Left;
                    }
                    else
                    {
                        base.Alignment = TabAlignment.Right;
                    }

                    this.RecreateHandle();
                }
            }
        }

        /// <summary>Gets or sets the images to display on the control's tabs.</summary>
        /// <returns>An <see cref="T:System.Windows.Forms.ImageList" /> that specifies the images to display on the tabs.</returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   </PermissionSet>
        [Description("The ImageList object from which this tab takes its images.")]
        public new ImageList ImageList
        {
            get { return base.ImageList; }
            set
            {
                if (!object.ReferenceEquals(value, base.ImageList))
                {
                    if (base.ImageList != null)
                    {
                        base.ImageList.RecreateHandle -= RecreatingImageListHandle;
                    }

                    base.ImageList = value;

                    if (value != null)
                    {
                        base.ImageList.RecreateHandle += RecreatingImageListHandle;
                    }

                    if (!customItemSize)
                        this.ResetItemSize();
                }
            }
        }

        /// <summary>Gets or sets the size of the control's tabs.</summary>
        /// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the tabs.</returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   </PermissionSet>
        [AmbientValue(typeof(Size), "0, 0")]
        public new Size ItemSize
        {
            // return the flipped item size
            get { return base.ItemSize.FlipWH(); }
            set
            {
                // ensure the width/height is never less than 0
                if (((value.Width < 0) || (value.Height < 0)))
                {
                    throw new ArgumentOutOfRangeException("ItemSize", "The width and/or height of the ItemSize cannot be less than 0.");
                }

                // set the flag if user has changed the item size
                value = value.FlipWH();
                customItemSize = (customItemSize) || (value != base.ItemSize);

                base.ItemSize = value;
            }
        }

        #region "ItemSize ambient value methods."

        /// <summary>Resets the <see cref="ItemSize"/> property.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetItemSize()
        {
            base.ItemSize = this.GetDefaultItemSize().FlipWH();
            customItemSize = false;
        }

        /// <summary>
        /// Determines whether the <see cref="ItemSize"/> property should be serialized.
        /// </summary>
        /// <remarks>
        /// This method indicates to designers whether the property value is different from the
        /// ambient value, in which case the designer should persist the value.
        /// </remarks>
        private bool ShouldSerializeItemSize()
        {
            return customItemSize;
        }

        #endregion "ItemSize ambient value methods."

        #region "Hidden inherited properties"

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("Not intended to be used in this class.", true)]
        public new TabDrawMode DrawMode { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("Not intended to be used in this class.", true)]
        public new bool Multiline { get; set; }

        [Browsable(false), Bindable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("Not intended to be used in this class.", true)]
        public new TabSizeMode SizeMode { get; set; }

        #endregion "Hidden inherited properties"

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TabControl.DrawItem" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> that contains the event data.
        /// </param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            dynamic g = e.Graphics;
            dynamic tab = this.TabPages[e.Index];

            // draw the white background for selected tabs
            if (this.SelectedIndex == e.Index)
            {
                g.FillRectangle(new SolidBrush(e.ForeColor), this.GetTabRect(e.Index));
            }

            // draw the tab image if there is any to be drawn
            if (TabHasValidImage(e.Index))
            {
                dynamic imageIndex = this.ImageList.Images.IndexOfKey(tab.ImageKey);
                imageIndex = imageIndex != -1 ? imageIndex : tab.ImageIndex;

                this.ImageList.Draw(g, this.GetTabImageRect(e.Index).Location, imageIndex);
            }

            // draw the text
            g.DrawString(tab.Text, this.Font, SystemBrushes.ControlText, this.GetTabTextRect(e.Index));
        }

        /// <summary>
        /// This member overrides <see cref="M:System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)" />.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // set the tab size
            this.ResetItemSize();
        }

        /// <summary>Gets the size of the tab.</summary>
        protected virtual Size GetDefaultItemSize()
        {
            if (this.TabCount == 0)
                return base.ItemSize;

            // tab height will be the height of text with ascending and descending bar
            dynamic height = TextRenderer.MeasureText("bp", this.Font).Height;
            dynamic width = this.TabPages.Cast<TabPage>().Max(t => TextRenderer.MeasureText(t.Text, this.Font).Width + (Padding.X / 2));

            // adjust height & width for cases where there is an image list
            if (this.AnyTabHasValidImage())
            {
                height = Math.Max(height, this.ImageList.ImageSize.Height);
                width += this.ImageList.ImageSize.Width + this.Padding.X;
            }

            // add the padding and return the result
            // width can't be more than 128
            return new Size(Math.Min(128, width + (2 * Padding.X)), height + (2 * Padding.Y));
        }

        /// <summary>
        /// Gets the rectangle which holds the location where the specified tab's image is drawn.
        /// </summary>
        /// <param name="index">The index of the tab whose image rectangle is to be calculated.</param>
        protected virtual Rectangle GetTabImageRect(int index)
        {
            dynamic rect = this.GetTabRect(index);
            dynamic imgSz = this.ImageList.ImageSize;

            rect.Inflate(-this.Padding.X, -this.Padding.Y);
            // remove the padding

            if (this.Alignment == VerticalTabAlignment.Left)
            {
                return new Rectangle(rect.Location, imgSz);
            }
            else
            {
                return new Rectangle(new Point(rect.Right - imgSz.Width, rect.Top), imgSz);
            }
        }

        /// <summary>
        /// Gets the rectangle which holds the location where the specified tab's text is drawn.
        /// </summary>
        /// <param name="index">The index of the tab whose text rectangle is to be calculated.</param>
        protected virtual Rectangle GetTabTextRect(int index)
        {
            dynamic textHeight = TextRenderer.MeasureText(this.TabPages[index].Text, this.Font).Height;

            dynamic rect = this.GetTabRect(index);

            rect.Inflate(-this.Padding.X, -this.Padding.Y);
            // remove the padding

            if (AnyTabHasValidImage())
            {
                dynamic horizAdjustment = this.ImageList.ImageSize.Width + this.Padding.X;

                rect.Width -= horizAdjustment;
                // reduce by image width + padding.X

                if (this.Alignment == VerticalTabAlignment.Left)
                {
                    rect.Offset(horizAdjustment, 0);
                }
            }

            return rect;
        }

        /// <summary>Determines if any tab has a valid image.</summary>
        /// <returns><c>true</c> if any tabPage has a valid image.</returns>
        private bool AnyTabHasValidImage()
        {
            for (int i = 0; i <= this.TabCount - 1; i++)
            {
                if (this.TabHasValidImage(i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Determines whether the tabPage with the specified index has valid image.</summary>
        /// <param name="index">The index of the tabPage.</param>
        /// <returns><c>true</c> if the tabPage has a valid image; otherwise false.</returns>
        private bool TabHasValidImage(int index)
        {
            dynamic tab = this.TabPages[index];

            return (this.ImageList != null && !this.ImageList.Images.Empty) && (tab.ImageIndex.IsInRange(0, this.ImageList.Images.Count - 1) || this.ImageList.Images.Keys.Contains(tab.ImageKey));
        }

        private void RecreatingImageListHandle(object sender, EventArgs e)
        {
            if (!customItemSize)
                this.ResetItemSize();
        }
    }

    /// <summary>
    /// Extension methods for the <see name="System.Windows.Forms.VerticalTabControl"/> control.
    /// </summary>
    static internal class VerticalTabControlExtensions
    {
        /// <summary>Determines whether the specified value is in the specified range.</summary>
        /// <typeparam name="T">The type of the items to be compared</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="min">The minimum bounds (inclusive) of the range.</param>
        /// <param name="max">The maximum bounds (inclusive) of the range.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is lies within the range specified
        /// <paramref name="min"/> and <paramref name="max"/> (boundaries inclusive).
        /// </returns>
        [DebuggerStepThrough()]
        public static bool IsInRange<T>(T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }

        /// <summary>Flips the width and height of the specified System.Drawing.Size variable.</summary>
        /// <param name="sz">The size variable whose width and height are to be flipped.</param>

        [DebuggerStepThrough()]
        public static Size FlipWH(this Size sz)
        {
            return new Size(sz.Height, sz.Width);
        }
    }
}