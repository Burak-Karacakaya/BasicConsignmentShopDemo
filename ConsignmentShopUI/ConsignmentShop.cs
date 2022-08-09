using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        
        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();

        private decimal storeProfit = 0;



        public ConsignmentShop()
        {
            InitializeComponent();
            SetupData();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListbox.DataSource = itemsBinding;

            itemsListbox.DisplayMember = "Display";
            itemsListbox.ValueMember = "Display";


            cartBinding.DataSource = shoppingCartData;
            shoppingCartListbox.DataSource = cartBinding;

            shoppingCartListbox.DisplayMember = "Display";
            shoppingCartListbox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorListbox.DataSource = vendorsBinding;

            vendorListbox.DisplayMember = "Display";
            vendorListbox.ValueMember = "Display";

            
        }

        private void SetupData()
        {

            store.Vendors.Add(new Vendor { FirstName = "Burak", LastName = "Oceans" });
            store.Vendors.Add(new Vendor { FirstName = "Beykay", LastName = "Jones" });
            store.Vendors.Add(new Vendor { FirstName = "Jack", LastName = "Jones" });

            store.Items.Add(new Item
            {
                Title = "Crime and Punishment",
                Description = "Crime and Punishment follows " +
                "the mental anguish and moral dilemmas of Rodion Raskolnikov, " +
                "an impoverished ex-student in Saint Petersburg who plans to kill an " +
                "unscrupulous pawnbroker, an old woman " +
                "who stores money and valuable objects in her flat.",
                Price = 24.50M,
                Owner = store.Vendors[0]
            });

            store.Items.Add(new Item
            {
                Title = "Les Miserables",
                Description = "Les Misérables is a show about courage, " +
                "love, heartbreak, passion, " +
                "and the resilience of the human spirit—themes which undoubtedly transcend time and place. " +
                "Perhaps the most relevant themes, however, are related to the dignity of the human person.",
                Price = 19.50M,
                Owner = store.Vendors[1]
            });

            store.Items.Add(new Item
            {
                Title = "A Tale of Two Cities",
                Description = "A book about a revolution",
                Price = 9.50M,
                Owner = store.Vendors[1]
            });

            store.Items.Add(new Item
            {
                Title = "Harry Potter Book 1",
                Description = "A book about a boy and magic",
                Price = 14.50M,
                Owner = store.Vendors[0]
            });

            store.Name = "Seconds are Better";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            // Figure out what is selected from the items list
            // Copy that tem to the shopping cart
            // Do we remove the item from the items list? - No

            Item selectedItem = (Item)itemsListbox.SelectedItem;

            shoppingCartData.Add(selectedItem);

            cartBinding.ResetBindings(false);

        }

        private void makePurchae_Click(object sender, EventArgs e)
        {
            // Mark each item in the cars as sold
            // Clear the cart

            foreach (Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price;
                storeProfit += ( 1 - (decimal)item.Owner.Commission) * item.Price;

            }

            shoppingCartData.Clear();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitValue.Text = string.Format("${0}", storeProfit);

            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);
        }
    }
}
