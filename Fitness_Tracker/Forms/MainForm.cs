using Fitness_Tracker.Activities;
using Fitness_Tracker.Models;
using Fitness_Tracker.Services;


namespace Fitness_Tracker.Forms;

///<summary>
///Main Windows : Set the calorie goal, record activities, see the calories  burned, the total and whether the goal has been achieved.
///</summary>

public class MainForm : Form
{
    private readonly User _user;
    private readonly AuthService _auth;
    private readonly WorkoutLog _log = new();


    // Goals 

    private readonly NumericUpDown _nudGoal = new();
    private readonly Label _lblCurrentGoal = new();

    // Activity 
    private readonly ComboBox _cmbActivity = new();
    private readonly Label[] _metricLabels = { new(), new(), new() };
    private readonly TextBox[] _metricBoxes = { new(), new(), new()};

    private readonly ListView _listLog = new();
    private readonly Label _lblTotal = new();
    private readonly ProgressBar _progress = new();
    private readonly Label _lblStatus = new();
    private readonly Label _lblFeedback = new();


    /// when the user press "Log out " the login window open again

    public bool LogoutRequested { get; private set; }

    public MainForm(User user, AuthService auth)
    {
        _user = user;
        _auth = auth;
        BuildInterface();

        _cmbActivity.SelectedIndex = 0;
        RefreshProgress();
    }

    

