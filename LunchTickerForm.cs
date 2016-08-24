using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LunchTicker
{
    public partial class LunchTickerForm : Form
    {
        int hr, min, sec, ampm;   // Clock variables.
        int countStaff = 0;       // Number of staff members.
        int countVolunteer = 0;   // Number of volunteers.
        int val = 0;              // Returns the parsed data if valid.
        int currentHour = 0;      // Current hour on the clock.
        StreamReader fileReader;  // File input.
        StreamWriter fileWriter;  // File output.

        public LunchTickerForm()
        {
            InitializeComponent();
        }

        private void currentTime_Tick(object sender, EventArgs e)
        {
            hr = DateTime.Now.Hour;     // Current hour.
            ampm = hr;                  // AM/PM setter.
            min = DateTime.Now.Minute;  // Current minute.
            sec = DateTime.Now.Second;  // Current second.

            // If the second is divisible by two, adjust the clock accordingly.
            // Complex ternary operations deterine:
            // - If the hour is greater than 12 [subtracts 12 from current hour].
            // - If the minute is less than 10 [places a 0 in front of minute].
            // - If the second is less than 10 [places a 0 in front of second].
            // - If the time of day is greater than 12 [sets AM/PM status].
            if (sec % 2 == 0)
            {
                lblClock.Text = (hr > 12 ? hr -= 12 : hr) + ":" + (min < 10 ? "0" + Convert.ToString(min):Convert.ToString(min)) + ":" + 
                    (sec<10?"0" + Convert.ToString(sec):Convert.ToString(sec)) + (ampm < 12 ? " AM" : " PM");
            }
            else
            {
                lblClock.Text = (hr > 12 ? hr -= 12 : hr) + " " + (min < 10 ? "0" + Convert.ToString(min) : Convert.ToString(min)) + " " +
                    (sec < 10 ? "0" + Convert.ToString(sec) : Convert.ToString(sec)) + (ampm < 12 ? " AM" : " PM");
            }
        }

        private void LunchTickerForm_Load(object sender, EventArgs e)
        {
            lblCurrentDateShow.Text = DateTime.Today.ToShortDateString(); // Current date.

            // Check the format to make sure it is valid. If valid, update
            // the currentHour variable. If not, do nothing.
            if (Int32.TryParse(DateTime.Now.TimeOfDay.Hours.ToString(), out val))
            {
                currentHour = val;              
            }

            // This controls the staff button. If the hours are between 8 and 11,
            // the staff button will be enabled for use. Otherwise, it will remain
            // disabled.
            if (currentHour >= 8 && currentHour <= 11)
            {
                btnStaff.Enabled = true;
            }
            else
            {
                btnStaff.Enabled = false;
            }

            // This controls the volunteer button. If the hours are between 11 and 12,
            // the volunteer button will be enabled for use. Otherwise, it will remain
            // disabled.
            if (currentHour >= 11 && currentHour <= 12)
            {
                btnVolunteer.Enabled = true;
            }
            else
            {
                btnVolunteer.Enabled = false;
            }

            // This controls the reset button. If the hours are between 12 and 13,
            // the reset button will be enabled for use. Otherwise, it will remain
            // disabled.
            if (currentHour >= 12 && currentHour <= 13)
            {
                btnReset.Enabled = true;
            }
            else
            {
                btnReset.Enabled = false;
            }

            // Check for and parse the file. Throw an exception if the file is not found.
            try
            {
                // Read from file.
                fileReader = new StreamReader("lunch.dat");

                // Read in data for the staff count label.
                lblStaffCount.Text = fileReader.ReadLine();
                
                // Check the format to make sure it is valid. If valid, update
                // the countStaff variable. If not, do nothing.
                if (int.TryParse(lblStaffCount.Text, out val))
                {
                    countStaff = val; 
                }
                else
                {
                    countStaff = 0;
                    lblStaffCount.Text = Convert.ToString(countStaff);
                }

                // Read in data for the volunteer count label.
                lblVolunteerCount.Text = fileReader.ReadLine();

                // Check the format to make sure it is valid. If valid, update
                // the countVoluteers variable. If not, do nothing.
                if (int.TryParse(lblVolunteerCount.Text, out val))
                {
                    countVolunteer = val;                  
                }
                else
                {
                    countVolunteer = 0;
                    lblVolunteerCount.Text = Convert.ToString(countVolunteer);
                }

                // Close file.
                fileReader.Close();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found. Will create one for you.", "File Not Found");
            }

            // ToolTips
            {
                ToolTip toolTipStaff = new ToolTip();
                toolTipStaff.SetToolTip(btnStaff, "Staff button can only be clicked once.");
            }
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            lblStaffCount.Text = Convert.ToString(++countStaff);
            btnStaff.Enabled = false;
        }

        private void btnVolunteer_Click(object sender, EventArgs e)
        {
            // Keep track of staff button clicks.
            lblVolunteerCount.Text = Convert.ToString(++countVolunteer);
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            try
            {
                // Write to file.
                fileWriter = new StreamWriter("lunch.dat");

                // Write Staff count to file.
                fileWriter.WriteLine(Convert.ToString(countStaff));

                // Write Volunteer count to file.
                fileWriter.WriteLine(Convert.ToString(countVolunteer));
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found. Will create one for you.", "File Not Found");
            }

            fileWriter.Close(); // Close file.
            this.Close();   // Closes application. 
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Reset counters
            countStaff = countVolunteer = 0;

            // Reset Staff and Volunteer labels.
            lblStaffCount.Text = Convert.ToString(countStaff);
            lblVolunteerCount.Text = Convert.ToString(countVolunteer);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This program is designed to keep track of the\n" +
                "number of lunches to be served on the third floor\n" + 
                "of the Siloam Mission building.", "About Time Ticker"
              );

            MessageBox.Show(
                 "Application designed by Theo J.\n",
                 "Credits"                  
              );
        }
    }
}
