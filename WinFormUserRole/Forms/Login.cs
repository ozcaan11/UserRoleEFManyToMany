using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using WinFormUserRole.Helper;
using WinFormUserRole.Models;

namespace WinFormUserRole
{
    public partial class frmLogin : Form
    {
        private readonly myDatabase _db = new myDatabase();
        private User _user;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Veritabanına kayıtlı rolleri getir checkedListBoxa aktar
            foreach (var role in _db.Roles.Select(x => x.name).ToList())
            {
                checkedListBoxAddRoles.Items.Add(role, false);
            }

            // Veritabanından kullanıcıları getir
            comboBoxUsers.DataSource = _db.Users.Select(x => new { x.Id, x.username }).ToList();
            comboBoxUsers.ValueMember = "Id";
            comboBoxUsers.DisplayMember = "username";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Bu bilgilerdeki kullanıcıyı getir
            _user = _db.Users.FirstOrDefault(x => x.username == txtUser.Text && x.password == txtPass.Text);

            if (_user == null) return;

            // Bu kullanıcıya ait rollerin adını getir
            var roles = _user.Roles.Select(x => x.name).ToList();
            lblUsername.Text = _user.username;
            foreach (var role in roles)
            {
                lblRoles.Text += role + "\n";
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var user = new User()
            {
                username = txtUsername.Text,
                password = txtPassword.Text
            };

            foreach (var role in checkedListBoxAddRoles.CheckedItems)
            {
                // Direkt checkboxın textini al ekle dersek yeni kayıtmış gibi ekleyecek
                // O yüzden önce veritabanında role tablosunda bu isimde(chxboxın texti) bir rol var mı ona bakacağız
                // eğer kayıtlı ise ki kayıtlıdır bunu kullanıcının rolleri arasına ekle 
                var query = _db.Roles.FirstOrDefault(x => x.name == role.ToString());
                user.Roles.Add(query);
            }

            _db.Users.Add(user);
            _db.SaveChanges();
            Mesaj.Show("Tamamdır!");

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(comboBoxUsers.SelectedValue);
            var user = _db.Users.FirstOrDefault(x => x.Id == id);

            foreach (var role in checkedListBoxRoles.Items)
            {
                var query = _db.Roles.FirstOrDefault(x => x.name == role.ToString());
                user?.Roles.Add(query);
            }

            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            Mesaj.Show("Tamamdır");
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            // Role tablosuna yeni rol ekle
            var role = new Role()
            {
                name = txtRole.Text
            };
            _db.Roles.Add(role);
            _db.SaveChanges();
            MessageBox.Show("Tamamdır");
        }

        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBoxRoles.Items.Clear();

            // Burada combobox form load olduğunda null değer aldığı için try catch yaptık 
            // ki display ve normal value değerini alsınlar
            try
            {
                var id = Convert.ToInt32(comboBoxUsers.SelectedValue);

                // Rol tablosunu getir
                var roles = _db.Roles;

                // İçerden başlamak gerekir
                // Önce bu id deki firstordefault kullanıcıyı getir (x => x.Users.FirstOrDefault(y => y.Id == id) -- User tipinde oluyor yani )
                // Bu Kullanıcıyı ait rollerin adını getir  (Kullanıcının id  sine göre)
                var roleOfUsers = roles.Where(x => x.Users.FirstOrDefault(y => y.Id == id).Id == id).Select(x => x.name).ToList();

                foreach (var role in roles.ToList())
                {
                    checkedListBoxRoles.Items.Add(role.name, roleOfUsers.Contains(role.name));
                }

            }
            catch (Exception)
            {
                //
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            if (_user == null) return;

            // Kullanıcının rolleri arasında ListStudent varsa eğer listeleme yap
            dataGridViewStudent.DataSource = _user.Roles.Any(x => x.name.Contains("ListStudent")) ? _db.Students.Select(n => new { n.name, n.lastname }).ToList() : null;
        }
    }
}
