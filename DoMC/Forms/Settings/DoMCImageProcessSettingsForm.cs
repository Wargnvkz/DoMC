using DoMCLib.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Forms
{
    public partial class DoMCImageProcessSettingsForm : Form
    {

        public ImageProcessParameters ImageProcessParameters
        {
            get
            {
                var ipp = new ImageProcessParameters();

                ipp.TopBorder = (int)(nudTop.Value);
                ipp.BottomBorder = (int)(nudBottom.Value);
                ipp.LeftBorder = (int)(nudLeft.Value);
                ipp.RightBorder = (int)(nudRight.Value);
                /*DeviationWindow = (int)nudDeviationWindow.Value,
                MaxDeviation = (short)nudDeviationExcess.Value,
                MaxAverage = (short)nudAverageExcess.Value*/
                GetActions(ipp);
                return ipp;
            }
            set
            {
                nudTop.Value = value.TopBorder;
                nudBottom.Value = value.BottomBorder;
                nudLeft.Value = value.LeftBorder;
                nudRight.Value = value.RightBorder;
                /*nudDeviationWindow.Value = value.DeviationWindow;
                nudDeviationExcess.Value = value.MaxDeviation;
                nudAverageExcess.Value = value.MaxAverage;*/
                SetActions(value);
            }
        }

        private ComboBox[] ActionDefects;
        private NumericUpDown[] ActionDefectsParameters;
        private ComboBox[] ActionColors;
        private NumericUpDown[] ActionColorsParameters;

        public DoMCImageProcessSettingsForm()
        {
            InitializeComponent();
            ActionDefects = new ComboBox[] { cbActionDefect1, cbActionDefect2, cbActionDefect3, cbActionDefect4, cbActionDefect5 };
            ActionDefectsParameters = new NumericUpDown[] { nudActionDefectParameter1, nudActionDefectParameter2, nudActionDefectParameter3, nudActionDefectParameter4, nudActionDefectParameter5 };
            ActionColors = new ComboBox[] { cbActionColor1, cbActionColor2, cbActionColor3, cbActionColor4, cbActionColor5 };
            ActionColorsParameters = new NumericUpDown[] { nudActionColorParameter1, nudActionColorParameter2, nudActionColorParameter3, nudActionColorParameter4, nudActionColorParameter5 };
            InitDecisionComboBoxes();
        }

        private void InitDecisionComboBoxes()
        {
            for (int i = 0; i < ActionDefects.Length; i++)
            {
                ActionDefects[i].Items.Clear();
                ActionDefects[i].DataSource = Classes.MakeDecision.GetDecisionOperationTypeList();
                ActionDefects[i].DisplayMember = "Item2";
                ActionDefects[i].ValueMember = "Item1";

            }

            cbDefectResult.Items.Clear();
            cbDefectResult.DataSource = Classes.MakeDecision.GetMakeDecisionActionList();
            cbDefectResult.DisplayMember = "Item2";
            cbDefectResult.ValueMember = "Item1";

            for (int i = 0; i < ActionDefects.Length; i++)
            {
                ActionColors[i].Items.Clear();
                ActionColors[i].DataSource = Classes.MakeDecision.GetDecisionOperationTypeList();
                ActionColors[i].DisplayMember = "Item2";
                ActionColors[i].ValueMember = "Item1";

            }

            cbColorResult.Items.Clear();
            cbColorResult.DataSource = Classes.MakeDecision.GetMakeDecisionActionList();
            cbColorResult.DisplayMember = "Item2";
            cbColorResult.ValueMember = "Item1";
        }

        private void SetActions(ImageProcessParameters ipp)
        {
            for (int i = 0; i < Math.Min(5, ipp.Decisions[0]?.Operations?.Count ?? 0); i++)
            {
                ActionDefects[i].SelectedValue = ipp.Decisions[0]?.Operations[i].OperationType ?? DecisionOperationType.None;
                ActionDefectsParameters[i].Value = ipp.Decisions[0]?.Operations[i].Parameter ?? 0;
            }
            //var mda0 = ipp.Decisions[0]?.DecisionAction;
            //var _mda0 = (mda0 != null ? mda0 : MakeDecisionAction.Max);
            cbDefectResult.SelectedValue = ipp.Decisions[0]?.DecisionAction ?? MakeDecisionAction.Max;
            nudDefectParameterResult.Value = ipp.Decisions[0]?.ParameterCompareGoodIfLess ?? 0;
            for (int i = 0; i < Math.Min(5, ipp.Decisions[1]?.Operations?.Count ?? 0); i++)
            {
                ActionColors[i].SelectedValue = ipp.Decisions[1]?.Operations[i].OperationType ?? DecisionOperationType.None;
                ActionColorsParameters[i].Value = ipp.Decisions[1]?.Operations[i].Parameter ?? 0;
            }
            var mda1 = ipp.Decisions[1]?.DecisionAction;
            cbColorResult.SelectedValue = (mda1 != null ? mda1 : MakeDecisionAction.Average);
            nudColorParameterResult.Value = ipp.Decisions[1]?.ParameterCompareGoodIfLess ?? 0;
        }
        private void GetActions(ImageProcessParameters ipp)
        {
            ipp.Decisions[0] = new MakeDecision();
            ipp.Decisions[0].Result = DecisionActionResult.Defect;
            ipp.Decisions[0].Operations = new List<DecisionOperation>();
            ipp.Decisions[0].Operations.Clear();
            for (int i = 0; i < 5; i++)
            {
                if ((DecisionOperationType)ActionDefects[i].SelectedValue != DecisionOperationType.None)
                {
                    var operation = new DecisionOperation();
                    operation.OperationType = (DecisionOperationType)ActionDefects[i].SelectedValue;
                    operation.Parameter = (short)ActionDefectsParameters[i].Value;
                    ipp.Decisions[0].Operations.Add(operation);
                }
            }

            ipp.Decisions[0].DecisionAction = (MakeDecisionAction)cbDefectResult.SelectedValue;
            ipp.Decisions[0].ParameterCompareGoodIfLess = (short)nudDefectParameterResult.Value;

            ipp.Decisions[1] = new MakeDecision();
            ipp.Decisions[1].Result = DecisionActionResult.Color;
            ipp.Decisions[1].Operations = new List<DecisionOperation>();
            ipp.Decisions[1].Operations.Clear();
            for (int i = 0; i < 5; i++)
            {
                if ((DecisionOperationType)ActionDefects[i].SelectedValue != DecisionOperationType.None)
                {
                    var operation = new DecisionOperation();
                    operation.OperationType = (DecisionOperationType)ActionColors[i].SelectedValue;
                    operation.Parameter = (short)ActionColorsParameters[i].Value;
                    ipp.Decisions[1].Operations.Add(operation);
                }
            }
            ipp.Decisions[1].DecisionAction = (MakeDecisionAction)cbColorResult.SelectedValue;
            ipp.Decisions[1].ParameterCompareGoodIfLess = (short)nudColorParameterResult.Value;
        }
    }
}
