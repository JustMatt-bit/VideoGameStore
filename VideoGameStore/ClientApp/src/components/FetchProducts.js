import React, { Component } from 'react';
import { createSearchParams, Link } from "react-router-dom";

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
                        <th>Name</th>
                        <th>Description</th>
                        <th>Release date</th>
                        <th>Developer</th>
                        <th>Seller</th>
                        <th>Amount in stock</th>
                        <th>Price</th>
                        <th>Is sold?</th>
                        <th>Type</th>
                    </tr>
                </thead>
                <tbody>
                    
                    {products.map(product =>
                        
                        <tr key={product.id}>
                            
                            <td><Link style={{ color: 'black', textDecoration: 'none' }} to={{
                                pathname: "/product",
                                search: `?${createSearchParams({
                                    id: product.id
                                })}`
                            }}>
                                {product.name}
                            </Link></td>
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
        let contents;
        let startingContent = <h1 id="tabelLabel" >Products</h1>;
        if (this.state.products.length == 0) {
            contents = this.state.loading
                ? <p><em>Loading...</em></p>
                : <p style={{ textAlign: 'center', lineHeight: '100px' }}>No products</p>;
        } else {
            contents = this.state.loading
                ? <p style={{ textAlign: 'center', lineHeight: '100px' }}><em>Loading...</em></p>
                : FetchProducts.renderProductTable(this.state.products);
        }
        contents = <>
            {startingContent}
            {contents}
        </>;
        return (
            <div>
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
