using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parrilla_Fee_Helper
{
    internal class FeeManager
    {
        public List<int> Mesa = new List<int>();
        public List<string> Description = new List<string>();
        public List<Double> Fee = new List<double>();
        public List<Double> Acrescimo = new List<double>();
        public List<Double> Desconto = new List<double>();
        bool IsDay = true;
        public FeeManager FeeNight;

        public void CreateNightManager()
        {
            FeeNight = new FeeManager();
            FeeNight.IsDay = false;
        }
        
        public void SaveFeeOnList(Form1 reference, int mesa, string description, Double fee, Double acrescimo, Double desconto)
        {
            if(!reference.IsDay && IsDay){
                FeeNight.SaveFeeOnList(reference, mesa, description, fee, acrescimo, desconto);
                return;
            }    
            ListBox lista = reference.feeList;

            lista.Items.Add($"{description} - Mesa: {mesa}");
            Mesa.Add(mesa);
            Description.Add(description);
            Fee.Add(fee);
            Acrescimo.Add(acrescimo);
            Desconto.Add(desconto);
        }
        public void UpdateFee(Form1 reference, int index, int mesa, string description, Double fee, Double acrescimo, Double desconto)
        {
            if(!reference.IsDay && IsDay){
                FeeNight.UpdateFee(reference, index, mesa, description, fee, acrescimo, desconto);
                return;
            }
            ListBox lista = reference.feeList;

            lista.Items[index] = $"{description} - Mesa: {mesa}";
            Mesa[index] = mesa;
            Description[index] = description;
            Fee[index] = fee;
            Acrescimo[index] = acrescimo;
            Desconto[index] = desconto;
            reference.FixIndexDetails(index);
        }
        public void DeleteFee(Form1 reference, int index)
        {
            if(!reference.IsDay && IsDay){
                FeeNight.DeleteFee(reference, index);
                return;
            }
            ListBox lista = reference.feeList;

            lista.Items.RemoveAt(index);
            Mesa.RemoveAt(index);
            Description.RemoveAt(index);
            Fee.RemoveAt(index);
            Acrescimo.RemoveAt(index);
            Desconto.RemoveAt(index);
            reference.FixIndexDetails(-1);
        }
        public void RefreshList(Form1 reference)
        {
            ListBox lista = reference.feeList;
            List<string> refresh;
            List<int> table;

            lista.Items.Clear();
            if(reference.IsDay){
                refresh = this.Description;
                table = this.Mesa;
            }
            else{
                refresh = FeeNight.Description;
                table = FeeNight.Mesa;
            } 

            for(int i=0; i<refresh.Count(); i++){
                lista.Items.Add($"{refresh[i]} - Mesa: {table[i]}");
            }
        }
    }
}
