using Fitness_Tracker.Services;


namespace Fitness_Tracker.Forms;

public class RegisterForm : Form
{
    private readonly AuthService _auth;

    private readonly TextBox _txtUsername = new();
    private readonly TextBox _txtPassword = new();

    private readonly TextBox _txtConfirm = new();

    private readonly Label _lblMessage= new();

    public string CreatedUsername { get; private set; } = "";

    public RegisterForm(AuthService auth)
    {
        _auth = auth;
        BuildInterface();
    }

    private void BuildInterface()
    {
        Text = "Fitness Tracker _ Create an account";
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(420, 420);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        var lblRules = new Label
        {
            Text = "Rules:\r\n" +
                   "• Username: letters and numbers only allowed. \r\n" +
                   $"• Password: exactly {CredentialValidator.PasswordLength} characters, with at least\r\n" +
                   " one lowercase and uppercase letter.",
            Location = new Point(20, 15),
            Size = new Size(380, 80)

        };

        var lblUsername = new Label { Text = "Username:", AutoSize = true, Location = new Point(20, 105) };
        _txtUsername.Location = new Point(20, 107);
        _txtUsername.Width = 380;

        var lblPassword = new Label { Text = "Password:", AutoSize = true, Location = new Point(20, 162) };
        _txtPassword.Location = new Point(20, 184);
        _txtPassword.Width = 380;
        _txtPassword.UseSystemPasswordChar = true;

        var lblConfirm = new Label { Text = "Confirm password:", AutoSize = true, Location = new Point(20, 219) };
        _txtConfirm.Location = new Point(20, 241);
        _txtConfirm.Width = 380;
        _txtConfirm.UseSystemPasswordChar = true;

        var btnCreate = new Button
        {
            Text = "Create Account",
            Location = new Point(20, 285),
            Size = new Size(185, 36)

        };
        btnCreate.Click += BtnCreate_Click;

        var btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(215, 285),
            Size = new Size(185, 36),
            DialogResult = DialogResult.Cancel
        };

        _lblMessage.Location = new Point(20, 335);
        _lblMessage.Size = new Size(380, 70);
        _lblMessage.ForeColor = Color.Firebrick;

        AcceptButton = btnCreate;
        CancelButton = btnCancel;

        Controls.AddRange(new Control[]
        {
            lblRules, lblUsername, lblPassword, _txtPassword, _txtUsername, lblUsername,
            lblConfirm, _txtConfirm, btnCreate, btnCancel, _lblMessage

        });
    }

        private void BtnCreate_Click(object? sender, EventArgs e)
        {
        string username = _txtUsername.Text.Trim();
        string? error = _auth.Register(username, _txtPassword.Text, _txtConfirm.Text);

        if (error != null)
        {
            _lblMessage.Text = error;
            return;
        }

        CreatedUsername = username;
        DialogResult = DialogResult.OK;
        Close();




    }
}
