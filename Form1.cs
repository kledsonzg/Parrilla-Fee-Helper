using System.Diagnostics;
namespace Parrilla_Fee_Helper
{
    public partial class Form1 : Form
    {
        TextBox[] txtBox = new TextBox[1];
        Label[] titleText = new Label[1];
        Functions functions = new Functions();
        FeeManager manager = new FeeManager();
        FeeViewer feeviewer = new FeeViewer();

        string mesaText = "";
        string feeText = "";
        string acrescimoText = "";
        string descontoText = "";

        const int TXTBOX_QUANTY = 5;
        
        const int descBox = 0;
        const int mesaBox = 1;
        const int feeBox = 2;
        const int incBox = 3;
        const int decBox = 4;
        int oldFormWidth;
        int EditMode = -1;
        public bool IsDay = false;
        const int LABEL_QUANTY = 1;
        const int valueLabel = 0;

        public Form1()
        {
            InitializeComponent();

            InitialTest();
            Start();
        }

        void Start()
        {
            txtBox = new TextBox[TXTBOX_QUANTY];
            titleText = new Label[LABEL_QUANTY];

            txtBox[descBox] = textBox1;
            txtBox[mesaBox] = textBox2;
            txtBox[feeBox] = textBox3;
            txtBox[incBox] = textBox4;
            txtBox[decBox] = textBox5;
            titleText[valueLabel] = label8;
            oldFormWidth = this.Width;
            Debug.WriteLine(panel5.Parent.Name);

            List<Label> labels = new List<Label>();
            labels.Add(label14);
            labels.Add(label15);
            labels.Add(label16);
            labels.Add(label17);
            labels.Add(label18);
            labels.Add(label19);
            labels.Add(label22);
            manager.CreateNightManager();
            feeviewer.SetControls(manager, panel6, panel5, labels.ToArray(), textBox6, panel7);
            OnUserChangeDayNightTrack();
        }

        void InitialTest()
        {       

        }

