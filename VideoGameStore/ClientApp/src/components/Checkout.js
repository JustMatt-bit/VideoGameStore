import React, { Component } from 'react';

export class Checkout extends Component {
    static displayName = Checkout.name;

    constructor(props) {
        super(props);
        this.state = {
            order_id: -1, cart_items: [], cart_total_price: 0, loading: true,
            userData: '',
            discounts: [],
            selectedDiscount: null
        };
        this.applyDiscount = this.applyDiscount.bind(this);
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const username = authCookie.split('=')[1];
            this.populateCart();
            this.populateUserData();
            this.fetchDiscounts(username);
        }
    }

    async fetchDiscounts(name) {
        const response = await fetch(`api/discount/userDiscounts/${name}`);
        const discounts = await response.json();
        this.setState({ discounts });
    }

    static renderCheckout(userData) {
        return (
            <div>
                <h2>Payment information</h2>
                <form>
                    <label>Name: </label><br />
                    <input type="text" value={userData.name} /><br />
                    <label>Surname: </label><br />
                    <input type="text" value={userData.surname} /><br />
                    <label>Email: </label><br />
                    <input type="email" value={userData.email} /><br />
                    <label>Phone: </label><br />
                    <input type="text" value={userData.phone} /><br />
                </form>

            </div>
        );
    }

    static renderCart(cart_items, cart_total_price) {
        let totalPrice = cart_items.reduce((total, item) => {
            return total + (item.price * item.units_in_cart);
        }, 0);
        return (
            <>
            <div style={{ padding: 10 }}>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Amount in cart</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    <tbody>
                        {cart_items.map(product =>
                            <tr key={product.id}>
                                <td><a style={{ color: 'black', textDecoration: 'none' }} href="/product">{product.name}</a></td>
                                <td>{product.units_in_cart}</td>
                                <td>{product.price}</td>
                            </tr>
                        )}
                    </tbody>
                </table>

                    <div style={{ textAlign: 'right' }}>
                        <h5>Total: {totalPrice.toFixed(2)}€</h5>
                    </div>
            </div>
            </>

        );
    }

    static renderDiscountDropdown(discounts, handleDiscountChange) {
        return (
            <select onChange={handleDiscountChange}>
                <option value="">Select a discount</option>
                {discounts.map(discount => (
                    <option key={discount.id} value={discount.id}>
                        {discount.percent}% Discount
                    </option>
                ))}
            </select>
        );
    }

    render() {
        let cartContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Checkout.renderCart(this.state.cart_items, this.state.cart_total_price);

        let userContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Checkout.renderCheckout(this.state.userData);

        let discountDropdown = this.state.loading
            ? <p><em>Loading discounts...</em></p>
            : Checkout.renderDiscountDropdown(this.state.discounts, this.handleDiscountChange);

        return (
            <div>
                <h1>Payment:</h1>

                <div style={{ display: 'flex', flexDirection: 'row' }}>
                    <div style={{ flex: 1, textAlign: 'center' }}>
                        <table border="1" style={{ borderWidth: 2 }}>{cartContents}</table>
                        <br></br>
                        {discountDropdown}
                        <button onClick={this.applyDiscount} style={{
                            backgroundColor: 'antiquewhite', // Green background
                            color: 'black', // White text
                            padding: '5px 10px', // Padding around the text
                            margin: '10px', // Margin around the button
                            border: 'none', // No border
                            borderRadius: '5px', // Slightly rounded corners
                            cursor: 'pointer', // Cursor pointer
                            fontSize: '16px', // Font size
                        }}>
                            Apply
                        </button>
                    </div>
                    <div style={{ flex: 2 }}>
                        {userContents}
                        <a href="/shipping">
                            <button style={{
                                textAlign: 'center',
                                backgroundColor: '#4CAF50', // Green background
                                color: 'white', // White text
                                padding: '5px 10px', // Padding around the text
                                margin: '10px', // Margin around the button
                                border: 'none', // No border
                                borderRadius: '5px', // Slightly rounded corners
                                cursor: 'pointer', // Cursor pointer
                                fontSize: '16px', // Font size
                            }}>
                                Continue
                            </button>
                        </a>
                    </div>
                </div>
            </div>
        );
    }

    handleDiscountChange = (event) => {
        const discountId = parseInt(event.target.value);
        const selectedDiscount = this.state.discounts.find(d => d.id === discountId);
        this.setState({ selectedDiscount }, () => {
            if (this.state.selectedDiscount) {
                this.applyDiscount();
            }
        });
    };

    applyDiscount = () => {
        console.log(this.state.selectedDiscount);
        if (this.state.selectedDiscount) {
            const discountFactor = 1 - this.state.selectedDiscount.percent / 100;
            const newTotalPrice = this.state.cart_items.reduce((total, item) => {
                return total + (item.price * item.units_in_cart);
            }, 0) * discountFactor;

            this.setState({ cart_total_price: newTotalPrice });
        }
    };

    async populateCart() {
        //var ls = localStorage;
        //var user = ls.getItem("UserID");
        var user = "JonasPonas";
        const response = await fetch(`api/cart/${user}`);
        const data = await response.json();
        var sum = 0;
        data[1].map(product => sum += product.price);
        this.setState({ order_id: data[0], cart_items: data[1], cart_total_price: sum, loading: false });
    }

    async populateUserData() {
        //var ls = localStorage;
        //var user = ls.getItem("UserID");
        var user = "JonasPonas";
        const response = await fetch(`api/checkout/${user}`); // !!!! CHANGE TO USER CONTROLLER !!!!!!
        const data = await response.json();
        this.setState({ userData: data, loading: false });
    }
}
