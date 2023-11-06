import React, { Component } from 'react';

export class ProductEdit extends Component {
    static displayName = ProductEdit.name;
    render() {

        return (

            <div>
                <h2>Produkto keitimas</h2>
                <form >
                    <label>Pavadinimas: </label><br />
                    <input type="text" /><br />
                    <label>Aprašymas: </label><br />
                    <input type="text" /><br />
                    <label>Išleidimo data: </label><br />
                    <input type="date" /><br />
                    <label>Kūrėjas: </label><br />
                    <input type="text" /><br />
                    <label>Pardavėjas: </label><br />
                    <input type="text" /><br />
                    <label>Kiekis: </label><br />
                    <input type="text" /><br />
                    <label>Kaina: </label><br />
                    <input type="text" /><br />
                    <label>Tipas: </label><br />
                    <input type="text" /><br />
                    <button type="submit"><a style={{ color: 'black', textDecoration: 'none' }} href="/product-control">Keisti</a></button>
                </form>

            </div>

        );
    }
}