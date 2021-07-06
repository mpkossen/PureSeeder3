using PureSeeder.Forms.Properties;
using System;
using System.Windows.Forms;

namespace PureSeeder.Forms
{
    public partial class ReleaseNotes : Form
    {
        public ReleaseNotes()
        {
            InitializeComponent();
        }

        private void ReleaseNotes_Load(object sender, EventArgs e)
        {
            Icon = Resources.PB;
            webBrowser1.Url = new Uri("http://pure-battlefield.github.io/PureSeeder3/");
        }
    }
}
