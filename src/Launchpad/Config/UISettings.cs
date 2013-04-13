using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LaunchPad.Forms;
using UpdaterCore;

namespace LaunchPad
{
	public class ComboSetting<T> : Setting
	{
		public ComboSetting (ComboBox control,
			Func<T, string> fromValue, Func<string, T> toValue)
			: base (SettingType.Combo, control)
		{
			this.fromValue = fromValue;
			this.toValue = toValue;

			control.DataSource = Enum.GetValues (typeof(T));
			control.MaxDropDownItems = control.Items.Count;
			control.SelectedValueChanged += (s, ev) => onChanged();
		}

		public override string Value
		{
			get {
				var value = (T)control.SelectedValue;
				return fromValue (value);
			}
			set {
				control.SelectedItem = toValue (value);
			}
		}

		private ComboBox control {get { return (ComboBox)Control; }}
		private readonly Func<T, string> fromValue;
		private readonly Func<string, T> toValue;
	}

	public class BoolSetting : Setting
	{
		public BoolSetting (CheckBox control)
			: base (SettingType.Boolean, control)
		{
			control.CheckedChanged += (s, e) => onChanged();
		}

		public override string Value
		{
			get { return ((CheckBox)Control).Checked.ToString(); }
			set {
				var result = value.Contains ("yes");
				((CheckBox)Control).Checked = result;
			}
		}
	}

	public class FileSetting : Setting
	{
		public FileSetting (FileSelector control)
			: base (SettingType.File, control)
		{
			control.inPath.TextChanged += (s, e) => onChanged();
		}

		public override string Value
		{
			get { return ((FileSelector)Control).Path; }
			set { ((FileSelector)Control).Path = value; }
		}
	}

	public class TextSetting : Setting
	{
		public TextSetting (TextBox text)
			: base (SettingType.Text, text)
		{
			text.TextChanged += (s, e) => onChanged();
		}

		public override string Value
		{
			get { return ((TextBox)Control).Text; }
			set { ((TextBox)Control).Text = value; }
		}
	}

	public class Setting
	{
		public Setting (SettingType type, Object control)
		{
			this.Type = type;
			this.Control = control;
		}

		public virtual string Value { get; set; }

		public SettingType Type;
		public Object Control;
		public Action Changed;

		protected void onChanged()
		{
			if (Changed != null)
				Changed();
		}
	}

	public enum SettingType
	{
		Text,
		Boolean,
		File,
		Combo
	}
}
