import React, { Component } from 'react';

export class FetchProducts extends Component {
    static displayName = FetchProducts.name;

    constructor(props) {
        super(props);
        this.state = { products: [], loading: true };
    }

    componentDidMount() {
        this.populateVideoGameData();
    }

    static renderProductTable(products) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
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
                    
                    {products.map(product =>
                        
                        <tr key={product.id}>
                            
                            <td><a style={{ color: 'black', textDecoration: 'none' }} href="/product">{product.name}</a></td>
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
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchProducts.renderProductTable(this.state.products);
        if (this.state.products.length == 0) {
            contents = <p style={{ textAlign: 'center', lineHeight: '100px' }}>Nėra produktų</p>;
        }
        return (
            <div>
                <h1 id="tabelLabel" >Produktai</h1>
                <p>Komponentas (puslapis), rodantis MySQL integraciją.</p>
                {contents}
            </div>
        );
    }

    async populateVideoGameData() {
        const response = await fetch('api/products/get');
        const data = await response.json();
        this.setState({ products: data, loading: false });
    }
}
