using GridCustomCell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCustomCell.ViewModels
{
    public class MainWindowViewModel
    {
        public List<InputReferenceItem> Items = new List<InputReferenceItem>();

        public MainWindowViewModel()
        {
            InitData();
        }

        private void InitData()
        {
            for (int i = 0; i < 100; i++)
            {
                Items.Add(new InputReferenceItem()
                {
                    ReferenceColumnName = $"Name_{i}",
                    ReferenceValue = new Random(100).Next().ToString()
                });
            }
        }
    }
}
