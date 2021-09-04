using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities carRentalEntities;
        public ManageRentalRecords()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord();
            addRentalRecord.MdiParent = this.MdiParent;
            addRentalRecord.Show();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            //get Id of selected row

            //give me the first selected row and the cell called id
            //and whatever value is
            var id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;

            //Query database for the record
            var record = carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.id == id);


            var addEditRentalRecord = new AddEditRentalRecord(record);
            addEditRentalRecord.MdiParent = this.MdiParent;
            addEditRentalRecord.Show();
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;

                //Query database for the record
                var record = carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.id == id);

                //Delete the vehicle from the table
                carRentalEntities.CarRentalRecords.Remove(record);
                carRentalEntities.SaveChanges();
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var records = carRentalEntities.CarRentalRecords
                .Select(q => new
                {
                    Customer = q.CustomerName,
                    DateOut = q.DateRented,
                    DateIn = q.DateReturned,
                    id = q.id,
                    q.Cost,
                    Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model
                }).ToList();

            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";

            //Hide the column for ID. 
            gvRecordList.Columns["id"].Visible = false;

        }
        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
