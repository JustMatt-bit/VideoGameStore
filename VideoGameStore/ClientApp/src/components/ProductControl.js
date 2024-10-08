﻿import React, { Component } from 'react';
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

async function handleProductDelete(id) {
    if (window.confirm("Confirm deletion") == true) {
        const response = await fetch('api/products/DeleteProductIfNotInUse', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(id),
        });
        const data = await response.json();
        if (data) {
            alert("Product deleted successfully.");
            window.location.href = '/product-control';
        }else{
            alert("Product is in use - can't be deleted. Changed to not sellable if it was sellable.");
            window.location.href = '/product-control';
        }
    }
    
}

export class ProductControl extends Component {
    static displayName = ProductControl.name;

    constructor(props) {
        super(props);
        this.state = { products: [], loading: true };
    }

    componentDidMount() {
        this.getUserType();
        this.populateVideoGameData();
        
    }

    async getUserType() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;

            await fetch(`/api/user/GetUserDetails/${username}`)
                .then(response => response.json())
                .then(data => {
                    this.setState({
                        userType: data.fk_user_type // Set userType based on API response
                    });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                });
            // Update the state with user information
            this.setState({ isLoggedIn: true, username, loading: false });
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false, loading: false });
        }
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
                            <td><Link style={{ color: 'black', textDecoration: 'none' }} to={{
                                pathname: "/product-edit",
                                search: `?${createSearchParams({
                                    id: product.id
                                })}`
                            }}>
                                Edit
                            </Link> <button onClick={() => handleProductDelete(product.id)}>Delete</button></td>
                        </tr>

                    )}

                </tbody>
            </table>
        );
    }

    static renderEmptyProductTable() {
        return (
            <p>No products</p>
        );
}

    render() {
        console.log(this.state);
        let contents;
        let startingContent = <>
            <h1 id="tabelLabel" >Product control</h1>
            <ToCreate />
            {!this.state.loading && (!this.state.isLoggedIn || this.state.isLoggedIn && this.state.userType !== 3) ? <></> : <> <ToCreateGenre />
            <ToDeleteGenres /></>}
           
        </>;
        if (this.state.products.length == 0) {
            contents = this.state.loading
                ? <p><em>Loading...</em></p>
                : <p style={{ textAlign: 'center', lineHeight: '100px' }}>No products</p>;
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
                {!this.state.loading && (!authCookie || authCookie && this.state.userType !== 2 && this.state.userType !== 3) ? <Navigate to="/fetch-products" replace={true} /> : <></>}
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
            
            this.setState({ products: data});
        }
        
    }
}
