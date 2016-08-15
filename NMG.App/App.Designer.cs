using System.Drawing;
using System.Windows.Forms;

namespace NHibernateMappingGenerator
{
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.connStrTextBox = new System.Windows.Forms.TextBox();
            this.dbConnStrLabel = new System.Windows.Forms.Label();
            this.connectBtn = new System.Windows.Forms.Button();
            this.sequencesComboBox = new System.Windows.Forms.ComboBox();
            this.dbTableDetailsGridView = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PersianName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSharpType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isPrimaryKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isForeignKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isNullable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isUniqueKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.inList = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.inSort = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.inSearch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.inLookUpLabel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.inLookUpCombo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.regx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BeginOfRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndOfRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.folderTextBox = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.folderSelectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nameSpaceTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.assemblyNameTextBox = new System.Windows.Forms.TextBox();
            this.serverTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.generateAllBtn = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.basicSettingsTabPage = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tablesListBox = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.SaveMetadatButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.entityNameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pOracleOnlyOptions = new System.Windows.Forms.Panel();
            this.ownersComboBox = new System.Windows.Forms.ComboBox();
            this.lblOwner = new System.Windows.Forms.Label();
            this.advanceSettingsTabPage = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.NhibernateRadioButton = new System.Windows.Forms.RadioButton();
            this.dbmlRadioButton = new System.Windows.Forms.RadioButton();
            this.KendoCheckBox = new System.Windows.Forms.CheckBox();
            this.GenerateViewCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.DfaultCheckBox = new System.Windows.Forms.CheckBox();
            this.SortExpressionCheckBox = new System.Windows.Forms.CheckBox();
            this.DataContextTextBox = new System.Windows.Forms.TextBox();
            this.DBUtilitytextBox = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.DALReferencesTextBox = new System.Windows.Forms.TextBox();
            this.DALNameSpace = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.DALFiles = new System.Windows.Forms.CheckBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.VirtualcheckBox = new System.Windows.Forms.CheckBox();
            this.AnnotationCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.SearchModeltextBox = new System.Windows.Forms.TextBox();
            this.Namespace = new System.Windows.Forms.Label();
            this.IncludePagingCheckBox = new System.Windows.Forms.CheckBox();
            this.SearchModelGenerationCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.RefrenceRichTextBox = new System.Windows.Forms.TextBox();
            this.UseAjaxCheckBox = new System.Windows.Forms.CheckBox();
            this.GenerateLookupCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateCrudCheckBox = new System.Windows.Forms.CheckBox();
            this.AuthorizeCheckBox = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.ControllerNamespaceTextBox = new System.Windows.Forms.TextBox();
            this.InheritBaseControllerCheckBox = new System.Windows.Forms.CheckBox();
            this.GenearteControllerCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.ResourceGenerationCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBoxAttributeGeneration = new System.Windows.Forms.GroupBox();
            this.CommonResourseTextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.RangeErrorTextBox = new System.Windows.Forms.TextBox();
            this.RegxErrorTextBox = new System.Windows.Forms.TextBox();
            this.errorResourceTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.requieredTextBox = new System.Windows.Forms.TextBox();
            this.maxLengthTextBox = new System.Windows.Forms.TextBox();
            this.tablePostTextBox = new System.Windows.Forms.TextBox();
            this.tablePreTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.displayNameAttributeTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.resouceReferenceTextBox = new System.Windows.Forms.TextBox();
            this.aatributeGenerationCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.generateInFoldersCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.comboBoxForeignCollection = new System.Windows.Forms.ComboBox();
            this.wcfDataContractCheckBox = new System.Windows.Forms.CheckBox();
            this.labelForeignEntity = new System.Windows.Forms.Label();
            this.partialClassesCheckBox = new System.Windows.Forms.CheckBox();
            this.labelCLassNamePrefix = new System.Windows.Forms.Label();
            this.labelInheritence = new System.Windows.Forms.Label();
            this.textBoxClassNamePrefix = new System.Windows.Forms.TextBox();
            this.textBoxInheritence = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.autoPropertyRadioBtn = new System.Windows.Forms.RadioButton();
            this.propertyRadioBtn = new System.Windows.Forms.RadioButton();
            this.fieldRadioBtn = new System.Windows.Forms.RadioButton();
            this.useLazyLoadingCheckBox = new System.Windows.Forms.CheckBox();
            this.includeForeignKeysCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nhFluentMappingStyle = new System.Windows.Forms.RadioButton();
            this.castleMappingOption = new System.Windows.Forms.RadioButton();
            this.fluentMappingOption = new System.Windows.Forms.RadioButton();
            this.hbmMappingOption = new System.Windows.Forms.RadioButton();
            this.byCodeMappingOption = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.vbRadioButton = new System.Windows.Forms.RadioButton();
            this.cSharpRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pascalCasedRadioButton = new System.Windows.Forms.RadioButton();
            this.prefixTextBox = new System.Windows.Forms.TextBox();
            this.prefixRadioButton = new System.Windows.Forms.RadioButton();
            this.prefixLabel = new System.Windows.Forms.Label();
            this.camelCasedRadioButton = new System.Windows.Forms.RadioButton();
            this.sameAsDBRadioButton = new System.Windows.Forms.RadioButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.colorDialog3 = new System.Windows.Forms.ColorDialog();
            this.bootstrapCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dbTableDetailsGridView)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.basicSettingsTabPage.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.pOracleOnlyOptions.SuspendLayout();
            this.advanceSettingsTabPage.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBoxAttributeGeneration.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // connStrTextBox
            // 
            this.connStrTextBox.Location = new System.Drawing.Point(127, 21);
            this.connStrTextBox.Name = "connStrTextBox";
            this.connStrTextBox.Size = new System.Drawing.Size(490, 20);
            this.connStrTextBox.TabIndex = 0;
            this.connStrTextBox.Text = "Data Source=192.Data Source=192.168.15.164;Initial Catalog=SIS;User ID=pezhman;Pa" +
    "ssword=nasiri168.15.164;Initial Catalog=sis;User ID=pezhman;Password=1234";
            // 
            // dbConnStrLabel
            // 
            this.dbConnStrLabel.AutoSize = true;
            this.dbConnStrLabel.Location = new System.Drawing.Point(8, 25);
            this.dbConnStrLabel.Name = "dbConnStrLabel";
            this.dbConnStrLabel.Size = new System.Drawing.Size(109, 13);
            this.dbConnStrLabel.TabIndex = 1;
            this.dbConnStrLabel.Text = "DB Connection String";
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(898, 21);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 2;
            this.connectBtn.Text = "&Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtnClicked);
            // 
            // sequencesComboBox
            // 
            this.sequencesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sequencesComboBox.DropDownWidth = 449;
            this.sequencesComboBox.FormattingEnabled = true;
            this.sequencesComboBox.Location = new System.Drawing.Point(6, 16);
            this.sequencesComboBox.Name = "sequencesComboBox";
            this.sequencesComboBox.Size = new System.Drawing.Size(449, 21);
            this.sequencesComboBox.TabIndex = 4;
            // 
            // dbTableDetailsGridView
            // 
            this.dbTableDetailsGridView.AllowUserToAddRows = false;
            this.dbTableDetailsGridView.AllowUserToDeleteRows = false;
            this.dbTableDetailsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dbTableDetailsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dbTableDetailsGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dbTableDetailsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dbTableDetailsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.PersianName,
            this.columnDataType,
            this.cSharpType,
            this.isPrimaryKey,
            this.isForeignKey,
            this.isNullable,
            this.isUniqueKey,
            this.inList,
            this.inSort,
            this.inSearch,
            this.inLookUpLabel,
            this.inLookUpCombo,
            this.regx,
            this.BeginOfRange,
            this.EndOfRange});
            this.dbTableDetailsGridView.Location = new System.Drawing.Point(243, 16);
            this.dbTableDetailsGridView.Name = "dbTableDetailsGridView";
            this.dbTableDetailsGridView.RowHeadersVisible = false;
            this.dbTableDetailsGridView.ShowCellErrors = false;
            this.dbTableDetailsGridView.Size = new System.Drawing.Size(910, 331);
            this.dbTableDetailsGridView.TabIndex = 5;
            // 
            // columnName
            // 
            this.columnName.DataPropertyName = "Name";
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            // 
            // PersianName
            // 
            this.PersianName.DataPropertyName = "PersianName";
            this.PersianName.HeaderText = "FaName";
            this.PersianName.Name = "PersianName";
            // 
            // columnDataType
            // 
            this.columnDataType.HeaderText = "DataType";
            this.columnDataType.Name = "columnDataType";
            this.columnDataType.ReadOnly = true;
            // 
            // cSharpType
            // 
            this.cSharpType.HeaderText = "C# Type";
            this.cSharpType.Name = "cSharpType";
            this.cSharpType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cSharpType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isPrimaryKey
            // 
            this.isPrimaryKey.HeaderText = "PKey";
            this.isPrimaryKey.Name = "isPrimaryKey";
            this.isPrimaryKey.ReadOnly = true;
            // 
            // isForeignKey
            // 
            this.isForeignKey.HeaderText = "FKey";
            this.isForeignKey.Name = "isForeignKey";
            this.isForeignKey.ReadOnly = true;
            // 
            // isNullable
            // 
            this.isNullable.HeaderText = "Null";
            this.isNullable.Name = "isNullable";
            this.isNullable.ReadOnly = true;
            // 
            // isUniqueKey
            // 
            this.isUniqueKey.HeaderText = "UKey";
            this.isUniqueKey.Name = "isUniqueKey";
            this.isUniqueKey.ReadOnly = true;
            // 
            // inList
            // 
            this.inList.DataPropertyName = "InList";
            this.inList.HeaderText = "List";
            this.inList.Name = "inList";
            // 
            // inSort
            // 
            this.inSort.DataPropertyName = "InSort";
            this.inSort.HeaderText = "Sort";
            this.inSort.Name = "inSort";
            // 
            // inSearch
            // 
            this.inSearch.DataPropertyName = "InSearch";
            this.inSearch.HeaderText = "Search";
            this.inSearch.Name = "inSearch";
            // 
            // inLookUpLabel
            // 
            this.inLookUpLabel.DataPropertyName = "InLookUpLabel";
            this.inLookUpLabel.HeaderText = "LULabel";
            this.inLookUpLabel.Name = "inLookUpLabel";
            // 
            // inLookUpCombo
            // 
            this.inLookUpCombo.DataPropertyName = "InLookUpCombo";
            this.inLookUpCombo.HeaderText = "LUCombo";
            this.inLookUpCombo.Name = "inLookUpCombo";
            // 
            // regx
            // 
            this.regx.DataPropertyName = "RegX";
            this.regx.HeaderText = "Regx";
            this.regx.Name = "regx";
            // 
            // BeginOfRange
            // 
            this.BeginOfRange.DataPropertyName = "BeginOfRange";
            this.BeginOfRange.HeaderText = "BRange";
            this.BeginOfRange.Name = "BeginOfRange";
            // 
            // EndOfRange
            // 
            this.EndOfRange.DataPropertyName = "EndOfRange";
            this.EndOfRange.HeaderText = "ERange";
            this.EndOfRange.Name = "EndOfRange";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.errorLabel.ForeColor = System.Drawing.Color.Crimson;
            this.errorLabel.Location = new System.Drawing.Point(13, 203);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 20);
            this.errorLabel.TabIndex = 6;
            // 
            // folderTextBox
            // 
            this.folderTextBox.Location = new System.Drawing.Point(14, 33);
            this.folderTextBox.Name = "folderTextBox";
            this.folderTextBox.Size = new System.Drawing.Size(605, 20);
            this.folderTextBox.TabIndex = 7;
            this.folderTextBox.Text = "c:\\NHibernate Mapping File Generator";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(275, 153);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(106, 23);
            this.generateButton.TabIndex = 8;
            this.generateButton.Text = "&Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.GenerateClicked);
            // 
            // folderSelectButton
            // 
            this.folderSelectButton.Location = new System.Drawing.Point(626, 32);
            this.folderSelectButton.Name = "folderSelectButton";
            this.folderSelectButton.Size = new System.Drawing.Size(75, 23);
            this.folderSelectButton.TabIndex = 9;
            this.folderSelectButton.Text = "&Select";
            this.folderSelectButton.UseVisualStyleBackColor = true;
            this.folderSelectButton.Click += new System.EventHandler(this.folderSelectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(363, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Select the folder in which the mapping and domain files would be generated";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Namespace :";
            // 
            // nameSpaceTextBox
            // 
            this.nameSpaceTextBox.Location = new System.Drawing.Point(105, 99);
            this.nameSpaceTextBox.Name = "nameSpaceTextBox";
            this.nameSpaceTextBox.Size = new System.Drawing.Size(514, 20);
            this.nameSpaceTextBox.TabIndex = 12;
            this.nameSpaceTextBox.Text = "APS.SIS.Model";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Assembly Name :";
            // 
            // assemblyNameTextBox
            // 
            this.assemblyNameTextBox.Location = new System.Drawing.Point(105, 127);
            this.assemblyNameTextBox.Name = "assemblyNameTextBox";
            this.assemblyNameTextBox.Size = new System.Drawing.Size(514, 20);
            this.assemblyNameTextBox.TabIndex = 14;
            this.assemblyNameTextBox.Text = "APS.SIS.Model";
            // 
            // serverTypeComboBox
            // 
            this.serverTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverTypeComboBox.FormattingEnabled = true;
            this.serverTypeComboBox.Location = new System.Drawing.Point(639, 21);
            this.serverTypeComboBox.Name = "serverTypeComboBox";
            this.serverTypeComboBox.Size = new System.Drawing.Size(244, 21);
            this.serverTypeComboBox.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(151, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Select a table";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Select the sequence for the selected table";
            // 
            // generateAllBtn
            // 
            this.generateAllBtn.Location = new System.Drawing.Point(387, 153);
            this.generateAllBtn.Name = "generateAllBtn";
            this.generateAllBtn.Size = new System.Drawing.Size(106, 23);
            this.generateAllBtn.TabIndex = 18;
            this.generateAllBtn.Text = "Generate &All";
            this.generateAllBtn.UseVisualStyleBackColor = true;
            this.generateAllBtn.Click += new System.EventHandler(this.GenerateAllClicked);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.basicSettingsTabPage);
            this.mainTabControl.Controls.Add(this.advanceSettingsTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1173, 723);
            this.mainTabControl.TabIndex = 19;
            // 
            // basicSettingsTabPage
            // 
            this.basicSettingsTabPage.Controls.Add(this.label7);
            this.basicSettingsTabPage.Controls.Add(this.groupBox6);
            this.basicSettingsTabPage.Controls.Add(this.groupBox5);
            this.basicSettingsTabPage.Controls.Add(this.groupBox4);
            this.basicSettingsTabPage.Location = new System.Drawing.Point(4, 22);
            this.basicSettingsTabPage.Name = "basicSettingsTabPage";
            this.basicSettingsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.basicSettingsTabPage.Size = new System.Drawing.Size(1165, 697);
            this.basicSettingsTabPage.TabIndex = 1;
            this.basicSettingsTabPage.Text = "Basic";
            this.basicSettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "label7";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tablesListBox);
            this.groupBox6.Controls.Add(this.dbTableDetailsGridView);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 106);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1159, 350);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            // 
            // tablesListBox
            // 
            this.tablesListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.tablesListBox.FormattingEnabled = true;
            this.tablesListBox.Location = new System.Drawing.Point(3, 16);
            this.tablesListBox.Name = "tablesListBox";
            this.tablesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.tablesListBox.Size = new System.Drawing.Size(234, 331);
            this.tablesListBox.TabIndex = 6;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.SaveMetadatButton);
            this.groupBox5.Controls.Add(this.cancelButton);
            this.groupBox5.Controls.Add(this.progressBar);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.entityNameTextBox);
            this.groupBox5.Controls.Add(this.folderTextBox);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.generateAllBtn);
            this.groupBox5.Controls.Add(this.folderSelectButton);
            this.groupBox5.Controls.Add(this.assemblyNameTextBox);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.generateButton);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.nameSpaceTextBox);
            this.groupBox5.Controls.Add(this.errorLabel);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox5.Location = new System.Drawing.Point(3, 456);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1159, 238);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            // 
            // SaveMetadatButton
            // 
            this.SaveMetadatButton.Location = new System.Drawing.Point(17, 153);
            this.SaveMetadatButton.Name = "SaveMetadatButton";
            this.SaveMetadatButton.Size = new System.Drawing.Size(111, 23);
            this.SaveMetadatButton.TabIndex = 23;
            this.SaveMetadatButton.Text = "Save Metadata";
            this.SaveMetadatButton.UseVisualStyleBackColor = true;
            this.SaveMetadatButton.Click += new System.EventHandler(this.SaveMetadatButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(513, 153);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(106, 23);
            this.cancelButton.TabIndex = 22;
            this.cancelButton.Text = "Cance&l";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 187);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1150, 13);
            this.progressBar.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Entity Name :";
            // 
            // entityNameTextBox
            // 
            this.entityNameTextBox.Location = new System.Drawing.Point(105, 70);
            this.entityNameTextBox.Name = "entityNameTextBox";
            this.entityNameTextBox.Size = new System.Drawing.Size(514, 20);
            this.entityNameTextBox.TabIndex = 20;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pOracleOnlyOptions);
            this.groupBox4.Controls.Add(this.ownersComboBox);
            this.groupBox4.Controls.Add(this.lblOwner);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.dbConnStrLabel);
            this.groupBox4.Controls.Add(this.serverTypeComboBox);
            this.groupBox4.Controls.Add(this.connStrTextBox);
            this.groupBox4.Controls.Add(this.connectBtn);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1159, 103);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            // 
            // pOracleOnlyOptions
            // 
            this.pOracleOnlyOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pOracleOnlyOptions.Controls.Add(this.label5);
            this.pOracleOnlyOptions.Controls.Add(this.sequencesComboBox);
            this.pOracleOnlyOptions.Location = new System.Drawing.Point(603, 54);
            this.pOracleOnlyOptions.Name = "pOracleOnlyOptions";
            this.pOracleOnlyOptions.Size = new System.Drawing.Size(559, 43);
            this.pOracleOnlyOptions.TabIndex = 20;
            // 
            // ownersComboBox
            // 
            this.ownersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ownersComboBox.FormattingEnabled = true;
            this.ownersComboBox.Location = new System.Drawing.Point(11, 71);
            this.ownersComboBox.Name = "ownersComboBox";
            this.ownersComboBox.Size = new System.Drawing.Size(137, 21);
            this.ownersComboBox.TabIndex = 19;
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(8, 53);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(38, 13);
            this.lblOwner.TabIndex = 18;
            this.lblOwner.Text = "Owner";
            // 
            // advanceSettingsTabPage
            // 
            this.advanceSettingsTabPage.Controls.Add(this.groupBox15);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox14);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox13);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox12);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox11);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox10);
            this.advanceSettingsTabPage.Controls.Add(this.groupBoxAttributeGeneration);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox9);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox8);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox7);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox3);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox2);
            this.advanceSettingsTabPage.Controls.Add(this.groupBox1);
            this.advanceSettingsTabPage.Location = new System.Drawing.Point(4, 22);
            this.advanceSettingsTabPage.Name = "advanceSettingsTabPage";
            this.advanceSettingsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.advanceSettingsTabPage.Size = new System.Drawing.Size(1165, 697);
            this.advanceSettingsTabPage.TabIndex = 2;
            this.advanceSettingsTabPage.Text = "Preferences";
            this.advanceSettingsTabPage.UseVisualStyleBackColor = true;
            this.advanceSettingsTabPage.Click += new System.EventHandler(this.advanceSettingsTabPage_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.bootstrapCheckBox);
            this.groupBox15.Controls.Add(this.NhibernateRadioButton);
            this.groupBox15.Controls.Add(this.dbmlRadioButton);
            this.groupBox15.Controls.Add(this.KendoCheckBox);
            this.groupBox15.Controls.Add(this.GenerateViewCheckBox);
            this.groupBox15.Location = new System.Drawing.Point(728, 411);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(413, 187);
            this.groupBox15.TabIndex = 15;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "View Generation";
            // 
            // NhibernateRadioButton
            // 
            this.NhibernateRadioButton.AutoSize = true;
            this.NhibernateRadioButton.Location = new System.Drawing.Point(132, 52);
            this.NhibernateRadioButton.Name = "NhibernateRadioButton";
            this.NhibernateRadioButton.Size = new System.Drawing.Size(133, 17);
            this.NhibernateRadioButton.TabIndex = 15;
            this.NhibernateRadioButton.Text = "For NHibernate models";
            this.NhibernateRadioButton.UseVisualStyleBackColor = true;
            // 
            // dbmlRadioButton
            // 
            this.dbmlRadioButton.AutoSize = true;
            this.dbmlRadioButton.Checked = true;
            this.dbmlRadioButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.dbmlRadioButton.Location = new System.Drawing.Point(15, 52);
            this.dbmlRadioButton.Name = "dbmlRadioButton";
            this.dbmlRadioButton.Size = new System.Drawing.Size(101, 17);
            this.dbmlRadioButton.TabIndex = 14;
            this.dbmlRadioButton.TabStop = true;
            this.dbmlRadioButton.Text = "For dbml models";
            this.dbmlRadioButton.UseVisualStyleBackColor = true;
            // 
            // KendoCheckBox
            // 
            this.KendoCheckBox.AutoSize = true;
            this.KendoCheckBox.Location = new System.Drawing.Point(137, 19);
            this.KendoCheckBox.Name = "KendoCheckBox";
            this.KendoCheckBox.Size = new System.Drawing.Size(96, 17);
            this.KendoCheckBox.TabIndex = 13;
            this.KendoCheckBox.Text = "With Kendo UI";
            this.KendoCheckBox.UseVisualStyleBackColor = true;
            // 
            // GenerateViewCheckBox
            // 
            this.GenerateViewCheckBox.AutoSize = true;
            this.GenerateViewCheckBox.Checked = true;
            this.GenerateViewCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GenerateViewCheckBox.Location = new System.Drawing.Point(15, 19);
            this.GenerateViewCheckBox.Name = "GenerateViewCheckBox";
            this.GenerateViewCheckBox.Size = new System.Drawing.Size(101, 17);
            this.GenerateViewCheckBox.TabIndex = 11;
            this.GenerateViewCheckBox.Text = "Generate Views";
            this.GenerateViewCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.DfaultCheckBox);
            this.groupBox14.Controls.Add(this.SortExpressionCheckBox);
            this.groupBox14.Controls.Add(this.DataContextTextBox);
            this.groupBox14.Controls.Add(this.DBUtilitytextBox);
            this.groupBox14.Controls.Add(this.label23);
            this.groupBox14.Controls.Add(this.label22);
            this.groupBox14.Controls.Add(this.label21);
            this.groupBox14.Controls.Add(this.DALReferencesTextBox);
            this.groupBox14.Controls.Add(this.DALNameSpace);
            this.groupBox14.Controls.Add(this.label20);
            this.groupBox14.Controls.Add(this.DALFiles);
            this.groupBox14.Location = new System.Drawing.Point(218, 405);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(495, 193);
            this.groupBox14.TabIndex = 14;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Data Access Layer";
            // 
            // DfaultCheckBox
            // 
            this.DfaultCheckBox.AutoSize = true;
            this.DfaultCheckBox.Checked = true;
            this.DfaultCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DfaultCheckBox.Location = new System.Drawing.Point(6, 44);
            this.DfaultCheckBox.Name = "DfaultCheckBox";
            this.DfaultCheckBox.Size = new System.Drawing.Size(165, 17);
            this.DfaultCheckBox.TabIndex = 19;
            this.DfaultCheckBox.Text = "Use System Defaults at Insert";
            this.DfaultCheckBox.UseVisualStyleBackColor = true;
            // 
            // SortExpressionCheckBox
            // 
            this.SortExpressionCheckBox.AutoSize = true;
            this.SortExpressionCheckBox.Checked = true;
            this.SortExpressionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SortExpressionCheckBox.Location = new System.Drawing.Point(216, 19);
            this.SortExpressionCheckBox.Name = "SortExpressionCheckBox";
            this.SortExpressionCheckBox.Size = new System.Drawing.Size(124, 17);
            this.SortExpressionCheckBox.TabIndex = 18;
            this.SortExpressionCheckBox.Text = "With Sort Expression";
            this.SortExpressionCheckBox.UseVisualStyleBackColor = true;
            // 
            // DataContextTextBox
            // 
            this.DataContextTextBox.Location = new System.Drawing.Point(76, 163);
            this.DataContextTextBox.Name = "DataContextTextBox";
            this.DataContextTextBox.Size = new System.Drawing.Size(298, 20);
            this.DataContextTextBox.TabIndex = 17;
            this.DataContextTextBox.Text = "SISDataContext";
            // 
            // DBUtilitytextBox
            // 
            this.DBUtilitytextBox.Location = new System.Drawing.Point(75, 134);
            this.DBUtilitytextBox.Name = "DBUtilitytextBox";
            this.DBUtilitytextBox.Size = new System.Drawing.Size(298, 20);
            this.DBUtilitytextBox.TabIndex = 16;
            this.DBUtilitytextBox.Text = "DBUtility";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 168);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(69, 13);
            this.label23.TabIndex = 15;
            this.label23.Text = "Data Context";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 139);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(50, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "DB Utility";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 111);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(56, 13);
            this.label21.TabIndex = 13;
            this.label21.Text = "Refrences";
            // 
            // DALReferencesTextBox
            // 
            this.DALReferencesTextBox.Location = new System.Drawing.Point(75, 108);
            this.DALReferencesTextBox.Name = "DALReferencesTextBox";
            this.DALReferencesTextBox.Size = new System.Drawing.Size(298, 20);
            this.DALReferencesTextBox.TabIndex = 12;
            this.DALReferencesTextBox.Text = "using System; using System.Collections.Generic; using System.Linq; using APS.Comm" +
    "on.Statics;";
            // 
            // DALNameSpace
            // 
            this.DALNameSpace.Location = new System.Drawing.Point(76, 82);
            this.DALNameSpace.Name = "DALNameSpace";
            this.DALNameSpace.Size = new System.Drawing.Size(297, 20);
            this.DALNameSpace.TabIndex = 6;
            this.DALNameSpace.Text = "APS.SIS.DAL";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 85);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 13);
            this.label20.TabIndex = 5;
            this.label20.Text = "Namespace";
            // 
            // DALFiles
            // 
            this.DALFiles.AutoSize = true;
            this.DALFiles.Checked = true;
            this.DALFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DALFiles.Location = new System.Drawing.Point(7, 20);
            this.DALFiles.Name = "DALFiles";
            this.DALFiles.Size = new System.Drawing.Size(191, 17);
            this.DALFiles.TabIndex = 0;
            this.DALFiles.Text = "Generate DAL Files and Controllers";
            this.DALFiles.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.VirtualcheckBox);
            this.groupBox13.Controls.Add(this.AnnotationCheckBox);
            this.groupBox13.Location = new System.Drawing.Point(7, 405);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(200, 193);
            this.groupBox13.TabIndex = 13;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Annotations";
            // 
            // VirtualcheckBox
            // 
            this.VirtualcheckBox.AutoSize = true;
            this.VirtualcheckBox.Location = new System.Drawing.Point(7, 44);
            this.VirtualcheckBox.Name = "VirtualcheckBox";
            this.VirtualcheckBox.Size = new System.Drawing.Size(121, 17);
            this.VirtualcheckBox.TabIndex = 1;
            this.VirtualcheckBox.Text = "Include Virtual prefix";
            this.VirtualcheckBox.UseVisualStyleBackColor = true;
            // 
            // AnnotationCheckBox
            // 
            this.AnnotationCheckBox.AutoSize = true;
            this.AnnotationCheckBox.Checked = true;
            this.AnnotationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AnnotationCheckBox.Location = new System.Drawing.Point(7, 20);
            this.AnnotationCheckBox.Name = "AnnotationCheckBox";
            this.AnnotationCheckBox.Size = new System.Drawing.Size(156, 17);
            this.AnnotationCheckBox.TabIndex = 0;
            this.AnnotationCheckBox.Text = "Generate Annotations in file";
            this.AnnotationCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.SearchModeltextBox);
            this.groupBox12.Controls.Add(this.Namespace);
            this.groupBox12.Controls.Add(this.IncludePagingCheckBox);
            this.groupBox12.Controls.Add(this.SearchModelGenerationCheckBox);
            this.groupBox12.Location = new System.Drawing.Point(424, 299);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(289, 100);
            this.groupBox12.TabIndex = 12;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Search Model Generation";
            // 
            // SearchModeltextBox
            // 
            this.SearchModeltextBox.Enabled = false;
            this.SearchModeltextBox.Location = new System.Drawing.Point(10, 74);
            this.SearchModeltextBox.Name = "SearchModeltextBox";
            this.SearchModeltextBox.Size = new System.Drawing.Size(226, 20);
            this.SearchModeltextBox.TabIndex = 3;
            // 
            // Namespace
            // 
            this.Namespace.AutoSize = true;
            this.Namespace.Location = new System.Drawing.Point(6, 57);
            this.Namespace.Name = "Namespace";
            this.Namespace.Size = new System.Drawing.Size(64, 13);
            this.Namespace.TabIndex = 2;
            this.Namespace.Text = "Namespace";
            // 
            // IncludePagingCheckBox
            // 
            this.IncludePagingCheckBox.AutoSize = true;
            this.IncludePagingCheckBox.Location = new System.Drawing.Point(7, 37);
            this.IncludePagingCheckBox.Name = "IncludePagingCheckBox";
            this.IncludePagingCheckBox.Size = new System.Drawing.Size(177, 17);
            this.IncludePagingCheckBox.TabIndex = 1;
            this.IncludePagingCheckBox.Text = "Include Paging in Search Model";
            this.IncludePagingCheckBox.UseVisualStyleBackColor = true;
            // 
            // SearchModelGenerationCheckBox
            // 
            this.SearchModelGenerationCheckBox.AutoSize = true;
            this.SearchModelGenerationCheckBox.Checked = true;
            this.SearchModelGenerationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SearchModelGenerationCheckBox.Location = new System.Drawing.Point(7, 20);
            this.SearchModelGenerationCheckBox.Name = "SearchModelGenerationCheckBox";
            this.SearchModelGenerationCheckBox.Size = new System.Drawing.Size(144, 17);
            this.SearchModelGenerationCheckBox.TabIndex = 0;
            this.SearchModelGenerationCheckBox.Text = "Generate Search Models";
            this.SearchModelGenerationCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.RefrenceRichTextBox);
            this.groupBox11.Controls.Add(this.UseAjaxCheckBox);
            this.groupBox11.Controls.Add(this.GenerateLookupCheckBox);
            this.groupBox11.Controls.Add(this.CreateCrudCheckBox);
            this.groupBox11.Controls.Add(this.AuthorizeCheckBox);
            this.groupBox11.Controls.Add(this.label18);
            this.groupBox11.Controls.Add(this.label17);
            this.groupBox11.Controls.Add(this.ControllerNamespaceTextBox);
            this.groupBox11.Controls.Add(this.InheritBaseControllerCheckBox);
            this.groupBox11.Controls.Add(this.GenearteControllerCheckBox);
            this.groupBox11.Location = new System.Drawing.Point(720, 219);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(437, 180);
            this.groupBox11.TabIndex = 11;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Controller Generation";
            // 
            // RefrenceRichTextBox
            // 
            this.RefrenceRichTextBox.Location = new System.Drawing.Point(76, 144);
            this.RefrenceRichTextBox.Name = "RefrenceRichTextBox";
            this.RefrenceRichTextBox.Size = new System.Drawing.Size(298, 20);
            this.RefrenceRichTextBox.TabIndex = 11;
            this.RefrenceRichTextBox.Text = "using System; using System.Linq; using System.Web.Mvc; using APS.Common.Statics; " +
    "using APS.Common.Models; using APS.SIS.UI.Controllers;";
            // 
            // UseAjaxCheckBox
            // 
            this.UseAjaxCheckBox.AutoSize = true;
            this.UseAjaxCheckBox.Checked = true;
            this.UseAjaxCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseAjaxCheckBox.Location = new System.Drawing.Point(274, 41);
            this.UseAjaxCheckBox.Name = "UseAjaxCheckBox";
            this.UseAjaxCheckBox.Size = new System.Drawing.Size(68, 17);
            this.UseAjaxCheckBox.TabIndex = 9;
            this.UseAjaxCheckBox.Text = "Use Ajax";
            this.UseAjaxCheckBox.UseVisualStyleBackColor = true;
            // 
            // GenerateLookupCheckBox
            // 
            this.GenerateLookupCheckBox.AutoSize = true;
            this.GenerateLookupCheckBox.Checked = true;
            this.GenerateLookupCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GenerateLookupCheckBox.Location = new System.Drawing.Point(140, 41);
            this.GenerateLookupCheckBox.Name = "GenerateLookupCheckBox";
            this.GenerateLookupCheckBox.Size = new System.Drawing.Size(116, 17);
            this.GenerateLookupCheckBox.TabIndex = 8;
            this.GenerateLookupCheckBox.Text = "Generate LookUps";
            this.GenerateLookupCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreateCrudCheckBox
            // 
            this.CreateCrudCheckBox.AutoSize = true;
            this.CreateCrudCheckBox.Checked = true;
            this.CreateCrudCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateCrudCheckBox.Location = new System.Drawing.Point(12, 41);
            this.CreateCrudCheckBox.Name = "CreateCrudCheckBox";
            this.CreateCrudCheckBox.Size = new System.Drawing.Size(104, 17);
            this.CreateCrudCheckBox.TabIndex = 7;
            this.CreateCrudCheckBox.Text = "Generate CRUD";
            this.CreateCrudCheckBox.UseVisualStyleBackColor = true;
            // 
            // AuthorizeCheckBox
            // 
            this.AuthorizeCheckBox.AutoSize = true;
            this.AuthorizeCheckBox.Checked = true;
            this.AuthorizeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AuthorizeCheckBox.Location = new System.Drawing.Point(274, 20);
            this.AuthorizeCheckBox.Name = "AuthorizeCheckBox";
            this.AuthorizeCheckBox.Size = new System.Drawing.Size(70, 17);
            this.AuthorizeCheckBox.TabIndex = 6;
            this.AuthorizeCheckBox.Text = "Authorize";
            this.AuthorizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 144);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 5;
            this.label18.Text = "Refrences";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 117);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(64, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Namespace";
            // 
            // ControllerNamespaceTextBox
            // 
            this.ControllerNamespaceTextBox.Location = new System.Drawing.Point(76, 114);
            this.ControllerNamespaceTextBox.Name = "ControllerNamespaceTextBox";
            this.ControllerNamespaceTextBox.Size = new System.Drawing.Size(298, 20);
            this.ControllerNamespaceTextBox.TabIndex = 3;
            this.ControllerNamespaceTextBox.Text = "APS.SIS.UI.Controllers";
            // 
            // InheritBaseControllerCheckBox
            // 
            this.InheritBaseControllerCheckBox.AutoSize = true;
            this.InheritBaseControllerCheckBox.Checked = true;
            this.InheritBaseControllerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.InheritBaseControllerCheckBox.Location = new System.Drawing.Point(140, 20);
            this.InheritBaseControllerCheckBox.Name = "InheritBaseControllerCheckBox";
            this.InheritBaseControllerCheckBox.Size = new System.Drawing.Size(126, 17);
            this.InheritBaseControllerCheckBox.TabIndex = 1;
            this.InheritBaseControllerCheckBox.Text = "Inherit BaseController";
            this.InheritBaseControllerCheckBox.UseVisualStyleBackColor = true;
            // 
            // GenearteControllerCheckBox
            // 
            this.GenearteControllerCheckBox.AutoSize = true;
            this.GenearteControllerCheckBox.Checked = true;
            this.GenearteControllerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GenearteControllerCheckBox.Location = new System.Drawing.Point(12, 20);
            this.GenearteControllerCheckBox.Name = "GenearteControllerCheckBox";
            this.GenearteControllerCheckBox.Size = new System.Drawing.Size(122, 17);
            this.GenearteControllerCheckBox.TabIndex = 0;
            this.GenearteControllerCheckBox.Text = "Generate Controllers";
            this.GenearteControllerCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.ResourceGenerationCheckBox);
            this.groupBox10.Location = new System.Drawing.Point(218, 299);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(200, 100);
            this.groupBox10.TabIndex = 10;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Resource Generation";
            // 
            // ResourceGenerationCheckBox
            // 
            this.ResourceGenerationCheckBox.AutoSize = true;
            this.ResourceGenerationCheckBox.Checked = true;
            this.ResourceGenerationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ResourceGenerationCheckBox.Location = new System.Drawing.Point(7, 20);
            this.ResourceGenerationCheckBox.Name = "ResourceGenerationCheckBox";
            this.ResourceGenerationCheckBox.Size = new System.Drawing.Size(140, 17);
            this.ResourceGenerationCheckBox.TabIndex = 0;
            this.ResourceGenerationCheckBox.Text = "Generate Alll Resources";
            this.ResourceGenerationCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBoxAttributeGeneration
            // 
            this.groupBoxAttributeGeneration.Controls.Add(this.CommonResourseTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.label19);
            this.groupBoxAttributeGeneration.Controls.Add(this.label16);
            this.groupBoxAttributeGeneration.Controls.Add(this.label15);
            this.groupBoxAttributeGeneration.Controls.Add(this.RangeErrorTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.RegxErrorTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.errorResourceTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.label14);
            this.groupBoxAttributeGeneration.Controls.Add(this.requieredTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.maxLengthTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.tablePostTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.tablePreTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.label13);
            this.groupBoxAttributeGeneration.Controls.Add(this.label12);
            this.groupBoxAttributeGeneration.Controls.Add(this.label11);
            this.groupBoxAttributeGeneration.Controls.Add(this.label10);
            this.groupBoxAttributeGeneration.Controls.Add(this.displayNameAttributeTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.label9);
            this.groupBoxAttributeGeneration.Controls.Add(this.label8);
            this.groupBoxAttributeGeneration.Controls.Add(this.resouceReferenceTextBox);
            this.groupBoxAttributeGeneration.Controls.Add(this.aatributeGenerationCheckBox);
            this.groupBoxAttributeGeneration.Location = new System.Drawing.Point(720, 6);
            this.groupBoxAttributeGeneration.Name = "groupBoxAttributeGeneration";
            this.groupBoxAttributeGeneration.Size = new System.Drawing.Size(437, 207);
            this.groupBoxAttributeGeneration.TabIndex = 9;
            this.groupBoxAttributeGeneration.TabStop = false;
            this.groupBoxAttributeGeneration.Text = "Attribute Generation";
            // 
            // CommonResourseTextBox
            // 
            this.CommonResourseTextBox.Location = new System.Drawing.Point(274, 170);
            this.CommonResourseTextBox.Name = "CommonResourseTextBox";
            this.CommonResourseTextBox.Size = new System.Drawing.Size(100, 20);
            this.CommonResourseTextBox.TabIndex = 20;
            this.CommonResourseTextBox.Text = "App_Common";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(216, 175);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(51, 13);
            this.label19.TabIndex = 19;
            this.label19.Text = "Common ";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(213, 150);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "Range";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 150);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "Regx";
            // 
            // RangeErrorTextBox
            // 
            this.RangeErrorTextBox.Location = new System.Drawing.Point(274, 144);
            this.RangeErrorTextBox.Name = "RangeErrorTextBox";
            this.RangeErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.RangeErrorTextBox.TabIndex = 16;
            this.RangeErrorTextBox.Text = "RequiredRange";
            // 
            // RegxErrorTextBox
            // 
            this.RegxErrorTextBox.Location = new System.Drawing.Point(93, 143);
            this.RegxErrorTextBox.Name = "RegxErrorTextBox";
            this.RegxErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.RegxErrorTextBox.TabIndex = 15;
            this.RegxErrorTextBox.Text = "FormatError";
            // 
            // errorResourceTextBox
            // 
            this.errorResourceTextBox.Location = new System.Drawing.Point(93, 169);
            this.errorResourceTextBox.Name = "errorResourceTextBox";
            this.errorResourceTextBox.Size = new System.Drawing.Size(100, 20);
            this.errorResourceTextBox.TabIndex = 14;
            this.errorResourceTextBox.Text = "App_Errors";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 172);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 13);
            this.label14.TabIndex = 13;
            this.label14.Text = "Error Resource";
            // 
            // requieredTextBox
            // 
            this.requieredTextBox.Location = new System.Drawing.Point(274, 118);
            this.requieredTextBox.Name = "requieredTextBox";
            this.requieredTextBox.Size = new System.Drawing.Size(100, 20);
            this.requieredTextBox.TabIndex = 12;
            this.requieredTextBox.Text = "FieldIsRequired";
            // 
            // maxLengthTextBox
            // 
            this.maxLengthTextBox.Location = new System.Drawing.Point(93, 118);
            this.maxLengthTextBox.Name = "maxLengthTextBox";
            this.maxLengthTextBox.Size = new System.Drawing.Size(100, 20);
            this.maxLengthTextBox.TabIndex = 11;
            this.maxLengthTextBox.Text = "MaxLength";
            // 
            // tablePostTextBox
            // 
            this.tablePostTextBox.Location = new System.Drawing.Point(274, 91);
            this.tablePostTextBox.Name = "tablePostTextBox";
            this.tablePostTextBox.Size = new System.Drawing.Size(100, 20);
            this.tablePostTextBox.TabIndex = 10;
            // 
            // tablePreTextBox
            // 
            this.tablePreTextBox.Location = new System.Drawing.Point(93, 95);
            this.tablePreTextBox.Name = "tablePreTextBox";
            this.tablePreTextBox.Size = new System.Drawing.Size(100, 20);
            this.tablePreTextBox.TabIndex = 9;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(209, 125);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "Required";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 125);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "MaxLengh";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(206, 98);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Table Post";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Table Pre";
            // 
            // displayNameAttributeTextBox
            // 
            this.displayNameAttributeTextBox.Enabled = false;
            this.displayNameAttributeTextBox.Location = new System.Drawing.Point(144, 59);
            this.displayNameAttributeTextBox.Name = "displayNameAttributeTextBox";
            this.displayNameAttributeTextBox.Size = new System.Drawing.Size(262, 20);
            this.displayNameAttributeTextBox.TabIndex = 4;
            this.displayNameAttributeTextBox.Text = "DisplayNameLocalized";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Display Name Attribute:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Resourse Files Reference:";
            // 
            // resouceReferenceTextBox
            // 
            this.resouceReferenceTextBox.Location = new System.Drawing.Point(144, 33);
            this.resouceReferenceTextBox.Name = "resouceReferenceTextBox";
            this.resouceReferenceTextBox.Size = new System.Drawing.Size(262, 20);
            this.resouceReferenceTextBox.TabIndex = 1;
            this.resouceReferenceTextBox.Text = "APS.SIS.Model.Resource";
            // 
            // aatributeGenerationCheckBox
            // 
            this.aatributeGenerationCheckBox.AutoSize = true;
            this.aatributeGenerationCheckBox.Checked = true;
            this.aatributeGenerationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.aatributeGenerationCheckBox.Location = new System.Drawing.Point(3, 16);
            this.aatributeGenerationCheckBox.Name = "aatributeGenerationCheckBox";
            this.aatributeGenerationCheckBox.Size = new System.Drawing.Size(249, 17);
            this.aatributeGenerationCheckBox.TabIndex = 0;
            this.aatributeGenerationCheckBox.Text = "Generate All Properties Recognizable Attributes";
            this.aatributeGenerationCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.generateInFoldersCheckBox);
            this.groupBox9.Location = new System.Drawing.Point(7, 299);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(200, 100);
            this.groupBox9.TabIndex = 8;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Files";
            // 
            // generateInFoldersCheckBox
            // 
            this.generateInFoldersCheckBox.AutoSize = true;
            this.generateInFoldersCheckBox.Location = new System.Drawing.Point(7, 20);
            this.generateInFoldersCheckBox.Name = "generateInFoldersCheckBox";
            this.generateInFoldersCheckBox.Size = new System.Drawing.Size(172, 17);
            this.generateInFoldersCheckBox.TabIndex = 0;
            this.generateInFoldersCheckBox.Text = "Group generated files in folders";
            this.generateInFoldersCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.comboBoxForeignCollection);
            this.groupBox8.Controls.Add(this.wcfDataContractCheckBox);
            this.groupBox8.Controls.Add(this.labelForeignEntity);
            this.groupBox8.Controls.Add(this.partialClassesCheckBox);
            this.groupBox8.Controls.Add(this.labelCLassNamePrefix);
            this.groupBox8.Controls.Add(this.labelInheritence);
            this.groupBox8.Controls.Add(this.textBoxClassNamePrefix);
            this.groupBox8.Controls.Add(this.textBoxInheritence);
            this.groupBox8.Location = new System.Drawing.Point(212, 152);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(502, 140);
            this.groupBox8.TabIndex = 6;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Fluent Options";
            // 
            // comboBoxForeignCollection
            // 
            this.comboBoxForeignCollection.AllowDrop = true;
            this.comboBoxForeignCollection.FormattingEnabled = true;
            this.comboBoxForeignCollection.Items.AddRange(new object[] {
            "IList",
            "Iesi.Collections.Generic.ISet"});
            this.comboBoxForeignCollection.Location = new System.Drawing.Point(9, 82);
            this.comboBoxForeignCollection.Name = "comboBoxForeignCollection";
            this.comboBoxForeignCollection.Size = new System.Drawing.Size(193, 21);
            this.comboBoxForeignCollection.TabIndex = 20;
            // 
            // wcfDataContractCheckBox
            // 
            this.wcfDataContractCheckBox.AutoSize = true;
            this.wcfDataContractCheckBox.Location = new System.Drawing.Point(7, 43);
            this.wcfDataContractCheckBox.Name = "wcfDataContractCheckBox";
            this.wcfDataContractCheckBox.Size = new System.Drawing.Size(171, 17);
            this.wcfDataContractCheckBox.TabIndex = 1;
            this.wcfDataContractCheckBox.Text = "Generate WCF Data Contracts";
            this.wcfDataContractCheckBox.UseVisualStyleBackColor = true;
            // 
            // labelForeignEntity
            // 
            this.labelForeignEntity.AutoSize = true;
            this.labelForeignEntity.Location = new System.Drawing.Point(6, 66);
            this.labelForeignEntity.Name = "labelForeignEntity";
            this.labelForeignEntity.Size = new System.Drawing.Size(196, 13);
            this.labelForeignEntity.TabIndex = 6;
            this.labelForeignEntity.Text = "Preferred Foreign Entity Collection Type:";
            // 
            // partialClassesCheckBox
            // 
            this.partialClassesCheckBox.AutoSize = true;
            this.partialClassesCheckBox.Location = new System.Drawing.Point(7, 20);
            this.partialClassesCheckBox.Name = "partialClassesCheckBox";
            this.partialClassesCheckBox.Size = new System.Drawing.Size(141, 17);
            this.partialClassesCheckBox.TabIndex = 0;
            this.partialClassesCheckBox.Text = "Generate Partial Classes";
            this.partialClassesCheckBox.UseVisualStyleBackColor = true;
            // 
            // labelCLassNamePrefix
            // 
            this.labelCLassNamePrefix.AutoSize = true;
            this.labelCLassNamePrefix.Location = new System.Drawing.Point(6, 113);
            this.labelCLassNamePrefix.Name = "labelCLassNamePrefix";
            this.labelCLassNamePrefix.Size = new System.Drawing.Size(95, 13);
            this.labelCLassNamePrefix.TabIndex = 5;
            this.labelCLassNamePrefix.Text = "Class Name Prefix:";
            // 
            // labelInheritence
            // 
            this.labelInheritence.AutoSize = true;
            this.labelInheritence.Location = new System.Drawing.Point(201, 15);
            this.labelInheritence.Name = "labelInheritence";
            this.labelInheritence.Size = new System.Drawing.Size(300, 13);
            this.labelInheritence.TabIndex = 5;
            this.labelInheritence.Text = "Inheritence && Interfaces (e.g. Entity<T>, ISomeInterface<T>):  ";
            // 
            // textBoxClassNamePrefix
            // 
            this.textBoxClassNamePrefix.Location = new System.Drawing.Point(104, 110);
            this.textBoxClassNamePrefix.Name = "textBoxClassNamePrefix";
            this.textBoxClassNamePrefix.Size = new System.Drawing.Size(98, 20);
            this.textBoxClassNamePrefix.TabIndex = 4;
            // 
            // textBoxInheritence
            // 
            this.textBoxInheritence.Location = new System.Drawing.Point(204, 31);
            this.textBoxInheritence.Name = "textBoxInheritence";
            this.textBoxInheritence.Size = new System.Drawing.Size(200, 20);
            this.textBoxInheritence.TabIndex = 4;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.autoPropertyRadioBtn);
            this.groupBox7.Controls.Add(this.propertyRadioBtn);
            this.groupBox7.Controls.Add(this.fieldRadioBtn);
            this.groupBox7.Controls.Add(this.useLazyLoadingCheckBox);
            this.groupBox7.Controls.Add(this.includeForeignKeysCheckBox);
            this.groupBox7.Location = new System.Drawing.Point(6, 152);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 140);
            this.groupBox7.TabIndex = 7;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Field Or Property";
            // 
            // autoPropertyRadioBtn
            // 
            this.autoPropertyRadioBtn.AutoSize = true;
            this.autoPropertyRadioBtn.Location = new System.Drawing.Point(6, 65);
            this.autoPropertyRadioBtn.Name = "autoPropertyRadioBtn";
            this.autoPropertyRadioBtn.Size = new System.Drawing.Size(89, 17);
            this.autoPropertyRadioBtn.TabIndex = 6;
            this.autoPropertyRadioBtn.Text = "Auto Property";
            this.autoPropertyRadioBtn.UseVisualStyleBackColor = true;
            // 
            // propertyRadioBtn
            // 
            this.propertyRadioBtn.AutoSize = true;
            this.propertyRadioBtn.Location = new System.Drawing.Point(6, 42);
            this.propertyRadioBtn.Name = "propertyRadioBtn";
            this.propertyRadioBtn.Size = new System.Drawing.Size(64, 17);
            this.propertyRadioBtn.TabIndex = 5;
            this.propertyRadioBtn.Text = "Property";
            this.propertyRadioBtn.UseVisualStyleBackColor = true;
            // 
            // fieldRadioBtn
            // 
            this.fieldRadioBtn.AutoSize = true;
            this.fieldRadioBtn.Checked = true;
            this.fieldRadioBtn.Location = new System.Drawing.Point(6, 19);
            this.fieldRadioBtn.Name = "fieldRadioBtn";
            this.fieldRadioBtn.Size = new System.Drawing.Size(47, 17);
            this.fieldRadioBtn.TabIndex = 4;
            this.fieldRadioBtn.TabStop = true;
            this.fieldRadioBtn.Text = "Field";
            this.fieldRadioBtn.UseVisualStyleBackColor = true;
            // 
            // useLazyLoadingCheckBox
            // 
            this.useLazyLoadingCheckBox.AutoSize = true;
            this.useLazyLoadingCheckBox.Checked = true;
            this.useLazyLoadingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useLazyLoadingCheckBox.Location = new System.Drawing.Point(6, 85);
            this.useLazyLoadingCheckBox.Name = "useLazyLoadingCheckBox";
            this.useLazyLoadingCheckBox.Size = new System.Drawing.Size(111, 17);
            this.useLazyLoadingCheckBox.TabIndex = 7;
            this.useLazyLoadingCheckBox.Text = "Use Lazy Loading";
            this.useLazyLoadingCheckBox.UseVisualStyleBackColor = true;
            // 
            // includeForeignKeysCheckBox
            // 
            this.includeForeignKeysCheckBox.AutoSize = true;
            this.includeForeignKeysCheckBox.Location = new System.Drawing.Point(6, 105);
            this.includeForeignKeysCheckBox.Name = "includeForeignKeysCheckBox";
            this.includeForeignKeysCheckBox.Size = new System.Drawing.Size(125, 17);
            this.includeForeignKeysCheckBox.TabIndex = 8;
            this.includeForeignKeysCheckBox.Text = "Include Foreign Keys";
            this.includeForeignKeysCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nhFluentMappingStyle);
            this.groupBox3.Controls.Add(this.castleMappingOption);
            this.groupBox3.Controls.Add(this.fluentMappingOption);
            this.groupBox3.Controls.Add(this.hbmMappingOption);
            this.groupBox3.Controls.Add(this.byCodeMappingOption);
            this.groupBox3.Location = new System.Drawing.Point(503, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 140);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mapping Style";
            // 
            // nhFluentMappingStyle
            // 
            this.nhFluentMappingStyle.AutoSize = true;
            this.nhFluentMappingStyle.Location = new System.Drawing.Point(6, 88);
            this.nhFluentMappingStyle.Name = "nhFluentMappingStyle";
            this.nhFluentMappingStyle.Size = new System.Drawing.Size(135, 17);
            this.nhFluentMappingStyle.TabIndex = 9;
            this.nhFluentMappingStyle.TabStop = true;
            this.nhFluentMappingStyle.Text = "NH 3.2 Fluent Mapping";
            this.nhFluentMappingStyle.UseVisualStyleBackColor = true;
            // 
            // castleMappingOption
            // 
            this.castleMappingOption.AutoSize = true;
            this.castleMappingOption.Location = new System.Drawing.Point(6, 65);
            this.castleMappingOption.Name = "castleMappingOption";
            this.castleMappingOption.Size = new System.Drawing.Size(125, 17);
            this.castleMappingOption.TabIndex = 8;
            this.castleMappingOption.TabStop = true;
            this.castleMappingOption.Text = "Castle Active Record";
            this.castleMappingOption.UseVisualStyleBackColor = true;
            // 
            // fluentMappingOption
            // 
            this.fluentMappingOption.AutoSize = true;
            this.fluentMappingOption.Location = new System.Drawing.Point(6, 42);
            this.fluentMappingOption.Name = "fluentMappingOption";
            this.fluentMappingOption.Size = new System.Drawing.Size(98, 17);
            this.fluentMappingOption.TabIndex = 5;
            this.fluentMappingOption.Text = "Fluent Mapping";
            this.fluentMappingOption.UseVisualStyleBackColor = true;
            // 
            // hbmMappingOption
            // 
            this.hbmMappingOption.AutoSize = true;
            this.hbmMappingOption.Checked = true;
            this.hbmMappingOption.Location = new System.Drawing.Point(6, 19);
            this.hbmMappingOption.Name = "hbmMappingOption";
            this.hbmMappingOption.Size = new System.Drawing.Size(82, 17);
            this.hbmMappingOption.TabIndex = 4;
            this.hbmMappingOption.TabStop = true;
            this.hbmMappingOption.Text = ".hbm.xml file";
            this.hbmMappingOption.UseVisualStyleBackColor = true;
            // 
            // byCodeMappingOption
            // 
            this.byCodeMappingOption.AutoSize = true;
            this.byCodeMappingOption.Location = new System.Drawing.Point(6, 110);
            this.byCodeMappingOption.Name = "byCodeMappingOption";
            this.byCodeMappingOption.Size = new System.Drawing.Size(109, 17);
            this.byCodeMappingOption.TabIndex = 10;
            this.byCodeMappingOption.TabStop = true;
            this.byCodeMappingOption.Text = "By Code Mapping";
            this.byCodeMappingOption.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.vbRadioButton);
            this.groupBox2.Controls.Add(this.cSharpRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(321, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 140);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Language";
            // 
            // vbRadioButton
            // 
            this.vbRadioButton.AutoSize = true;
            this.vbRadioButton.Location = new System.Drawing.Point(6, 42);
            this.vbRadioButton.Name = "vbRadioButton";
            this.vbRadioButton.Size = new System.Drawing.Size(39, 17);
            this.vbRadioButton.TabIndex = 5;
            this.vbRadioButton.Text = "VB";
            this.vbRadioButton.UseVisualStyleBackColor = true;
            // 
            // cSharpRadioButton
            // 
            this.cSharpRadioButton.AutoSize = true;
            this.cSharpRadioButton.Checked = true;
            this.cSharpRadioButton.Location = new System.Drawing.Point(6, 19);
            this.cSharpRadioButton.Name = "cSharpRadioButton";
            this.cSharpRadioButton.Size = new System.Drawing.Size(39, 17);
            this.cSharpRadioButton.TabIndex = 4;
            this.cSharpRadioButton.TabStop = true;
            this.cSharpRadioButton.Text = "C#";
            this.cSharpRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pascalCasedRadioButton);
            this.groupBox1.Controls.Add(this.prefixTextBox);
            this.groupBox1.Controls.Add(this.prefixRadioButton);
            this.groupBox1.Controls.Add(this.prefixLabel);
            this.groupBox1.Controls.Add(this.camelCasedRadioButton);
            this.groupBox1.Controls.Add(this.sameAsDBRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 140);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generated Property Name";
            // 
            // pascalCasedRadioButton
            // 
            this.pascalCasedRadioButton.AutoSize = true;
            this.pascalCasedRadioButton.Location = new System.Drawing.Point(6, 65);
            this.pascalCasedRadioButton.Name = "pascalCasedRadioButton";
            this.pascalCasedRadioButton.Size = new System.Drawing.Size(219, 17);
            this.pascalCasedRadioButton.TabIndex = 4;
            this.pascalCasedRadioButton.Text = "Pascal Case (e.g. ThisIsMyColumnName)";
            this.pascalCasedRadioButton.UseVisualStyleBackColor = true;
            // 
            // prefixTextBox
            // 
            this.prefixTextBox.Location = new System.Drawing.Point(70, 110);
            this.prefixTextBox.Name = "prefixTextBox";
            this.prefixTextBox.Size = new System.Drawing.Size(105, 20);
            this.prefixTextBox.TabIndex = 3;
            this.prefixTextBox.Text = "m_";
            // 
            // prefixRadioButton
            // 
            this.prefixRadioButton.AutoSize = true;
            this.prefixRadioButton.Location = new System.Drawing.Point(6, 88);
            this.prefixRadioButton.Name = "prefixRadioButton";
            this.prefixRadioButton.Size = new System.Drawing.Size(212, 17);
            this.prefixRadioButton.TabIndex = 2;
            this.prefixRadioButton.Text = "Prefixed (e.g. m_ThisIsMyColumnName)";
            this.prefixRadioButton.UseVisualStyleBackColor = true;
            this.prefixRadioButton.CheckedChanged += new System.EventHandler(this.prefixCheckChanged);
            // 
            // prefixLabel
            // 
            this.prefixLabel.AutoSize = true;
            this.prefixLabel.Location = new System.Drawing.Point(22, 113);
            this.prefixLabel.Name = "prefixLabel";
            this.prefixLabel.Size = new System.Drawing.Size(42, 13);
            this.prefixLabel.TabIndex = 2;
            this.prefixLabel.Text = "Prefix : ";
            // 
            // camelCasedRadioButton
            // 
            this.camelCasedRadioButton.AutoSize = true;
            this.camelCasedRadioButton.Location = new System.Drawing.Point(6, 42);
            this.camelCasedRadioButton.Name = "camelCasedRadioButton";
            this.camelCasedRadioButton.Size = new System.Drawing.Size(212, 17);
            this.camelCasedRadioButton.TabIndex = 1;
            this.camelCasedRadioButton.Text = "Camel Case (e.g. thisIsMyColumnName)";
            this.camelCasedRadioButton.UseVisualStyleBackColor = true;
            // 
            // sameAsDBRadioButton
            // 
            this.sameAsDBRadioButton.AutoSize = true;
            this.sameAsDBRadioButton.Checked = true;
            this.sameAsDBRadioButton.Location = new System.Drawing.Point(6, 19);
            this.sameAsDBRadioButton.Name = "sameAsDBRadioButton";
            this.sameAsDBRadioButton.Size = new System.Drawing.Size(241, 17);
            this.sameAsDBRadioButton.TabIndex = 0;
            this.sameAsDBRadioButton.TabStop = true;
            this.sameAsDBRadioButton.Text = "Same as database column name (No change)";
            this.sameAsDBRadioButton.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // bootstrapCheckBox
            // 
            this.bootstrapCheckBox.AutoSize = true;
            this.bootstrapCheckBox.Checked = true;
            this.bootstrapCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bootstrapCheckBox.Location = new System.Drawing.Point(15, 79);
            this.bootstrapCheckBox.Name = "bootstrapCheckBox";
            this.bootstrapCheckBox.Size = new System.Drawing.Size(149, 17);
            this.bootstrapCheckBox.TabIndex = 16;
            this.bootstrapCheckBox.Text = "Generate Bootstrap Views";
            this.bootstrapCheckBox.UseVisualStyleBackColor = true;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 723);
            this.Controls.Add(this.mainTabControl);
            this.Name = "App";
            this.Text = "NHibernate Mapping Generator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dbTableDetailsGridView)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.basicSettingsTabPage.ResumeLayout(false);
            this.basicSettingsTabPage.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.pOracleOnlyOptions.ResumeLayout(false);
            this.pOracleOnlyOptions.PerformLayout();
            this.advanceSettingsTabPage.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBoxAttributeGeneration.ResumeLayout(false);
            this.groupBoxAttributeGeneration.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox connStrTextBox;
        private System.Windows.Forms.Label dbConnStrLabel;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.ComboBox sequencesComboBox;
        private System.Windows.Forms.DataGridView dbTableDetailsGridView;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox folderTextBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button folderSelectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameSpaceTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox assemblyNameTextBox;
        private System.Windows.Forms.ComboBox serverTypeComboBox;
        private Label label4;
        private Label label5;
        private Button generateAllBtn;
        private TabControl mainTabControl;
        private TabPage basicSettingsTabPage;
        private TabPage advanceSettingsTabPage;
        private GroupBox groupBox1;
        private RadioButton sameAsDBRadioButton;
        private RadioButton prefixRadioButton;
        private RadioButton camelCasedRadioButton;
        private TextBox prefixTextBox;
        private Label prefixLabel;
        private GroupBox groupBox2;
        private RadioButton vbRadioButton;
        private RadioButton cSharpRadioButton;
        private GroupBox groupBox3;
        private RadioButton fluentMappingOption;
        private RadioButton hbmMappingOption;
        private RadioButton byCodeMappingOption;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private RadioButton propertyRadioBtn;
        private RadioButton fieldRadioBtn;
        private RadioButton autoPropertyRadioBtn;
        private Label label6;
        private TextBox entityNameTextBox;
        private RadioButton castleMappingOption;
        private ComboBox ownersComboBox;
        private Label lblOwner;
        private RadioButton pascalCasedRadioButton;
        private GroupBox groupBox8;
        private CheckBox partialClassesCheckBox;
        private Panel pOracleOnlyOptions;
        private ProgressBar progressBar;
        private Button cancelButton;
        private CheckBox wcfDataContractCheckBox;
        private RadioButton nhFluentMappingStyle;
        private ListBox tablesListBox;
        private Label labelInheritence;
        private TextBox textBoxInheritence;
        private ComboBox comboBoxForeignCollection;
        private Label labelForeignEntity;
        private Label labelCLassNamePrefix;
        private TextBox textBoxClassNamePrefix;
        private GroupBox groupBox9;
        private CheckBox generateInFoldersCheckBox;
        private CheckBox useLazyLoadingCheckBox;
        private CheckBox includeForeignKeysCheckBox;
        private Label label7;
        private GroupBox groupBoxAttributeGeneration;
        private Label label8;
        private TextBox resouceReferenceTextBox;
        private CheckBox aatributeGenerationCheckBox;
        private TextBox displayNameAttributeTextBox;
        private Label label9;
        private Label label13;
        private Label label12;
        private Label label11;
        private Label label10;
        private TextBox requieredTextBox;
        private TextBox maxLengthTextBox;
        private TextBox tablePostTextBox;
        private TextBox tablePreTextBox;
        private TextBox errorResourceTextBox;
        private Label label14;
        private GroupBox groupBox10;
        private CheckBox ResourceGenerationCheckBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label label16;
        private Label label15;
        private TextBox RangeErrorTextBox;
        private TextBox RegxErrorTextBox;
        private DataGridViewTextBoxColumn columnName;
        private DataGridViewTextBoxColumn PersianName;
        private DataGridViewTextBoxColumn columnDataType;
        private DataGridViewComboBoxColumn cSharpType;
        private DataGridViewCheckBoxColumn isPrimaryKey;
        private DataGridViewCheckBoxColumn isForeignKey;
        private DataGridViewCheckBoxColumn isNullable;
        private DataGridViewCheckBoxColumn isUniqueKey;
        private DataGridViewCheckBoxColumn inList;
        private DataGridViewCheckBoxColumn inSort;
        private DataGridViewCheckBoxColumn inSearch;
        private DataGridViewCheckBoxColumn inLookUpLabel;
        private DataGridViewCheckBoxColumn inLookUpCombo;
        private DataGridViewTextBoxColumn regx;
        private DataGridViewTextBoxColumn BeginOfRange;
        private DataGridViewTextBoxColumn EndOfRange;
        private Button SaveMetadatButton;
        private GroupBox groupBox11;
        private CheckBox InheritBaseControllerCheckBox;
        private CheckBox GenearteControllerCheckBox;
        private CheckBox CreateCrudCheckBox;
        private CheckBox AuthorizeCheckBox;
        private Label label18;
        private TextBox ControllerNamespaceTextBox;
        private ContextMenuStrip contextMenuStrip1;
        private CheckBox GenerateLookupCheckBox;
        private GroupBox groupBox12;
        private CheckBox IncludePagingCheckBox;
        private CheckBox SearchModelGenerationCheckBox;
        private CheckBox UseAjaxCheckBox;
        private TextBox CommonResourseTextBox;
        private Label label19;
        private TextBox SearchModeltextBox;
        private Label Namespace;
        private TextBox RefrenceRichTextBox;
        private GroupBox groupBox13;
        private CheckBox VirtualcheckBox;
        private CheckBox AnnotationCheckBox;
        private ColorDialog colorDialog1;
        private ColorDialog colorDialog2;
        private ColorDialog colorDialog3;
        private GroupBox groupBox14;
        private CheckBox DALFiles;
        private TextBox DALNameSpace;
        private Label label20;
        private Label label17;
        private Label label21;
        private TextBox DALReferencesTextBox;
        private TextBox DataContextTextBox;
        private TextBox DBUtilitytextBox;
        private Label label23;
        private Label label22;
        private CheckBox SortExpressionCheckBox;
        private GroupBox groupBox15;
        private RadioButton NhibernateRadioButton;
        private RadioButton dbmlRadioButton;
        private CheckBox KendoCheckBox;
        private CheckBox GenerateViewCheckBox;
        private CheckBox DfaultCheckBox;
        private CheckBox bootstrapCheckBox;
    }
}

