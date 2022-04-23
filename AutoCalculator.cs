using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Parrilla_Fee_Helper
{
    public partial class AutoCalculator : Form
    {
        
        Functions functions = new Functions();
        string feeBackup;
        string totalBackup;
        string cardBackup;
        string moneyBackup;
        bool[] blockBox = new bool[2];
        Form1 Reference;
        List<TextBox> textBoxes = new List<TextBox>();

        public AutoCalculator(Form1 reference, TextBox FeeBox, TextBox AdditionBox, TextBox DiscountBox)
        {
            InitializeComponent();
            Reference = reference;
            textBoxes.Add(FeeBox);
            textBoxes.Add(AdditionBox);
            textBoxes.Add(DiscountBox);
            CheckLockers();
        }

        void ValidateAsNumber(TextBox text, string strBackup)
        {
            if(!String.IsNullOrEmpty( text.Text ))
            {
                int virgulas = 0;
                for(int i = 0; i<text.TextLength; i++){
                    switch(text.Text[i]){ //Check if TextBox has more than 1 ',' or '.'.
                        case '.':{
                            virgulas++;
                            break;
                        }
                        case ',':{
                            virgulas++;
                            break;
                        }
                        default:{
                            if(virgulas > 1)
                                break;
                            break;
                        }
                    }
                }
                if(!functions.IsNumber(text.Text) || virgulas > 1){
                    text.Text = strBackup;
                }
            }               
        }
        void Calculate(bool MainChanged, TextBox text)
        {
            Double fee = 0f;
            Double total = 0f;
            Double card = 0f;
            Double cash = 0f;
            Double received = 0f;
            Double difference = 0f;
            Double addition = 0f;
            Double discount = 0f;
            //bool IsOk;

            if(MainChanged){
                if(text == feeBox){
                    Double.TryParse(text.Text, out fee);
                  
                    if(!blockBox[1]){
                        total = fee * 10;
                        totalBox.Text = $"{total}";
                    }
                    else Double.TryParse(totalBox.Text, out total);
                }
                else if(text == totalBox){
                    Double.TryParse(text.Text, out total);
               
                    if(!blockBox[0]){
                        fee = total / 10;
                        feeBox.Text = $"{fee}";
                    }
                    else Double.TryParse(feeBox.Text, out fee);
                }
            }
            else{
                Double.TryParse(totalBox.Text, out total);
                Double.TryParse(feeBox.Text, out fee);
            }

            Double.TryParse(textBox1.Text, out card);
            Double.TryParse(textBox2.Text, out cash);

            received = card + cash;
            difference = received;
            difference -= total;
            if(difference > 0){
                addition = difference;
            }
            else if(difference < 0){
                discount = difference * -1;
            }
            textBox3.Text = $"{discount}";
            textBox4.Text = $"{addition}";
        }
        void CheckLockers()
        {
            switch(blockBox[0]){
                case true:{
                    pictureBox3.Image = Properties.Resources._lock;
                    break;
                }
                case false:{
                    pictureBox3.Image = Properties.Resources.unlock;
                    break;
                }
            }
            switch(blockBox[1]){
                case true:{
                    pictureBox4.Image = Properties.Resources._lock;
                    break;
                }
                case false:{
                    pictureBox4.Image = Properties.Resources.unlock;
                    break;
                }
            }
        }    
        private void feeBox_TextChanged(object sender, EventArgs e)
        {
            if(!feeBox.Focused) return;
            ValidateAsNumber(feeBox, feeBackup);
            if(feeBox.Text != feeBackup) Calculate(true, feeBox);
            feeBackup = feeBox.Text;
        }

        private void totalBox_TextChanged(object sender, EventArgs e)
        {
            if(!totalBox.Focused) return;
            ValidateAsNumber(totalBox, totalBackup);
            if(totalBox.Text != totalBackup) Calculate(true, totalBox);
            totalBackup = totalBox.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(!textBox1.Focused) return;
            ValidateAsNumber(textBox1, cardBackup);
            cardBackup = textBox1.Text;
            Calculate(false, null);
        }   

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(!textBox2.Focused) return;
            ValidateAsNumber(textBox2, moneyBackup);
            moneyBackup = textBox2.Text;
            Calculate(false, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxes[0].Text = feeBox.Text;
            textBoxes[1].Text = textBox4.Text;
            textBoxes[2].Text = textBox3.Text;
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            blockBox[1] = !blockBox[1];
            CheckLockers();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            blockBox[0] = !blockBox[0];
            CheckLockers();
        }
    }
}
