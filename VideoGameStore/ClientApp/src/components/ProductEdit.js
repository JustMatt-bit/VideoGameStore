import React, { Component } from 'react';

export class ProductEdit extends Component {
    static displayName = ProductEdit.name;
    render() {

        return (

            <div>
                <h2>Product editing</h2>
                <form >
                    <label>Name: </label><br />
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
                    <button type="submit"><a style={{ color: 'black', textDecoration: 'none' }} href="/product-control">Confirm edit</a></button>
                </form>

            </div>

        );
    }
}