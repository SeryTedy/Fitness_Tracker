using Fitness_Tracker.Models;
using Fitness_Tracker.Services;


namespace Fitness_Tracker.Forms;

/// <summary>
/// Login Windows : Allow the user also to open the registration windows 
/// </summary>

public class LoginForm : Form
{
    private readonly AuthService _auth; 

    private readonly TextBox _txtUsername = new();
    private readonly TextBox _txtPassword = new();

    private readonly CheckBox _chkShowPassword = new();

    private readonly Button _btnLogin = new();

    private readonly Button _btnRegister = new();

    private readonly Label _lblMessage = new();

    /// <summary>
    ///
    /// </summary>
    
    public User? LoggedInUser { get; private set;  }

    public LoginForm(AuthService auth)
    {
        _auth = auth;
        BuildInterface();
    }

    private void BuildInterface()
    {
        Text = "Fitness Tracker - Log in"; 
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(420, 330);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        var lblTitle = new Label
        {
            Text = "Fitness Tracker",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 15)

        };

        var lblSubtitle = new Label
        {
            Text = "set your goals and record your activities. ",
            AutoSize = true,
            Location = new Point(22, 52)
        };

        var lblUsername = new Label { Text = "Username:", AutoSize = true, Location = new Point(20, 85) };
        _txtUsername.Location = new Point(20, 107);
        _txtUsername.Width = 380;

        var lblPassword = new Label { Text = "Password:", AutoSize = true, Location = new Point(20, 140) };
        _txtPassword.Location = new Point(20, 162);
        _txtPassword.Width = 380;
        _txtPassword.UseSystemPasswordChar = true;

        _chkShowPassword.Text = "Show password";
        _chkShowPassword.AutoSize = true;
        _chkShowPassword.Location = new Point(20, 193);
        _chkShowPassword.CheckedChanged += (s, e) => 
             _txtPassword.UseSystemPasswordChar = !_chkShowPassword.Checked;

        _btnLogin.Text = "Log in";
        _btnLogin.Location = new Point(20, 225);
        _btnLogin.Size = new Size(185, 36);
        _btnLogin.Click += BtnLogin_Click;


        _btnRegister.Text = "Create an account";
        _btnRegister.Location = new Point(215, 225);
        _btnRegister.Size = new Size(185, 36);
        _btnRegister.Click += BtnRegister_Click;


        _lblMessage.Location = new Point(20, 272);
        _lblMessage.Size = new Size(380, 45);
        _lblMessage.ForeColor = Color.Firebrick;

        AcceptButton = _btnLogin; // Enter key = log in 

        Controls.AddRange(new Control[]
        {
            lblTitle, lblSubtitle, lblUsername, _txtUsername, lblPassword,
            _txtPassword, _chkShowPassword, _btnLogin, _btnRegister, _lblMessage

        });



    }

    private void  BtnLogin_Click(object? sender, EventArgs e)
    {
        LoginResult result = _auth.Login(_txtUsername.Text.Trim(), _txtPassword.Text);

            if (result.Status == LoginStatus.Success)
        {
            LoggedInUser = result.User;
            DialogResult = DialogResult.OK;
            Close();
            return;
        }


        _lblMessage.ForeColor = Color.Firebrick;
        _lblMessage.Text = result.Message;
        _txtPassword.Clear();
        _txtPassword.Focus();
        
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        using var registerForm = new RegisterForm(_auth);
        if (registerForm.ShowDialog(this) == DialogResult.OK)
        {
            _txtUsername.Text = registerForm.CreatedUsername;
            _txtPassword.Clear();
            _lblMessage.ForeColor = Color.SeaGreen;
            _lblMessage.Text = "Account created. You can log in.";
            _txtPassword.Focus();
        }
    }




}