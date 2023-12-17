import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';

function ToCreate() {
    const navigate = useNavigate();
    const navigateToProductCreate = () => {
        navigate('/product-create');
    };
    return (
        <div>
            <button onClick={navigateToProductCreate}>Create product</button>
        </div>
    );
}

function ToProducts() {
    const navigate = useNavigate();
    const navigateToProduct = () => {
        navigate('/product');
    };
    return (
        <div>
            navigateToProduct
        </div>
    );
}

function ToEdit() {
    const navigate = useNavigate();
    const navigateToProductEdit = () => {
        navigate('/product-edit');
    };
    return (
        <div>
            <button onClick={navigateToProductEdit}>Change</button>
        </div>
    );
}

function ToCreateGenre() {
    const navigate = useNavigate();
    const navigateToGenreCreate = () => {
        navigate('/genre-create');
    };
    return (
        <div>
            <button onClick={navigateToGenreCreate}>Create genre</button>
        </div>
    );
}

function ToDeleteGenres() {
    const navigate = useNavigate();
    const navigateToGenresDelete = () => {
        navigate('/genres-delete');
    };
    return (
        <div>
            <button onClick={navigateToGenresDelete}>Delete genres</button>
        </div>
    );
}

export class ProductControl extends Component {
    static displayName = ProductControl.name;

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
                        <th>Creation date</th>
                        <th>Developer</th>
                        <th>Seller</th>
                        <th>Stock</th>
                        <th>Price</th>
                        <th>Sellable</th>
                        <th>Type</th>
                        <th>Actions</th>
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
                            <td><ToEdit /> <button>Delete</button></td>
                        </tr>

                    )}

                </tbody>
            </table>
        );
    }

    static renderEmptyProductTable() {
        return (
            <p>No items</p>
        );
}

    render() {
        let contents;
        let startingContent = <>
            <h1 id="tabelLabel" >Product control</h1>
            <ToCreate />
            <ToCreateGenre />
            <ToDeleteGenres />
        </>;
        if (this.state.products.length == 0) {
            contents = this.state.loading
                ? <p><em>Loading...</em></p>
                : <p style={{ textAlign: 'center', lineHeight: '100px' }}>No items</p>;
        } else { 
            contents = this.state.loading
                ? <p style={{ textAlign: 'center', lineHeight: '100px' }}><em>Loading...</em></p>
                : ProductControl.renderProductTable(this.state.products);
        }
        contents = <>
            {startingContent }
            {contents}
        </>;
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (!authCookie) {
            contents = <Navigate to="/fetch-products" replace={true} />
        }
        return (
            <div>
                
                {contents}
                
            </div>
        );
    }

    async populateVideoGameData() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));
        
        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;
            const response = await fetch(`/api/products/GetUserProducts/${username}`);
            const data = await response.json();
            
            this.setState({ products: data, loading: false });
        }
        
    }
}
