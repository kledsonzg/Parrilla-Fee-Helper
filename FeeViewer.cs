using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parrilla_Fee_Helper
{
    internal class FeeViewer
    {
        Panel[] register = new Panel[2];
        Panel SidePanel;
        Label Mesa;
        Label Valor;
        Label Fee;
        Label Acrescimo;
        Label Desconto;
        Label Total;
        Label TotalFee;
        TextBox Description;
        FeeManager Manager;
        int Index;
        public bool IsDay = true;

        public void SetControls(FeeManager manager, Panel Panel1, Panel Panel2, Label[] Texts, TextBox textBox, Panel sidePanel)
        {
            register[0] = Panel1;
            register[1] = Panel2;
            SidePanel = sidePanel;
            Description = textBox;

            for(int i=0; i<Texts.Count(); i++){
                switch(i){
                    case 0:
                        Mesa = Texts[i];
                        break;
                    case 1:
                        Valor = Texts[i];
                        break;
                    case 2:
                        Fee = Texts[i];
                        break;
                    case 3:
                        Total = Texts[i];
                        break;
                    case 4:
                        Desconto = Texts[i];
                        break;
                    case 5:
                        Acrescimo = Texts[i];
                        break;
                    case 6:
                        TotalFee = Texts[i];
                        break;
                }
            }
            Manager = manager;
        }
        public void ShowFee(int index)
        {
            FeeManager manager;
            switch(IsDay){
                case false:{
                    manager = Manager.FeeNight;
                    break;
                }
                case true:{
                    manager = Manager;
                    break;
                }
            }
            
            Double subtotal = manager.Fee[index] * 10;
            Double total = subtotal + manager.Fee[index];
            Double fee = manager.Fee[index] + manager.Acrescimo[index] - manager.Desconto[index];

            Mesa.Text = $"Mesa: {manager.Mesa[index]}";
            Valor.Text = $"Valor: R${subtotal}";
            Fee.Text = $"Taxa de Serviço: R${manager.Fee[index]}";
            Acrescimo.Text = $"Acréscimo: R${manager.Acrescimo[index]}";
            Desconto.Text = $"Desconto: R${manager.Desconto[index]}";
            Total.Text = $"Valor Total da Mesa: R${total}";
            TotalFee.Text = $"R$ {fee}";
            Description.Text = $"{manager.Description[index]}";
            Index = index;
            ViewFee();
        }
        public void DeleteFee(Form1 reference)
        {
            Manager.DeleteFee(reference, Index);
        }
        public void HideFee()
        {
            register[0].Hide();
            register[1].Hide();
            SidePanel.Hide();
        }
        void ViewFee()
        {
            register[0].Show();
            register[1].Show();
            SidePanel.Show();
        }
        int GetIndex()
        {
            return Index;
        }
        public int EnterOnEditMode()
        {
            int index = GetIndex();
            Index = -1;
            HideFee();
            return index;
        }
    }
}
