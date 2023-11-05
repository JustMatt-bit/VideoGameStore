import React, { Component } from 'react';

export class Cart extends Component {
    static displayName = Cart.name;

    constructor(props) {
        super(props);
        this.state = { cart_items: [], cart_total_price: 0, loading: true };
    }

    componentDidMount() {
        this.populateCart();
    }

    static renderCartTable(cart_items, cart_total_price) {
        return (
            <><table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Pavadinimas</th>
                        <th>Aprašymas</th>
                        <th>Išleidimo data</th>
                        <th>Kūrėjas</th>
                        <th>Pardavėjas</th>
                        <th>Likutis</th>
                        <th>Kaina</th>
                        <th>Parduodamas</th>
                        <th>Tipas</th>
                    </tr>
                </thead>
                <tbody>
                    {cart_items.map(product =>
                        <tr key={product.id}>
                            <td>{product.name}</td>
                            <td>{product.description}</td>
                            <td>{product.release_date.split("T")[0]}</td>
                            <td>{product.developer_name}</td>
                            <td>{product.fk_account}</td>
                            <td>{product.stock}</td>
                            <td>{product.price}</td>
                            <td>{product.being_sold ? "Taip" : "Ne"}</td>
                            <td>{product.game_type_name}</td>
                        </tr>
                    )}
                </tbody>
            </table>
            <h4 style={{ textAlign: 'right' }}>Viso: {cart_total_price}€</h4></>

        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Cart.renderCartTable(this.state.cart_items, this.state.cart_total_price);

        return (
            <div>
                <h1 id="tabelLabel" >Esamas krepšelis:</h1>
                {contents}
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
        data.map(product => sum += product.price);
        this.setState({ cart_items: data, cart_total_price: sum, loading: false });
    }
}
