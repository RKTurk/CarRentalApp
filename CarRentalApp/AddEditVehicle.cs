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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private readonly ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities carRentalEntities;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            carRentalEntities = new CarRentalEntities();
        }
        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                carRentalEntities = new CarRentalEntities();
                PopulateFields(carToEdit);
            }

        }
        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLPN.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Added Validation for make and model
                if (string.IsNullOrWhiteSpace(tbMake.Text) ||
                        string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Please ensure that you provide a make and a model");
                }
                else
                {
                    if (isEditMode)
                    {
                        //Edit Code here
                        var id = int.Parse(lblId.Text);
                        var car = carRentalEntities.TypesOfCars.FirstOrDefault(q => q.id == id);
                        car.Model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicensePlateNumber = tbLPN.Text;


                    }
                    else
                    {
                        //Added validation for make and model of cars being added

                        // Add Code Here
                        var newCar = new TypesOfCar
                        {
                            LicensePlateNumber = tbLPN.Text,
                            Make = tbMake.Text,
                            Model = tbModel.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text)
                        };

                        carRentalEntities.TypesOfCars.Add(newCar);

                    }
                    carRentalEntities.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation Completed!");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
