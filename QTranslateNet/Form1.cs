using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using GoogleTranslateServiceLibrary;

using GrokAITranslateServiceLibrary;

using LaraTranslateServiceLibrary;

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

            if (String.IsNullOrWhiteSpace(textBoxFrom.Text))
            {
                return;
            }

            string langFrom = comboBoxFrom.SelectedValue!.ToString()!;
            string langTo = comboBoxTo.SelectedValue!.ToString()!;
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
            textBoxFrom.Text = "home";// String.Empty;
            textBoxTo.Text = String.Empty;

            toolStripStatusLabel1.Text = String.Empty;
        }

        /// <summary>
        ///     Поменять местами выбранные языки перевода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSwapLang_Click(object sender, EventArgs e)
        {
            (comboBoxTo.Text, comboBoxFrom.Text) = (comboBoxFrom.Text, comboBoxTo.Text);
        }

        private void TranslateServiceCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = String.Empty;

            CheckBox control = (CheckBox)sender!;

            if (_currentTranslateService != null
                && control.AccessibleName == _currentTranslateService.GetServiceHeader().AccessibleName)
            {
                // Сервис уже выбран

                control.CheckState = CheckState.Checked;
                return;
            }

            foreach (Control flpControl in flowLayoutPanel1.Controls)
            {
                if (flpControl is not CheckBox flpCheckBox)
                {
                    continue;
                }

                flpCheckBox.CheckedChanged -= TranslateServiceCheckBox_CheckedChanged;

                flpCheckBox.CheckState = CheckState.Unchecked;
            }

            control.CheckState = CheckState.Checked;

            _currentTranslateService = _translateServices[control.AccessibleName!];

            InitializeLanguageComboBoxes(_currentTranslateService);

            foreach (Control flpControl in flowLayoutPanel1.Controls)
            {
                if (flpControl is not CheckBox flpCheckBox)
                {
                    continue;
                }

                flpCheckBox.CheckedChanged += TranslateServiceCheckBox_CheckedChanged;
            }
        }

        #endregion

        #region Private methods

        private void InitializeLanguageComboBoxes(ITranslateService currentTranslateService)
        {
            string[] serviceLangs = currentTranslateService.GetServiceHeader().SupportedLanguages;

            SupportedLanguage[] supportedLanguages = serviceLangs.Length == 0
                ? MyConstants.SupportedLanguage
                : MyConstants.SupportedLanguage
                    .Where(x => serviceLangs.Contains(x.Code))
                    .ToArray();

            comboBoxFrom.DataSource = new BindingSource() { DataSource = supportedLanguages };
            comboBoxFrom.ValueMember = nameof(SupportedLanguage.Code);
            comboBoxFrom.DisplayMember = nameof(SupportedLanguage.Name);

            comboBoxTo.DataSource = new BindingSource() { DataSource = supportedLanguages };
            comboBoxTo.ValueMember = nameof(SupportedLanguage.Code);
            comboBoxTo.DisplayMember = nameof(SupportedLanguage.Name);

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
        }

        private void InitTranslateServiceControl(ITranslateService translateService)
        {
            CheckBox control = CreateDefaultCheckBox();
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

        private static CheckBox CreateDefaultCheckBox()
        {
            CheckBox control = new CheckBox
            {
                Appearance = Appearance.Button,
                AutoSize = true,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                UseVisualStyleBackColor = true
            };

            return control;
        }

        #endregion
    }
}
