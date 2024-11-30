using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaseKeeper.Forms
{
    public partial class MainForm : Form
    {
        private Panel navPanel;

        // Theme Colors
        private readonly Color PrimaryColor = ColorTranslator.FromHtml("#007BFF");
        private readonly Color SecondaryColor = ColorTranslator.FromHtml("#F8F9FA");
        private readonly Color TextColor = ColorTranslator.FromHtml("#343A40");

        public MainForm()
        {
            InitializeComponents();
            SetupMainForm();
        }

        private void InitializeComponents()
        {
            this.Text = "CaseKeeper - Legal Case Management";
            this.Size = new Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize navigation panel
            navPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = PrimaryColor
            };

            this.Controls.Add(navPanel);
        }

        private void SetupMainForm()
        {
            // Add navigation buttons
            AddNavigationButton("New Case", 0);
            AddNavigationButton("View Cases", 1);
            AddNavigationButton("Reminders", 2);
            AddNavigationButton("Generate Report", 3);
            AddNavigationButton("Settings", 4);
        }

        private void AddNavigationButton(string text, int position)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(180, 40),
                Location = new Point(10, 20 + (position * 50)),
                BackColor = SecondaryColor,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };

            btn.Click += NavigationButton_Click;
            navPanel.Controls.Add(btn);
        }

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text)
            {
                case "New Case":
                    MessageBox.Show("New Case form will be implemented soon.");
                    break;
                case "View Cases":
                    MessageBox.Show("View Cases form will be implemented soon.");
                    break;
                case "Reminders":
                    MessageBox.Show("Reminders form will be implemented soon.");
                    break;
                case "Generate Report":
                    MessageBox.Show("Report generation will be implemented soon.");
                    break;
                case "Settings":
                    MessageBox.Show("Settings form will be implemented soon.");
                    break;
            }
        }
    }
}