    private void BuildInterface()
    {
        Text = "Fitness Tracker"; 
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(800, 700);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;


            // header 

            var lblWelcome = new Label
            {
                Text = $"Welcome, {_user.Username}!",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var btnLogout = new Button { Text = "Log out", Location = new Point(680, 15), Size = new Size(100, 32) };
            btnLogout.Click += (s, e) =>

            {
                LogoutRequested = true;
                Close();
            };

            // goal front 

            var gbGoal = new GroupBox { Text = "1. Your goal", Location = new Point(20, 60), Size = new Size(760, 85) };

            var lblGoal = new Label { Text = "Calories to burn (kcal):", AutoSize = true, Location = new Point(15, 33) };
            _nudGoal.Location = new Point(175, 30);
            _nudGoal.Width = 100;
            _nudGoal.Minimum = User.MinGoal;
            _nudGoal.Maximum = User.MaxGoal;
            _nudGoal.Value = _user.CalorieGoal > 0 ? _user.CalorieGoal : 300;


            var btnSetGoal = new Button { Text = "Save goal", Location = new Point(290, 27), Size = new Size(110, 32) };
            btnSetGoal.Click += btnSetGoal_Click;


            _lblCurrentGoal.AutoSize = true;
            _lblCurrentGoal.Location = new Point(420, 33);

            gbGoal.Controls.AddRange(new Control[] { lblGoal, _nudGoal, btnSetGoal, _lblCurrentGoal });

            // Activity front 

            var gbActivity = new GroupBox { Text = "2. Record an Activity", Location = new Point(20, 155), Size = new Size(760, 215) };


            var lblActivity = new Label { Text = "Activity:", AutoSize = true, Location = new Point(15, 33) };
            _cmbActivity.Location = new Point(175, 30);
            _cmbActivity.Width = 220;
            _cmbActivity.DropDownStyle = ComboBoxStyle.DropDownList;

            foreach (Activity activity in ActivityCatalog.All)
            {
                _cmbActivity.Items.Add(activity);

            }

            _cmbActivity.SelectedIndexChanged += (s, e) => UpdateMetricFields();

            gbActivity.Controls.AddRange(new Control[] { lblActivity, _cmbActivity });

            for (int i = 0; i < 3; i++)
            {
                int y = 70 + i * 38;
                _metricLabels[i].AutoSize = false;
                _metricLabels[i].Location = new Point(15, y - 2);
                _metricLabels[i].Size = new Size(155, 30);
                _metricLabels[i].TextAlign = ContentAlignment.MiddleLeft;


                _metricBoxes[i].Location = new Point(175, y);
                _metricBoxes[i].Width = 220;

                gbActivity.Controls.Add(_metricLabels[i]);
                gbActivity.Controls.Add(_metricBoxes[i]);
            }

            var btnAdd = new Button
            {
                Text = "Calculate and Add",
                Location = new Point(175, 175),
                Size = new Size(220, 32)
            };
            btnAdd.Click += BtnAdd_Click;
            gbActivity.Controls.Add(btnAdd);

            var lblHelp = new Label
            {
                Text = "Fill in the threeBoxes. the accepted range in \r\n brackets next to each name. You may use a . or a, for decimals.",
                Location = new Point(420, 70),
                Size = new Size(330, 60)

            };
            gbActivity.Controls.Add(lblHelp);
            AcceptButton = btnAdd; // Enter key = ass the activity 

            // log front 

            var gbLog = new GroupBox { Text = "3.Activities recorded in this session", Location = new Point(20, 380), Size = new Size(760, 205) };

            _listLog.Location = new Point(15, 25);
            _listLog.Size = new Size(730, 130);
            _listLog.View = View.Details;
            _listLog.FullRowSelect = true;
            _listLog.GridLines = true;
            _listLog.HideSelection = false;
            _listLog.MultiSelect = false;
            _listLog.Columns.Add("Time", 70);
            _listLog.Columns.Add("Activity", 110);
            _listLog.Columns.Add("Metrics entered", 420);
            _listLog.Columns.Add("Calories(kcal)", 110, HorizontalAlignment.Right);


            var btnRemove = new Button { Text = "Remove selected", Location = new Point(15, 163), Size = new Size(140, 32) };
            btnRemove.Click += BtnRemove_Click;

            var btnClear = new Button { Text = "Clear all", Location = new Point(165, 163), Size = new Size(110, 32) };
            btnClear.Click += BtnClear_Click;


            gbLog.Controls.AddRange(new Control[] { _listLog, btnRemove, btnClear });

            // Progress front 

            var gbProgress = new GroupBox { Text = "4.Your progress", Location = new Point(20, 595), Size = new Size(760, 95) };

            _lblTotal.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _lblTotal.AutoSize = true;
            _lblTotal.Location = new Point(15, 25);

            _progress.Location = new Point(15, 52);
            _progress.Size = new Size(300, 22);

            _lblStatus.Location = new Point(330, 52);
            _lblStatus.Size = new Size(420, 38);
            _lblStatus.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);


            gbProgress.Controls.AddRange(new Control[] { _lblTotal, _progress, _lblStatus });


            // feedback line errors or confirmationq 
            _lblFeedback.Location = new Point(300, 18);
            _lblFeedback.Size = new Size(370, 36);
            _lblFeedback.TextAlign = ContentAlignment.MiddleLeft;

            Controls.AddRange(new Control[] { lblWelcome, btnLogout, _lblFeedback, gbGoal, gbActivity, gbLog, gbProgress });
}


            // behavours 

            // Changes the three input Labels when another activity is selcted. 
            private void UpdateMetricFields()
            {
                if(_cmbActivity.SelectedItem is not Activity activity)
                {
                    return; 
                }

                for (int  i = 0; i < 3; i++)
        

                {

                   _metricLabels[i].Text = activity.Metrics[i].Label;
                   _metricBoxes[i].Clear();
                 }

                 ShowFeedback("", isError: false);
                 _metricBoxes[0].Focus();
            }
    private void btnSetGoal_Click(object? sender, EventArgs e)
    {
        string? error = _auth.SaveGoal(_user, (int)_nudGoal.Value);
        if (error != null)
        {
            ShowFeedback(error, isError: true);
            return;
        }
        ShowFeedback($"Goal saved :{_user.CalorieGoal} kcal.", isError: false);
        RefreshProgress();

    }


    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (_cmbActivity.SelectedItem is not Activity activity)
        {
            ShowFeedback("Please choose an activity first.", isError: true);
            return;

        }

        // 1.Read the three numbers : say clearly which box is wrong

