using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListBoxDemo.Models;

namespace ListBoxDemo.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<DatabaseModel> DatabaseCollection { get; set; }
        public ObservableCollection<ModelTypeModel> ModelTypeCollection { get; set; }

        public MainWindowViewModel()
        {
            DatabaseCollection = new ObservableCollection<DatabaseModel>();
            ModelTypeCollection = new ObservableCollection<ModelTypeModel>();
            InitDatabaseInfo();
            InitModelTypeInfo();
        }

        private void InitDatabaseInfo()
        {
            DatabaseCollection.Clear();
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Oracel", DatabaseIcon = "/Img/oracle.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Oracel", DatabaseIcon = "/Img/oracle.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Oracel", DatabaseIcon = "/Img/oracle.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Oracel", DatabaseIcon = "/Img/oracle.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Oracel", DatabaseIcon = "/Img/oracle.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "SqlServer", DatabaseIcon = "/Img/sqlServer.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "SqlServer", DatabaseIcon = "/Img/sqlServer.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "SqlServer", DatabaseIcon = "/Img/sqlServer.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "SqlServer", DatabaseIcon = "/Img/sqlServer.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "SqlServer", DatabaseIcon = "/Img/sqlServer.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "SqlServer", DatabaseIcon = "/Img/sqlServer.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Hive", DatabaseIcon = "/Img/hive.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Hive", DatabaseIcon = "/Img/hive.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Hive", DatabaseIcon = "/Img/hive.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Hive", DatabaseIcon = "/Img/hive.png" });
            DatabaseCollection.Add(new DatabaseModel() { DatabaseName = "Hive", DatabaseIcon = "/Img/hive.png" });
        }
        private void InitModelTypeInfo()
        {
            ModelTypeCollection.Clear();
            ModelTypeCollection.Add(new ModelTypeModel() { ModelTypeName = "逻辑-物理模型", ModelTypeIcon = "/Img/icon-2.svg" });
            ModelTypeCollection.Add(new ModelTypeModel() { ModelTypeName = "概念模型", ModelTypeIcon = "/Img/icon-3.svg" });
            ModelTypeCollection.Add(new ModelTypeModel() { ModelTypeName = "主题域模型", ModelTypeIcon = "/Img/icon-1.svg" });
        }
    }
}
