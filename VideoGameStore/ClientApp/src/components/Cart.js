import React, { Component } from 'react';

export class Cart extends Component {
    static displayName = Cart.name;

    constructor(props) {
        super(props);
        this.state = { order_id: -1, cart_items: [], cart_total_price: 0, loading: true };
    }

    componentDidMount() {
        this.populateCart();
    }

    itemsInCartChange(pid, newVal) {
        this.setState(prevState => ({
            cart_items: prevState.cart_items.map(item =>
                item.id === pid ? { ...item, units_in_cart: newVal } : item
            ),
        }));

        const data = { oid: this.state.order_id, pid: pid, val: newVal }
        fetch('api/cart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
    }

    static renderCartTable(cart_items, cart_total_price, itemsInCartChange) {
        return (
            <><table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Pavadinimas</th>
                        <th>Pardavėjas</th>
                        <th>Likutis</th>
                        <th>Kaina</th>
                        <th>Parduodamas</th>
                        <th>Tipas</th>
                        <th>Kiekis krepšelyje</th>
                    </tr>
                </thead>
                <tbody>
                    {cart_items.map(product =>
                        <tr key={product.id}>
                            <td><a style={{ color: 'black', textDecoration: 'none' }} href="/product">{product.name}</a></td>
                            <td>{product.fk_account}</td>
                            <td>{product.stock}</td>
                            <td>{product.price}</td>
                            <td>{product.being_sold ? "Taip" : "Ne"}</td>
                            <td>{product.game_type_name}</td>
                            <td style={{ textAlign: 'center' }}><input type="number" value={product.units_in_cart} style={{ width: 50, textAlign: 'center', paddingLeft: 15, borderRadius: 4}} min="0" max={product.stock} onChange={(e) => itemsInCartChange(product.id, parseInt(e.target.value))} /></td>
                        </tr>
                    )}
                </tbody>
            </table>
            <br></br>
            <div style={{ textAlign: 'right' }}>
                    <h4>Viso: {cart_total_price}€</h4>
                    <a href="/checkout"> <button
                        style={{
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
                        href="/checkout"
                    >
                        Mokėti
                    </button></a>
            </div>
            </>
            
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Cart.renderCartTable(this.state.cart_items, this.state.cart_total_price, this.itemsInCartChange.bind(this));

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
        data[1].map(product => sum += product.price);
        this.setState({ order_id:data[0], cart_items: data[1], cart_total_price: sum, loading: false });
    }
}
