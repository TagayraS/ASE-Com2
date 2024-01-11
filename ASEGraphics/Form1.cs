using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ASEGraphics
{
    /// <summary>
    /// Main form for the GraphicalProgrammingEnvironmentASE application.
    /// </summary>
    public partial class Form1 : Form
    {

        Bitmap bitmap1 = new Bitmap(411, 414);
        Bitmap bitmap2 = new Bitmap(411, 414);
        Pen pen = new Pen(Color.Yellow, 2);
        Boolean GiveBoolForFillColor = false;
        Color Backgroudcolor = Color.Black;
        Graphics g;
        Point penposition;
        private string feedbackMessage;


        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            penposition = new Point(10, 10);
            InitializeComponent();
            g = Graphics.FromImage(bitmap1);
            g.Clear(Color.Black);

        }

        /// <summary>
        /// Event handler for painting the PictureBox.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(bitmap1, 0, 0);
            g.DrawImageUnscaled(bitmap2, 0, 0);
            e.Graphics.DrawEllipse(pen, penposition.X, penposition.Y, 10, 10);

        }
        /// <summary>
        /// Event handler for the button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

        public void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) & string.IsNullOrEmpty(textBox2.Text))
            {

                string[] converttostring = textBox1.Lines;
                foreach (string commandline in converttostring)
                {
                    string storeSinglelineCode = Convert.ToString(commandline);
                    storeSinglelineCode = storeSinglelineCode.ToLower();
                    string[] addCommandToList = storeSinglelineCode.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    CommandsInCommandLine(addCommandToList);          //function that runs all command 
                }
            }

            else if (!string.IsNullOrEmpty(textBox2.Text) & string.IsNullOrEmpty(textBox1.Text))
            {
                string converttostring = textBox2.Text.ToString();
                converttostring = converttostring.ToLower();
                string[] getcommand = converttostring.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                CommandsInCommandLine(getcommand);
            }

        }

        /// <summary>
        /// Processes the commands provided in the command line.
        /// </summary>
        /// <param name="listForCommands">The list of commands to be processed.</param>
        public void CommandsInCommandLine(string[] listForCommands)
        {
            Command command = new Command(g, pen, penposition.X, penposition.Y);

            if (listForCommands == null || listForCommands.Length == 0)
            {
                SetFeedbackMessage("No command entered.");
                return;
            }

            switch (listForCommands[0].ToLower())
            {
                case "rectangle":
                    if (listForCommands.Length == 3)
                    {
                        if (int.TryParse(listForCommands[1], out int valueForWidth) &&
                            int.TryParse(listForCommands[2], out int valueForHeight))
                        {
                            if (valueForWidth > 0 && valueForHeight > 0)
                            {
                                command.DrawRectangle(command, GiveBoolForFillColor, valueForHeight, valueForWidth);
                                pictureBox1.Refresh();
                            }
                            else
                            {
                                SetFeedbackMessage("Width and height must be greater than zero for rectangle command.");
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Invalid arguments for rectangle command.");
                        }
                    }
                    else
                    {
                        SetFeedbackMessage("Incorrect amount of arguments for rectangle command");
                    }
                    break;


                case "triangle":
                    if (listForCommands.Length == 2 || listForCommands.Length == 4)
                    {
                        if (int.TryParse(listForCommands[1], out int valueforsidelength))
                        {
                            if (valueforsidelength > 0)
                            {
                                command.DrawTriangle(command, GiveBoolForFillColor, valueforsidelength);
                                pictureBox1.Refresh();
                            }
                            else
                            {
                                SetFeedbackMessage("Negative or zero parameter for triangle command.");
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Invalid argument for triangle command.");
                        }
                    }
                    else
                    {
                        SetFeedbackMessage("Incorect amount of arguments for triangle command.");
                    }
                    break;

                case "circle":
                    if (listForCommands.Length == 2)
                    {
                        if (int.TryParse(listForCommands[1], out int valueforradius))
                        {
                            if (valueforradius > 0)
                            {
                                command.DrawCircle(command, GiveBoolForFillColor, valueforradius);
                                pictureBox1.Refresh();
                            }
                            else
                            {
                                SetFeedbackMessage("Negative or zero parameter for circle command.");
                            }
                        }
                        else
                        {
                            SetFeedbackMessage("Invalid argument for circle command.");
                        }
                    }
                    else
                    {
                        SetFeedbackMessage("Incorect amount of arguments for circle command.");
                    }
                    break;

                case "moveto":
                    if (listForCommands.Length == 3)
                    {
                        if (int.TryParse(listForCommands[1], out int valueforxaxis) &&
                            int.TryParse(listForCommands[2], out int valueforyaxis))
                        {
                            penposition = new Point(valueforxaxis, valueforyaxis);
                            pictureBox1.Refresh();
                        }
                        else
                        {
                            SetFeedbackMessage("Invalid arguments for moveto command.");
                        }
                    }
                    else
                    {
                        SetFeedbackMessage("Incorrect amount of arguments for moveto command.");
                    }
                    break;

                case "drawto":
                    if (listForCommands.Length == 3)
                    {
                        if (int.TryParse(listForCommands[1], out int valueforxaxis) &&
                            int.TryParse(listForCommands[2], out int valueforyaxis))
                        {
                            command.DrawTo(command, valueforxaxis, valueforyaxis);
                            penposition = new Point(valueforxaxis, valueforyaxis);
                            pictureBox1.Refresh();
                        }
                        else
                        {
                            SetFeedbackMessage("Invalid arguments for drawto command.");
                        }
                    }
                    else
                    {
                        SetFeedbackMessage("Incorect amount of arguments for drawto command.");
                    }
                    break;

                case "pen":
                    if (listForCommands.Length == 2)
                    {
                        if (listForCommands[1].Equals("yellow") || listForCommands[1].Equals("white") ||
                            listForCommands[1].Equals("red") || listForCommands[1].Equals("green") ||
                            listForCommands[1].Equals("blue") || listForCommands[1].Equals("pink") ||
                            listForCommands[1].Equals("purple") || listForCommands[1].Equals("orange"))
                        {
                            command.PenColor(listForCommands[1], pen);

                        }
                        else
                        {
                            MessageBox.Show("Try entering one of these colors: green, blue, pink, white, yellow, orange, red, purple");
                        }
                    }
                    else
                    {
                        SetFeedbackMessage("Incorect amount of arguments for pen command.");
                    }
                    break;

                case "fill":
                    if (listForCommands.Length == 2)
                    {
                        try
                        {
                            if (command.Fill(listForCommands[1]))
                            {
                                GiveBoolForFillColor = true;
                            }
                            else
                            {
                                GiveBoolForFillColor = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            SetFeedbackMessage("An error occurred: " + ex.Message);
                        }
                    }
                    else
                    {
                        SetFeedbackMessage("Incorrect amount of arguments for fill command.");
                    }
                    break;

                case "reset":
                    // Reset the pen position
                    if (listForCommands.Length == 1)
                    {
                        penposition = new Point(10, 10);
                        g.Clear(Color.Black);
                        pictureBox1.Refresh();
                    }
                    else
                    {
                        SetFeedbackMessage("Reset does not take arguements.");
                    }

                    break;

                case "clear":
                    // Clear the canvas
                    if (listForCommands.Length == 1)
                    {
                        g.Clear(Color.Black);
                        pictureBox1.Refresh();
                    }
                    else
                    {
                        SetFeedbackMessage("Clear does not take arguements.");
                    }

                    break;

                default:
                    SetFeedbackMessage($"Invalid command: {listForCommands[0]}");
                    break;
            }
        }


        private void SetFeedbackMessage(string message)
        {
            feedbackMessage = message;
            // Display the feedback message, for example, in a label or MessageBox
            MessageBox.Show(message);
        }


        public Point PenPosition
        {
            get { return penposition; }
        }

        public Color BackgroundColor
        {
            get { return Backgroudcolor; }
        }

        /*
        public TextBox TextBox1
        {
            get { return textBox1; }
        }
        */

        public string FeedbackMessage
        {
            get { return feedbackMessage; }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        // <summary>
        /// Event handler for the button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "\"Text Files (.txt)|*.txt|All Files|*.*";
            openFileDialog.DefaultExt = "txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        g.Clear(Color.Black);
                        while (!reader.EndOfStream)
                        {

                            string command = reader.ReadLine();
                            string[] singlecommand = command.Split(' ');
                            CommandsInCommandLine(singlecommand);
                        }
                    }
                    MessageBox.Show("Successfully Loaded File");
                }
                catch
                {
                    MessageBox.Show("ERROR LOADING THE FILE");

                }
            }



        }

        /// <summary>
        /// Event handler for the button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void button4_Click(object sender, EventArgs e)
        {


            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                string[] storeLineInFile = textBox1.Lines;

                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (string commandline in storeLineInFile)
                        {
                            writer.WriteLine(commandline);
                        }
                    }

                    MessageBox.Show("FILE ADDED");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR IN SAVING THE FILE");
                }

            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}