import React, { Component } from 'react';

export class Checkout extends Component {
    static displayName = Checkout.name;

    constructor(props) {
        super(props);
        this.state = {
            order_id: -1, cart_items: [], cart_total_price: 0, loading: true,
            userData: ''
        };
    }

    componentDidMount() {
        this.populateCart();
        this.populateUserData();
    }

    static renderCheckout(userData) {
        return (
            <div>
                <h2>Mokėjimo informacija</h2>
                <form>
                    <label>Vardas: </label><br />
                    <input type="text" value={userData.name} /><br />
                    <label>Pavardė: </label><br />
                    <input type="text" value={userData.surname} /><br />
                    <label>Paštas: </label><br />
                    <input type="email" value={userData.email} /><br />
                    <label>Telefonas: </label><br />
                    <input type="text" value={userData.phone} /><br />
                </form>

            </div>
        );
    }

    static renderCart(cart_items, cart_total_price) {
        return (
            <>
            <div style={{ padding: 10 }}>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Pavadinimas</th>
                            <th>Kiekis krepšelyje</th>
                            <th>Kaina</th>
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

                <div style={{ textAlign: 'right'}}>
                    <h5>Viso: {cart_total_price}€</h5>
                </div>
            </div>
            </>

        );
    }

    render() {
        let cart = this.state.loading
            ? <p><em>Loading...</em></p>
            : Checkout.renderCart(this.state.cart_items, this.state.cart_total_price);

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Checkout.renderCheckout(this.state.userData);

        return (
            <div>
                <h1>Atsiskaitymas:</h1>

                <div style={{ display: 'flex', flexDirection: 'row' }}>
                    <div style={{ flex: 1, textAlign: 'center' }}>
                        <table border="1" style={{ borderWidth: 2 }}>{cart}</table>
                        <br></br>
                        <input type="text"></input>
                        <button
                            style={{
                                backgroundColor: 'antiquewhite', // Green background
                                color: 'black', // White text
                                padding: '5px 10px', // Padding around the text
                                margin: '10px', // Margin around the button
                                border: 'none', // No border
                                borderRadius: '5px', // Slightly rounded corners
                                cursor: 'pointer', // Cursor pointer
                                fontSize: '16px', // Font size
                            }}
                            onMouseOver={(e) => e.target.style.backgroundColor = 'whitesmoke'} // Darker green on hover
                            onMouseOut={(e) => e.target.style.backgroundColor = 'antiquewhite'} // Original color when not hovered
                        >
                            Pritakyti
                        </button>
                    </div>
                    <div style={{ flex: 2 }}>
                        {contents}
                        <a href="/shipping"> <button
                            style={{
                                textAlign: 'center',
                                backgroundColor: '#4CAF50', // Green background
                                color: 'white', // White text
                                padding: '5px 10px', // Padding around the text
                                margin: '10px', // Margin around the button
                                border: 'none', // No border
                                borderRadius: '5px', // Slightly rounded corners
                                cursor: 'pointer', // Cursor pointer
                                fontSize: '16px', // Font size
                            }}
                            onMouseOver={(e) => e.target.style.backgroundColor = '#45a049'} // Darker green on hover
                            onMouseOut={(e) => e.target.style.backgroundColor = '#4CAF50'} // Original color when not hovered
                        >
                            Tęsti
                        </button></a>
                    </div>
                </div>
            </div>
        );
    }

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
