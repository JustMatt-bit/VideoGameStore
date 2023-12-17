import React, { Component } from 'react';

export class ProductCreate extends Component {
    static displayName = ProductCreate.name;
    render() {

        return (

            <div>
                <h2>Product creation</h2>
                <form >
                    <label>Name: </label><br/>
                    <input type="text" /><br />
                    <label>Description: </label><br />
                    <input type="text" /><br />
                    <label>Release date: </label><br />
                    <input type="date" /><br />
                    <label>Developer: </label><br />
                    <input type="text" /><br />
                    <label>Seller: </label><br />
                    <input type="text" /><br />
                    <label>Amount: </label><br />
                    <input type="text" /><br />
                    <label>Price: </label><br />
                    <input type="text" /><br />
                    <label>Type: </label><br />
                    <input type="text" /><br />
                    <button type="submit"><a style={{ color: 'black', textDecoration: 'none' }} href="/product-control">Create</a></button>
                </form>

            </div>

        );
    }
}