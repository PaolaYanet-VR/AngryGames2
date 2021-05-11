using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppGas.Triggers
{
    class PriceTrigger : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            int n;
            bool isNumeric = int.TryParse(sender.Text, out n);
            if (string.IsNullOrWhiteSpace(sender.Text) || !isNumeric)
            { // No es numerico
                sender.Text = ""; //text anterior
            }
            else
            { // Es numerico
                if (n < 0)
                {
                    sender.Text = "0";
                }
                else if (n > 1000)
                {
                    sender.Text = "1000";
                }
            }
        }
    }
}
