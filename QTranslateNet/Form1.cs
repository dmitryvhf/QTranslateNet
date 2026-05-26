using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using GoogleTranslateServiceLibrary;

using GrokAITranslateServiceLibrary;

using LaraTranslateServiceLibrary;

using MicrosoftTranslateServiceLibrary;

using QTranslateNet.Core;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

using ReversoTranslateServiceLibrary;

using YandexTranslateServiceLibrary;

namespace QTranslateNet
{
    public partial class Form1 : Form
    {
        private ITranslateService? _currentTranslateService;

        private readonly Dictionary<String, ITranslateService> _translateServices
             = new Dictionary<string, ITranslateService>();

        public Form1()
        {
            InitializeComponent();

            InitializeTranslateServices();

#if DEBUG
            textBoxFrom.Text = "home";
#else
            textBoxFrom.Text = String.Empty;
#endif
        }

        #region GUI event methods

        private void BtnTranslate_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = String.Empty;

            // Step: validation
            if (_currentTranslateService == null)
            {
                toolStripStatusLabel1.Text = "[ERR] Не выбран сервис перевода";

                return;
            }

            if (String.IsNullOrWhiteSpace(textBoxFrom.Text)
                || comboBoxFrom.SelectedValue == null
                || comboBoxTo.SelectedValue == null)
            {
                return;
            }

            string langFrom = comboBoxFrom.SelectedValue.ToString()!;
            string langTo = comboBoxTo.SelectedValue.ToString()!;
            string originalText = textBoxFrom.Text;

            TranslateService.Translate(
                _currentTranslateService,
                originalText,
                langFrom,
                langTo,
                toolStripStatusLabel1,
                textBoxTo);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            textBoxFrom.Text = String.Empty;
            textBoxTo.Text = String.Empty;

            toolStripStatusLabel1.Text = String.Empty;
        }

        /// <summary>
        ///     Поменять местами выбранные языки перевода
        /// </summary>
        private void BtnSwapLang_Click(object sender, EventArgs e)
        {
            (comboBoxTo.Text, comboBoxFrom.Text) = (comboBoxFrom.Text, comboBoxTo.Text);
        }

        private void TranslateServiceCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            RadioButton control = (RadioButton)sender!;

            if (_currentTranslateService != null
                && control.AccessibleName == _currentTranslateService.GetServiceHeader().AccessibleName)
            {
                // Сервис уже выбран
                return;
            }

            _currentTranslateService = _translateServices[control.AccessibleName!];

            InitializeLanguageComboBoxes(_currentTranslateService);

            toolStripStatusLabel1.Text = "[INFO] Выбран сервис: " + _currentTranslateService.GetServiceHeader().Name;
        }

        #endregion

        #region Private methods

        private void InitializeLanguageComboBoxes(ITranslateService currentTranslateService)
        {
            string[] serviceLangs = currentTranslateService.GetServiceHeader().SupportedLanguages;

            SupportedLanguage[] supportedLanguages = MyConstants.SupportedLanguage
                .ToArray();
            foreach (SupportedLanguage item in supportedLanguages)
            {
                item.Enabled = serviceLangs.Contains(item.Code);
            }

            if (currentTranslateService.GetServiceHeader().Capabilities.Contains(Capability.DetectLanguage))
            {
                supportedLanguages = new SupportedLanguage[]
                {
                   MyConstants.AutoDetectLanguage
                }
                .Concat(supportedLanguages)
                .ToArray();
            }

            comboBoxFrom.ValueMember = nameof(SupportedLanguage.Code);
            comboBoxFrom.DisplayMember = nameof(SupportedLanguage.Name);
            comboBoxFrom.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxFrom.DrawItem += ComboBox_DrawItem!;
            comboBoxFrom.SelectedIndexChanged += ComboBox_SelectedIndexChanged!;
            comboBoxFrom.DataSource = new BindingSource() { DataSource = supportedLanguages };

            comboBoxTo.ValueMember = nameof(SupportedLanguage.Code);
            comboBoxTo.DisplayMember = nameof(SupportedLanguage.Name);
            comboBoxTo.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxTo.DrawItem += ComboBox_DrawItem!;
            comboBoxTo.SelectedIndexChanged += ComboBox_SelectedIndexChanged!;
            comboBoxTo.DataSource = new BindingSource() { DataSource = supportedLanguages };

            // Temporary realization
            comboBoxFrom.Text = "English";
            comboBoxTo.Text = "Russian";
        }

        private void InitializeTranslateServices()
        {
            // Available translate services
            // Temporary manual initialization

            ITranslateService googleTranslateService = new GoogleTranslateService();
            InitTranslateServiceControl(googleTranslateService);

            ITranslateService yandexTranslateService = new YandexTranslateService();
            InitTranslateServiceControl(yandexTranslateService);

            ITranslateService grokAITranslateService = new GrokAITranslateService();
            InitTranslateServiceControl(grokAITranslateService);

            ITranslateService laraTranslateService = new LaraTranslateService();
            InitTranslateServiceControl(laraTranslateService);

            ITranslateService reversoTranslateService = new ReversoTranslateService();
            InitTranslateServiceControl(reversoTranslateService);

            ITranslateService microsoftTranslateService = new MicrosoftTranslateService();
            InitTranslateServiceControl(microsoftTranslateService);
        }

        private void InitTranslateServiceControl(ITranslateService translateService)
        {
            RadioButton control = CreateDefaultServiceSelectorControl();
            ServiceHeader serviceHeader = translateService.GetServiceHeader();

            using (MemoryStream ms = new MemoryStream(serviceHeader.ServiceIco))
            {
                // control.Name = "ckb_" + serviceHeader.AccessibleName;
                control.Image = Image.FromStream(ms);
                control.Text = serviceHeader.Name;
                control.AccessibleName = serviceHeader.AccessibleName;

                toolTip1.SetToolTip(control, serviceHeader.Name + "\n" + serviceHeader.Info);
            }

            _translateServices.Add(serviceHeader.AccessibleName, translateService);

            control.CheckedChanged += TranslateServiceCheckBox_CheckedChanged;

            flowLayoutPanel1.Controls.Add(control);
        }

        private static RadioButton CreateDefaultServiceSelectorControl()
        {
            RadioButton control = new RadioButton
            {
                Appearance = Appearance.Button,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0),
                Padding = new Padding(0),
                AutoSize = true,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight,
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            return control;
        }

        /// <summary>
        ///     Событие для отрисовки элемента списка
        ///     с неподдерживаемым языком перевода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            ComboBox comboBox = (ComboBox)sender;
            SupportedLanguage item = (SupportedLanguage)comboBox.Items[e.Index]!;
            Brush brush = Brushes.Black;

            if (!item.Enabled)
            {
                brush = Brushes.Gray;
            }

            e.DrawBackground();
            e.Graphics.DrawString(item.Name, e.Font, brush, e.Bounds);
            e.DrawFocusRectangle();
        }

        /// <summary>
        ///     Событие для блокирования выбора неподдерживаемого языка перевода
        /// </summary>
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            SupportedLanguage item = (SupportedLanguage)comboBox.SelectedItem!;

            if (comboBox.SelectedIndex != -1 && !item.Enabled)
            {
                // Revert to previous selection or clear it
                comboBox.SelectedItem = comboBox.Tag;
            }
            else
            {
                comboBox.Tag = item;
            }
        }

        #endregion
    }
}
