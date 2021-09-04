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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities carRentalEntities;
        public ManageVehicleListing()
        {
            InitializeComponent();
            carRentalEntities = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            //Aliasing in LINQ to select data of particular columns
            //Select id as CarId, name as CarName from TypesOfCars
            //var cars = carRentalEntities.TypesOfCars
            //    .Select(q => new { CarId = q.id, CarName = q.Make})
            //    .ToList();
            var cars = carRentalEntities.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.id
                })
                .ToList();
            gvVehicleList.DataSource = cars;
            //gvVehicleList.Columns[0].HeaderText = "ID";
            //gvVehicleList.Columns[1].HeaderText = "NAME";
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            gvVehicleList.Columns[5].Visible = false;
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            AddEditVehicle addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            //get Id of selected row

            //give me the first selected row and the cell called id
            //and whatever value is
            var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;

            //Query database for the record
            var car = carRentalEntities.TypesOfCars.FirstOrDefault(q => q.id == id);


            //Launch AddEditVehicle with Data
            var addEditVehicle = new AddEditVehicle(car, this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                //get Id of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;

                //Query database for the record
                var car = carRentalEntities.TypesOfCars.FirstOrDefault(q => q.id == id);

                DialogResult dr = MessageBox.Show("Are You Sure You Want To Delete this Record?",
                    "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    //Delete the vehicle from the table
                    carRentalEntities.TypesOfCars.Remove(car);
                    carRentalEntities.SaveChanges();
                }
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
        //New Function to PopulateGrid. Can be called anytime we need a grid refresh
        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var cars = carRentalEntities.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.id
                }).ToList();
            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            //Hide the column for ID. 
            gvVehicleList.Columns["id"].Visible = false;
        }

    }
}
