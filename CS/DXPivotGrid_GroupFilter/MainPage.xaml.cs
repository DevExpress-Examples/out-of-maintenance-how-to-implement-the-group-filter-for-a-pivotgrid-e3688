using System.Windows.Controls;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using DevExpress.Xpf.PivotGrid;

namespace DXPivotGrid_GroupFilter {
    public partial class MainPage : UserControl {
        string dataFileName = "DXPivotGrid_GroupFilter.nwind.xml";
        public MainPage() {
            InitializeComponent();

            // Parses an XML file and creates a collection of data items.
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(dataFileName);
            XmlSerializer s = new XmlSerializer(typeof(OrderData));
            object dataSource = s.Deserialize(stream);

            // Binds a pivot grid to this collection.
            pivotGridControl1.DataSource = dataSource;
        }
        private void LayoutRoot_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            fieldMonth.CollapseAll();
        }
        private void listBox1_SelectedIndexChanged(object sender, System.Windows.RoutedEventArgs e) {
            if (pivotGridControl1 == null) return;
            this.pivotGridControl1.BeginUpdate();
            PivotGridGroup group = this.pivotGridControl1.Groups[0];
            group.FilterValues.Reset();
            group.FilterValues.BeginUpdate();
            switch (listBox1.SelectedIndex) {
                case 0:
                    group.FilterValues.FilterType = FieldFilterType.Excluded;
                    break;
                case 1:
                    group.FilterValues.FilterType = FieldFilterType.Included;
                    group.FilterValues.Values.Add(1994).ChildValues.Add(12);
                    group.FilterValues.Values.Add(1995).ChildValues.Add(1);
                    break;
                case 2:
                    group.FilterValues.FilterType = FieldFilterType.Excluded;
                    group.FilterValues.Values.Add(1994);
                    break;
                case 3:
                    group.FilterValues.FilterType = FieldFilterType.Included;
                    SelectFirstDays(group);
                    break;
            }
            group.FilterValues.EndUpdate();
            this.pivotGridControl1.EndUpdate();
            if (listBox1.SelectedIndex == 3)
                fieldMonth.ExpandAll();
            else
                fieldMonth.CollapseAll();
        }
        void SelectFirstDays(PivotGridGroup group) {
            foreach (object year in group.GetUniqueValues(null)) {
                DevExpress.XtraPivotGrid.PivotGroupFilterValue value =
                    group.FilterValues.Values.Add(year);
                foreach (object month in group.GetUniqueValues(new object[] { year })) {
                    value.ChildValues.Add(month).ChildValues.Add(1);
                }
            }
        }

    }
}