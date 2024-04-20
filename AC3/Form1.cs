using AC4.Persistence.DAO;
using GestioAigua;
using System.Text.RegularExpressions;
using AC4.Persistence.Mapping;
using AC4.Persistence.Utils;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AC4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 2012; i <= 2050; i++)
            {
                cbAny.Items.Add(i);

            }
            Helper.CsvToXml();
            Dictionary<int, string> comarques = Helper.Comarques();
            cbComarca.DataSource = new BindingSource(comarques, null);
            cbComarca.DisplayMember = "Value";
            cbComarca.ValueMember = "Key";
            GenerarTaula(dataGridView1);
        }
        public static void GenerarTaula(DataGridView dg)
        {
            
            List<Record> records = Helper.Reader();
            dg.DataSource = records;
            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Regex digitos = new Regex(@"^\d+$");
            Regex decimales = new Regex(@"^\d+(\.\d{2})?$|^\d+$");
            if (!cbComarca.Items.Contains(cbComarca.SelectedItem) || !cbAny.Items.Contains(cbAny.SelectedItem) || !digitos.IsMatch(txtPoblacio.Text) || !digitos.IsMatch(txtXarxa.Text) || !digitos.IsMatch(txtAct.Text) || !digitos.IsMatch(txtTotal.Text) || !decimales.IsMatch(txtCapita.Text))
            {
                if (!digitos.IsMatch(txtPoblacio.Text))
                {
                    errorPoblacio.SetError(txtPoblacio, "Debe ser un numero mayor a 0 sin decimales");
                }
                if (!digitos.IsMatch(txtXarxa.Text))
                {
                    errorXarxa.SetError(txtXarxa, "Debe ser un numero mayor a 0 sin decimales");
                }
                if (!digitos.IsMatch(txtAct.Text))
                {
                    errorAct.SetError(txtAct, "Debe ser un numero mayor a 0 sin decimales");
                }
                if (!digitos.IsMatch(txtTotal.Text))
                {
                    errorTotal.SetError(txtTotal, "Debe ser un numero mayor a 0 sin decimales");
                }
                if (!decimales.IsMatch(txtCapita.Text))
                {
                    errorCapita.SetError(txtCapita, "Debe ser un numero mayor a 0 i como m�ximo con 2 decimales");
                }
                if (!cbComarca.Items.Contains(cbComarca.SelectedItem))
                {
                    errorComarca.SetError(cbComarca, "Debe seleccionar una comarca");
                }
                if (!cbAny.Items.Contains(cbAny.SelectedItem))
                {
                    errorAny.SetError(cbAny, "Debe seleccionar un a�o");
                }

            }
            //controla que el anyo y la comarca no se repitan
            else if (Helper.Repeticio((int)cbAny.SelectedItem, (int)cbComarca.SelectedValue))
            {
                MessageBox.Show("Ja existeix un registre per aquest any i comarca!");
            }
            else
            {
                IRecordDAO recordDAO = new RecordDAO(NpgsqlUtils.OpenConnection());

                Record record = new Record();
                record.Any = (int)cbAny.SelectedItem;
                record.CodiComarca = (int)cbComarca.SelectedValue;
                record.Comarca = cbComarca.Text;
                record.Poblacio = int.Parse(txtPoblacio.Text);
                record.DomesticXarxa = int.Parse(txtXarxa.Text);
                record.ActivitatsEconomiques = int.Parse(txtAct.Text);
                record.Total = int.Parse(txtTotal.Text);
                record.ConsumDomesticPerCapita = double.Parse(txtCapita.Text);
                Helper.Append(record);
                GenerarTaula(dataGridView1);
                errorAct.Clear();
                errorCapita.Clear();
                errorPoblacio.Clear();
                errorTotal.Clear();
                errorXarxa.Clear();
                errorComarca.Clear();
                errorAny.Clear();
                errorPoblacio.Clear();
                try
                {
                    recordDAO.AddRecord(record);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                /*RecordCRUD recordCRUD = new RecordCRUD();
                recordCRUD.InsertRecord(record);
                */
                Helper.CsvToXml();
                
            }
        }

        private void btnNetejaar_Click(object sender, EventArgs e)
        {
            cbAny.SelectedIndex = -1;
            cbComarca.SelectedIndex = -1;
            txtAct.Text = "";
            txtCapita.Text = "";
            txtPoblacio.Text = "";
            txtTotal.Text = "";
            txtXarxa.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int n = e.RowIndex;
            if (n != -1)
            {

                lblMitja.Text = Helper.CalcConsum(dataGridView1.Rows[n].Cells[2].Value.ToString());
                lblPob.Text = Convert.ToInt32(dataGridView1.Rows[n].Cells[3].Value) > 200000 ? "Si" : "No";
                lblAlt.Text = Helper.ConsDomesticAlt(dataGridView1.Rows[n].Cells[0].Value.ToString(), dataGridView1.Rows[n].Cells[2].Value.ToString()) == dataGridView1.Rows[n].Cells[2].Value.ToString() ? "Si" : "No";
                lblBaix.Text = Helper.ConsDomesticBaix(dataGridView1.Rows[n].Cells[0].Value.ToString(), dataGridView1.Rows[n].Cells[2].Value.ToString()) == dataGridView1.Rows[n].Cells[2].Value.ToString() ? "Si" : "No";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
