using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Services
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            //float result;
            //bool isValid = float.TryParse(args.NewTextValue, out result);
            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                ((Entry)sender).Text = null;
            }
        }
    }
}