        var values = new double[3];
        for (int i = 0; i < 3; i++)
        {
            if (!InputParser.TryParse(_metricBoxes[i].Text, out values[i]))
            {
                ShowFeedback($"'{activity.Metrics[i].Name}' must be a number ", isError: true);
                _metricBoxes[i].Focus();
                _metricBoxes[i].SelectAll();
                return;

            }
        }

        // 2.Check the ranges 

        string? error = activity.ValidateMetrics(values);
        if (error != null)
        {
            ShowFeedback(error, isError: true);
            FocusInvalidBox(activity, values);
            return;
        }

        // Calculate, store and display.

        double calories = activity.CalculateCalories(values);
        _log.Add(new ActivityRecord(activity.Name, DescribeMetrics(activity, values), calories));
        foreach (TextBox box in _metricBoxes)
        {
            box.Clear();
        }

        _metricBoxes[0].Focus();

        ShowFeedback($"{activity.Name}: {calories:F1} kcal added.", isError: false);
        RefreshLog();
        RefreshProgress();


    }

    private void BtnRemove_Click(object? sender, EventArgs e)
    {
        if(_listLog.SelectedItems.Count == 0)
        {
            ShowFeedback("Select an activity in the list first.", isError: true);
            return;
        }

        _log.Remove((ActivityRecord)_listLog.SelectedItems[0].Tag!);
        ShowFeedback("Activity removed.", isError: false);
        RefreshLog();
        RefreshProgress();
    }

    private void BtnClear_Click(object? sender, EventArgs e)
    {
        if(_log.Records.Count == 0)
        {
            return;
        }


        DialogResult answer = MessageBox.Show("Remove all recorded activities from this session?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (answer == DialogResult.Yes)
        {
            _log.Clear();
            ShowFeedback("All activities removed.", isError: false);
            RefreshLog();
            RefreshProgress();
        }

    }







            


        // display 

        private void RefreshLog()

    {
        _listLog.Items.Clear();
        foreach (ActivityRecord record in _log.Records)
        {
            var item = new ListViewItem(record.Time.ToString("HH:mm:ss"));
            item.SubItems.Add(record.ActivityName);
            item.SubItems.Add(record.MetricsSummary);
            item.SubItems.Add(record.Calories.ToString("F1"));
            item.Tag = record;
            _listLog.Items.Add(item);
        }
    }

    // updates the goal labels , the total, the progress bar and the goal message.

    private void RefreshProgress()
    {
        _lblCurrentGoal.Text = _user.CalorieGoal > 0 ? $" Current saved goal : { _user.CalorieGoal} kcal" : "No goal saved yet.";

        GoalReport report = _log.Evaluate(_user.CalorieGoal);

        _lblTotal.Text = $"Total calories burned: {_log.TotalCalories:F1} kcal";
        _progress.Value = (int)Math.Min(100, Math.Max(0, report.Percentage));
        _lblStatus.Text = report.Message;
        _lblStatus.ForeColor = report.Achieved ? Color.SeaGreen
                               : report.HasGoal ? Color.DarkOrange
                               : Color.DimGray; 
    }

    private void ShowFeedback(string message, bool isError)
    {
        _lblFeedback.ForeColor = isError ? Color.Firebrick: Color.SeaGreen;
        _lblFeedback.Text = message; 
    }


    /// Puts the cursor in the first box whose value is not valid
    
    private void FocusInvalidBox(Activity activity, double[] values)
    {
        for(int i  = 0; i < 3; i++)
        {
            if (activity.Metrics[i].Validate(values[i]) != null)
            {
                _metricBoxes[i].Focus();
                _metricBoxes[i].SelectAll();
                return; 

            }
        }
    }


    private static string DescribeMetrics (Activity activity, double[] values)
    {
        var parts = new string[3];
        for(int i = 0; i < 3; i++)
        {
            MetricDefinition metric = activity.Metrics[i];
            parts[i] = $"{metric.Name}:{values[i]:0.##} {metric.Unit}";

        }

        return string.Join(", ", parts);

    }



    }



