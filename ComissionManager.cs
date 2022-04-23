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
    public partial class ComissionManager : Form
    {
        List<TextBox> textGarcons = new List<TextBox>();
        List<TextBox> textValores = new List<TextBox>();
        List<TextBox> textTurnos = new List<TextBox>();
        List<PictureBox> addButtons = new List<PictureBox>();
        public ComissionManager()
        {
            InitializeComponent();
            InitializeLists();
        }
        void InitializeLists()
        {
            textGarcons.Add(C_textGarcom);
            textValores.Add(C_textValor);
            textTurnos.Add(C_textTurno);
            addButtons.Add(C_Add);
            var source = new AutoCompleteStringCollection();
            for(int i=0; i<C_listGarcons.Items.Count; i++){
                source.Add(C_listGarcons.Items[i].ToString());
            }

            for(int i = 0; i<addButtons.Count(); i++){
                textGarcons[i].AutoCompleteCustomSource = source;
                addButtons[0].Click += OnUserClickAddOrMinusComission;
            }
            textTurnos[0].AutoCompleteCustomSource = new AutoCompleteStringCollection();
            textTurnos[0].AutoCompleteCustomSource.Add("Dia");
            textTurnos[0].AutoCompleteCustomSource.Add("Noite");
        }

        private void OnUserClickAddOrMinusComission(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            PictureBox controle = (PictureBox)control;
            Debug.WriteLine($"name: {controle.Name}");
            Image ImageAdd = Properties.Resources.plus_icon_13085;
            Debug.WriteLine($"image: {ImageAdd.GetPropertyItem}");
            
            if(controle == addButtons.Last() || controle == addButtons[0]){
                //Clonando (Garçom TextBox)
                Debug.WriteLine($"botao de adicionar");
                TextBox textReference = textGarcons.Last();
                TextBox clone = new TextBox();
                CopyTextBoxProperties(clone, textReference);
                textGarcons.Add(clone);

                //Clonando (Valor TextBox)
                clone = new TextBox();
                textReference = textValores.Last();
                CopyTextBoxProperties(clone, textReference);
                textValores.Add(clone);
                clone.Text = textReference.Text;

                //Clonando (Turno TextBox)
                clone = new TextBox();
                textReference = textTurnos.Last();
                CopyTextBoxProperties(clone, textReference);
                textTurnos.Add(clone);
                clone.Text = textReference.Text;

                //Clonando (Adicionar PictureBox)
                PictureBox pClone = new PictureBox();
                CopyPictureBoxProperties(pClone, controle);
                pClone.Click += OnUserClickAddOrMinusComission;
                addButtons.Add(pClone);
                
                if(controle != addButtons[0]){
                    controle.Image = Properties.Resources.Flat_minus_icon___red_svg;
                }
                else controle.Enabled = false;
            }
            else{
                for(int i=0; i<addButtons.Count; i++){
                    if(controle == addButtons[i]){
                        addButtons[i].Dispose();
                        textGarcons[i].Dispose();
                        textValores[i].Dispose();
                        textTurnos[i].Dispose();
                        if(i != addButtons.Count -1){
                            for(int d= i+1; d<addButtons.Count; d++){
                                FixControlPosition(addButtons[d]);
                                FixControlPosition(textGarcons[d]);
                                FixControlPosition(textValores[d]);
                                FixControlPosition(textTurnos[d]);
                            }
                        }
                        addButtons.RemoveAt(i);
                        textGarcons.RemoveAt(i);
                        textValores.RemoveAt(i);
                        textTurnos.RemoveAt(i);
                        break;   
                    }
                }
            }    
        }

        void FixControlPosition(Control control)
        {
            int moving = 35;
            control.Location = new Point(control.Location.X, control.Location.Y - moving);
        }

        void CopyTextBoxProperties(TextBox paste, TextBox clone)
        {
            int moving = 35;

            paste.Size = clone.Size;
            paste.Location = new Point(clone.Location.X, clone.Location.Y + moving);
            paste.Anchor = clone.Anchor;
            paste.BackColor = clone.BackColor;
            paste.BorderStyle = clone.BorderStyle;
            paste.ForeColor = clone.ForeColor;
            paste.TextAlign = clone.TextAlign;
            paste.AutoCompleteMode = clone.AutoCompleteMode;
            paste.AutoCompleteSource = clone.AutoCompleteSource;
            paste.AutoCompleteCustomSource = clone.AutoCompleteCustomSource;
            paste.PlaceholderText = clone.PlaceholderText;
            this.Controls.Add(paste);
            paste.Parent = clone.Parent;
        }

        void CopyPictureBoxProperties(PictureBox paste, PictureBox clone)
        {
            int moving = 35;

            paste.Size = clone.Size;
            paste.Location = new Point(clone.Location.X, clone.Location.Y + moving);
            paste.Anchor = clone.Anchor;
            paste.BackColor = clone.BackColor;
            paste.BorderStyle = clone.BorderStyle;
            paste.ForeColor = clone.ForeColor;
            paste.Image = clone.Image;
            paste.SizeMode = clone.SizeMode;
            paste.Cursor = clone.Cursor;
            this.Controls.Add(paste);
            paste.Parent = clone.Parent;
        }

        private void C_buttonGarcons_Click(object sender, EventArgs e)
        {
            C_listGarcons.Visible = !C_listGarcons.Visible;
            bool Visible = C_listGarcons.Visible;
            if(Visible){
                C_panel2.Location = new Point(267, C_panel2.Location.Y);
            }
            else{
                C_panel2.Location = new Point(0, C_panel2.Location.Y);
            }
        }

        private void C_buttonExportar_Click(object sender, EventArgs e)
        {

        }
    }
}