        void OnUserSelectNewButton()
        {
            feeviewer.HideFee();
            panel3.Show();
            label4.Text = $"Registro de Comissão";
            for(int i=0; i<TXTBOX_QUANTY; i++){
                txtBox[i].Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e) //Botão "Novo".
        {
            OnUserSelectNewButton();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //txtbox Mesa.
        {
            ValidateTextBoxNumber(textBox2);
        }
        private void textBox3_TextChanged(object sender, EventArgs e) //txtbox Taxa de Serviço.
        {
            ValidateTextBoxNumber(textBox3);
        }
        private void textBox4_TextChanged(object sender, EventArgs e) //txtbox Acrescimo de Serviço.
        {
            ValidateTextBoxNumber(textBox4);
        }
        private void textBox5_TextChanged(object sender, EventArgs e) //txtbox Desconto de Serviço.
        {
            ValidateTextBoxNumber(textBox5);
        }
        void ValidateTextBoxNumber(TextBox obj) //Função para validar se tudo que tá sendo escrevido é um número.
        {
            string backupStr = "";
            
            foreach(TextBox textBox in txtBox){
                if(textBox == obj){
                    if(textBox == txtBox[mesaBox]){
                        backupStr = mesaText;
                    }
                    else if(textBox == txtBox[feeBox]){
                        backupStr = feeText;
                    }
                    else if(textBox == txtBox[incBox]){
                        backupStr = acrescimoText;
                    }
                    else if(textBox == txtBox[decBox]){
                        backupStr = descontoText;
                    }                  
                }
            }

            if(!String.IsNullOrEmpty( obj.Text ))
            {
                int virgulas = 0;
                for(int i = 0; i<obj.TextLength; i++){
                    switch(obj.Text[i]){ //Check if TextBox has more than 1 ',' or '.'.
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
                
                if(!functions.IsNumber(obj.Text) || virgulas > 1){
                    obj.Text = backupStr;
                    Debug.WriteLine("Erro");
                }
            } 

            obj.Text = obj.Text.Replace(".", ",");

            foreach(TextBox textBox in txtBox){
                if(obj == textBox){
                    if(textBox == txtBox[mesaBox]){
                        mesaText = textBox.Text;
                    }
                    else if(textBox == txtBox[feeBox]){
                        feeText = textBox.Text;
                    }
                    else if(textBox == txtBox[incBox]){
                        acrescimoText = textBox.Text;
                    }
                    else if(textBox == txtBox[decBox]){
                        descontoText = textBox.Text;
                    }
                }
            }
            Double[] values = new Double[3];
            for(int i = 0; i<values.Count(); i++){
                values[i] = 0f;
            }
            
            for(int i = feeBox; i < decBox + 1; i++){

                if(String.IsNullOrEmpty(txtBox[i].Text) || String.IsNullOrWhiteSpace(txtBox[i].Text) )
                    continue;

                values[i - 2] = Double.Parse(txtBox[i].Text);
            }


            Double finalValue = GetFeeSubTotal(values[0], values[1], values[2]);
            titleText[valueLabel].Text = $"R$ {finalValue}";
        }

        void OnUserSaveFee()
        {
            if( functions.IsEmpty( txtBox[mesaBox].Text) ) return;

            //bool[] isPossibleToUse = new bool[4];
            int mesa = -1;
            string description = txtBox[descBox].Text;
            Double[] values = new Double[3];

            if(functions.IsEmpty(description))
                description = "Sem Descrição";

            for(int i = 0; i<values.Count(); i++){
                values[i] = 0f;
            }

            Int32.TryParse(txtBox[mesaBox].Text, out mesa);
            Double.TryParse(txtBox[feeBox].Text, out values[0]);
            Double.TryParse(txtBox[incBox].Text, out values[1]);
            Double.TryParse(txtBox[decBox].Text, out values[2]);

            if(mesa == -1) return;

            if(EditMode == -1) manager.SaveFeeOnList(this, mesa, description, values[0], values[1], values[2]);
            else {
                manager.UpdateFee(this, EditMode, mesa, description, values[0], values[1], values[2]);
                EditMode = -1;
                label4.Text = $"Registro de Comissão";
            }                
        }
        Double GetFeeSubTotal(Double FeeValue, Double AcrescimoValue, Double DescontoValue)
        {
            Double subtotal = 0;
            if(DescontoValue > 0f)
                DescontoValue =  DescontoValue * -1;

            subtotal = FeeValue + AcrescimoValue + DescontoValue;
            return subtotal;
        }

        void OnMenuButtonClick()
        {              
            panel1.Visible = !panel1.Visible; 

            if(panel1.Visible){
                tableLayoutPanel1.ColumnCount = 2;
                tableLayoutPanel1.SetColumn(menu_Button, 1);
                menu_Button.Image = Properties.Resources.close_menu;
                tableLayoutPanel1.GetControlFromPosition(0, 0).BackColor = panel1.BackColor;
                
            }
            else{
                tableLayoutPanel1.SetColumn(menu_Button, 0);
                menu_Button.Image = Properties.Resources.open_menu;
                tableLayoutPanel1.GetControlFromPosition(0, 0).BackColor = panel4.BackColor;
                tableLayoutPanel1.ColumnCount = 1;
            }
        }
    
        void OnUserSelectAnIndex()
        {
            panel3.Hide();
            feeviewer.IsDay = this.IsDay;
            feeviewer.ShowFee(feeList.SelectedIndex);
        }

        void OnUserEnterOnEditMode()
        {           
            Debug.WriteLine("OnUserEnterOnEditMode");
            EditMode = feeviewer.EnterOnEditMode();
            label4.Text = $"Editando Registro: {EditMode.ToString()}";
            txtBox[descBox].Text = manager.Description[EditMode];
            txtBox[feeBox].Text = manager.Fee[EditMode].ToString();
            txtBox[mesaBox].Text = manager.Mesa[EditMode].ToString();
            txtBox[incBox].Text = manager.Acrescimo[EditMode].ToString();
            txtBox[decBox].Text = manager.Desconto[EditMode].ToString();
            panel3.Show();
        }
        public void FixIndexDetails(int index)
        {
            if(index == -1){
                feeList.SelectedIndex = -1;
                OnUserSelectNewButton();
                return;
            }
            feeList.SelectedIndex = -1;
            feeList.SelectedIndex = index;     
        }
        void OnUserDeleteFee()
        {
            feeviewer.DeleteFee(this);
        }
        void OnUserChangeDayNightTrack()
        {
            switch(trackDayNight.Value){
                case 0:{
                    pictureBox6.BorderStyle = BorderStyle.Fixed3D;
                    pictureBox7.BorderStyle = BorderStyle.None;
                    IsDay = true;
                    break;
                }
                case 1:{
                    pictureBox6.BorderStyle = BorderStyle.None;
                    pictureBox7.BorderStyle = BorderStyle.Fixed3D;
                    IsDay = false;
                    break;
                }
            }
            manager.RefreshList(this);
            OnUserSelectNewButton();
        }

        void OnUserClickOnAutoButton()
        {
            AutoCalculator calculator = new AutoCalculator(this, textBox3, textBox4, textBox5);
            calculator.ShowDialog();
        }
        void OnUserSelectOpenManagerButton()
        {
            ComissionManager comissionManager = new ComissionManager();
            comissionManager.Show();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            OnUserSaveFee();
        }

        private void menu_Button_Click(object sender, EventArgs e)
        {   
            OnMenuButtonClick();
        }

        private void feeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(feeList.SelectedIndex == -1) return;
            else OnUserSelectAnIndex();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            OnUserEnterOnEditMode();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            OnUserDeleteFee();
        }

        private void trackDayNight_ValueChanged(object sender, EventArgs e)
        {
            OnUserChangeDayNightTrack();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            OnUserClickOnAutoButton();
        }

        private void buttonAbrirGestor_Click(object sender, EventArgs e)
        {
            OnUserSelectOpenManagerButton();
        }
    }   
}